using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPi_App.Data
{
	[Table("User")]
	public class User
	{
		[Key]
		public int IdUser { get; set; }

		[Required]
		[MaxLength(50)]
		public string Username { get; set; } = string.Empty;

		[Required]
		[MaxLength(250)]
		public string Password { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;
	}
}
