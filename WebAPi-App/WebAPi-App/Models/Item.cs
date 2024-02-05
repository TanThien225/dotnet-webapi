namespace WebAPi_App.Models
{
	public class ItemVM
	{
		public string Name { get; set; } = string.Empty;
		public double Price { get; set; }
	}
	
	public class Item : ItemVM
	{
		public Guid IdItem { get; set; } 
		
	}

	public class ItemModel
	{
		public Guid IdItem { get; set; }
		public string Name { get; set; } = string.Empty;
		public double Price { get; set; }
		public string CategoryName { get; set; } = string.Empty;
	}
}
