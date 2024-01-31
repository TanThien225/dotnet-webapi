namespace WebAPi_App.Data
{
	public enum OrderStatus
	{
		New = 0, Payment = 1, Complete = 2, Cancel = -1
	}
	public class Order
	{
		public Guid OrderId { get; set; }
		public DateTime OrderDate { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public OrderStatus OrderStatusInfo { get; set; }	
		public string ReceiverName { get; set; } = string.Empty;
		public string ReceiverAddress { get; set; } = string.Empty;
		public string MobilePhone { get; set; } = string.Empty;


		public ICollection<OrderDetail> OrderDetails { get; set; }
		public Order()
		{
			OrderDetails = new List<OrderDetail>();
		}
	}
}
