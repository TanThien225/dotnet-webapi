using System.ComponentModel.DataAnnotations;

namespace WebAPi_App.Models
{
	public class CategoryModel
	{
		[Required]
		[MaxLength(50)]
		public string CategoryName { get; set; } = string.Empty;
	}
}
