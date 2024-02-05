using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using WebAPi_App.Data;
using WebAPi_App.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace WebAPi_App.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly MyDBContext _context;

		private readonly AppSetting _appSettings;

		public UserController(MyDBContext context, IOptionsMonitor<AppSetting> optionsMonitor)
		{
			_context = context;
			_appSettings = optionsMonitor.CurrentValue;
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Validate(LoginModel model)
		{
			var user = _context.Users.SingleOrDefault(p => p.Username == model.Username
													&& p.Password == model.Password);
			if (user == null)
			{
				return Ok(new ApiResponse
				{
					Success = false,
					Message = "Invalid username/password"
				});
			}

			//Cấp token
			var token = await GenerateToken(user);

			return Ok(new ApiResponse
			{
				Success = true,
				Message = "Authenticated successfully",
				Data = token
			});
		}

		//Generate token
		private async Task<TokenModel> GenerateToken(User user)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();

			var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

			var tokenDescription = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim (ClaimTypes.Name, user.Name),
					new Claim (JwtRegisteredClaimNames.Email, user.Email),
					new Claim (JwtRegisteredClaimNames.Sub, user.Email),
					new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new Claim ("Username", user.Username),
					new Claim ("IdUser", user.IdUser.ToString()),

					//roles
					//------later

					//new Claim("TokenId", Guid.NewGuid().ToString())
				}),
				Expires = DateTime.UtcNow.AddSeconds(60),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(secretKeyBytes),
					SecurityAlgorithms.HmacSha256Signature)
				//HmacSha512Signature
			};
			var token = jwtTokenHandler.CreateToken(tokenDescription);

			var accessToken = jwtTokenHandler.WriteToken(token);
			var refreshToken = GenerateRefreshToken();

			//Save to database refreshtoken entity

			var refreshTokenEntity = new RefreshToken
			{
				Id = Guid.NewGuid(),
				JwtId = token.Id,
				UserId = user.IdUser,
				Token = refreshToken,
				IsUsed = false,
				IsRevoked = false,
				IssuedAt = DateTime.UtcNow,
				ExpiredAt = DateTime.UtcNow.AddHours(1),
			};

			await _context.AddAsync(refreshTokenEntity);
			await _context.SaveChangesAsync();

			return new TokenModel
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken
			};
		}

		private string GenerateRefreshToken()
		{
			var random = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(random);

				return Convert.ToBase64String(random);
			}
		}

		[HttpPost("RenewToken")]
		public async Task<IActionResult> RenewToken(TokenModel tokenModel)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

			var tokenValidationParam = new TokenValidationParameters
			{
				//Tu cap token
				ValidateIssuer = false,
				ValidateAudience = false,

				//ký vào token
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

				ClockSkew = TimeSpan.Zero,

				//ỨNg vơi trường hợp renew không cần validatetime
				ValidateLifetime = false,  //không kiểm tra token hết hạn
			};

			try
			{
				//Check 1: accesstoken valid format
				var tokenInVerification = jwtTokenHandler.ValidateToken(tokenModel.AccessToken,
					tokenValidationParam, out var validatedToken);

				//Check 2: Algorithm secure 
				if (validatedToken is JwtSecurityToken jwtSecurityToken)
				{
					var result = jwtSecurityToken.Header.Alg.Equals
						(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

					//algo is wrong
					if (!result)
					{
						return Ok(new ApiResponse
						{
							Success = false,
							Message = "Invalid Token",
						});
					}
				}

				//Check 3: Check accesstoken expire? 

				//Duyet danh sach claims
				var utcExpireDate = long.Parse(tokenInVerification.Claims.
					FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

				var expireDate = ConvertUnixTimeToDatetime(utcExpireDate);

				//greater still have time to use token
				if (expireDate > DateTime.UtcNow)
				{
					return Ok(new ApiResponse
					{
						Success = false,
						Message = "Access token has not yet expired.",
					});
				}

				//Check 4: Check refreshtoken exist in DB
				var storedToken = _context.RefreshTokens.
					FirstOrDefault(tk => tk.Token == tokenModel.RefreshToken);

				if (storedToken == null) 
				{
					return Ok(new ApiResponse
					{
						Success = false,
						Message = "Refresh token does not exist",
					});
				}

				//Check 5: check refresh token is used/ revoked?
				if (storedToken.IsUsed)
				{
					return Ok(new ApiResponse
					{
						Success = false,
						Message = "Refresh token has been used",
					});
				}

				if (storedToken.IsRevoked)
				{
					return Ok(new ApiResponse
					{
						Success = false,
						Message = "Refresh token has been revoked",
					});
				}

				//Check 6:  Accesstoken id == jwtId in refreshtoken
				var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

				if(storedToken.JwtId != jti)
				{
					return Ok(new ApiResponse
					{
						Success = false,
						Message = "Token does not match.",
					});
				}

				//Update token is used
				storedToken.IsRevoked = true;
				storedToken.IsUsed = true;
				_context.Update(storedToken);
				await _context.SaveChangesAsync();


				//Finally pass all
				//Create new token
				var user = await _context.Users.SingleOrDefaultAsync(us => us.IdUser == storedToken.UserId);

				var token = await GenerateToken(user);

				return Ok(new ApiResponse
				{
					Success = true,
					Message = "Renew token successfully",
					Data = token
				});

			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Message = "Something went wrong",
				});
			}
		}

		private DateTime ConvertUnixTimeToDatetime(long utcExpireDate)
		{
			var datetimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			datetimeInterval = datetimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

			return datetimeInterval;
		}
	}
}
