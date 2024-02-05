using Microsoft.EntityFrameworkCore;

namespace WebAPi_App.Data
{
	public class MyDBContext : DbContext
	{
		public MyDBContext(DbContextOptions options) : base(options) { }

		#region DBset
		public DbSet<Item> Items { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<User> Users { get; set; }
		
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		#endregion

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Order>(e =>
			{
				e.ToTable("Order");
				e.HasKey(ord => ord.OrderId);
				e.Property(ord => ord.OrderDate).HasDefaultValueSql("GETUTCDATE()");
				e.Property(ord => ord.ReceiverName).IsRequired().HasMaxLength(100);
			});

			modelBuilder.Entity<OrderDetail>(entity =>
			{
				entity.ToTable("OrderDetail");
				//2 khoa chinh orderid va id(item)
				entity.HasKey(e => new {e.OrderId, e.Id});

				entity.HasOne(e => e.order)
					.WithMany(e => e.OrderDetails)
					.HasForeignKey(e => e.OrderId)
					.HasConstraintName("FK_OrderDetail_Order");
				
				entity.HasOne(e => e.item)
					.WithMany(e => e.OrderDetails)
					.HasForeignKey(e => e.Id)
					.HasConstraintName("FK_OrderDetail_Item");
			});

			modelBuilder.Entity<User>(entity =>
			{
				entity.HasIndex(e => e.Username).IsUnique();
				entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
			});
		}
	}
}
