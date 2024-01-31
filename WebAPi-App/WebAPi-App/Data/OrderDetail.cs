namespace WebAPi_App.Data
{
	public class OrderDetail
	{
		public Guid Id { get; set; }
		public Guid OrderId { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
		public byte OnSale { get; set; }

		//Relationship
		public Order order { get; set; } 
		public Item item { get; set; }
	}
}
