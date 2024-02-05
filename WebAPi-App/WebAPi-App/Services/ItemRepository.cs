using Microsoft.EntityFrameworkCore;
using WebAPi_App.Data;
using WebAPi_App.Models;
using Item = WebAPi_App.Data.Item;

namespace WebAPi_App.Services
{
	public class ItemRepository : IItemRepository
	{
		private readonly MyDBContext _context;

		public static int PAGE_SIZE { get; set; } = 3;

		public ItemRepository(MyDBContext context)
		{
			_context = context;
		}
		public List<ItemModel> GetAll(string? search, double? from, double? to, string? sortBy, int page = 1)
		{
			var allProducts = _context.Items.Include(pro => pro.Category).AsQueryable();

			#region Filtering
			if (!string.IsNullOrEmpty(search))
			{
				allProducts = allProducts.Where(i => i.Name.Contains(search));
			}

			if (from.HasValue)
			{
				allProducts = allProducts.Where(pro => pro.Price >= from);
			}

			if (to.HasValue)
			{
				allProducts = allProducts.Where(pro => pro.Price <= to);
			}
			#endregion

			#region Sorting
			//Default sort by Name
			allProducts = allProducts.OrderBy(pro => pro.Name);

			if (!string.IsNullOrEmpty(sortBy))
			{
				switch (sortBy)
				{
					case "Name_desc":
						allProducts = allProducts.OrderByDescending(pro => pro.Name);
						break;
					case "Price_asc":
						allProducts = allProducts.OrderBy(pro => pro.Price);
						break;
					case "Price_desc":
						allProducts = allProducts.OrderByDescending(pro => pro.Price);
						break;
				}
			}
			#endregion

			#region Paging
			//allProducts = allProducts.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);

			#endregion

			//var result = allProducts.Select(pro => new ItemModel
			//{
			//	IdItem = pro.Id,
			//	Name = pro.Name,
			//	Price = pro.Price,
			//	CategoryName = pro.Category.CategoryName
			//});
			//return result.ToList();

			var result = PaginatedList<Item>.Create(allProducts, page, PAGE_SIZE);

			return result.Select(pro => new ItemModel
			{
				IdItem = pro.Id,
				Name = pro.Name,
				Price = pro.Price,
				CategoryName = pro.Category.CategoryName
			}).ToList();
		}
	}
}
