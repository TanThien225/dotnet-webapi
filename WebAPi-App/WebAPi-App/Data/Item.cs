using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPi_App.Data
{
	[Table("Item")]
	public class Item
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		[Range(0, double.MaxValue)]
		public double Price { get; set; }

		public byte OnSale { get; set; }

		public int? CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		public Category Category { get; set; }


		public ICollection<OrderDetail> OrderDetails { get; set; }
		public Item()
		{
			OrderDetails = new List<OrderDetail>();
		}
	}
	
	
}
