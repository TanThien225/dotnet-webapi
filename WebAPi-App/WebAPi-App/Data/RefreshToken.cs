using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPi_App.Data
{
	[Table("RefreshToken")]
	public class RefreshToken
	{
		[Key]
		public Guid Id { get; set; }

		public int UserId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }

		public string Token { get; set; } = string.Empty;
		public string JwtId { get; set; } = string.Empty;
		public bool IsUsed { get; set;}
		public bool IsRevoked { get; set;}
		public DateTime IssuedAt { get; set;}
		public DateTime ExpiredAt { get; set;}

	}
}
