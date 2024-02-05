using System.ComponentModel.DataAnnotations;

namespace WebAPi_App.Models
{
	public class LoginModel
	{

		[Required]
		[MaxLength(50)]
		public string Username { get; set; } = string.Empty;

		[Required]
		[MaxLength(250)]
		public string Password { get; set; } = string.Empty;
	}
}
