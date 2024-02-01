using WebAPi_App.Data;
using WebAPi_App.Models;

namespace WebAPi_App.Services
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly MyDBContext _context;

		public CategoryRepository(MyDBContext context)
		{
			_context = context;
		}

		public CategoryVM Add(CategoryModel categoryModel)
		{
			var category = new Category
			{
				CategoryName = categoryModel.CategoryName
			};
			_context.Add(category);
			_context.SaveChanges();
			return new CategoryVM
			{
				CategoryId = category.CategoryId,
				CategoryName = category.CategoryName
			};
		}

		public void Delete(int id)
		{
			var category = _context.Categories.SingleOrDefault(c => c.CategoryId == id);
			if (category != null)
			{
				_context.Remove(category);
				_context.SaveChanges();
			}
		}

		public List<CategoryVM> GetAll()
		{
			var categories = _context.Categories.Select(c => new CategoryVM
			{
				CategoryId = c.CategoryId,
				CategoryName = c.CategoryName,
			});
			return categories.ToList();
		}

		public CategoryVM getById(int id)
		{
			var category = _context.Categories.SingleOrDefault(c => c.CategoryId == id);
			if (category != null)
			{
				return new CategoryVM
				{
					CategoryId = category.CategoryId,
					CategoryName = category.CategoryName
				};
			}
			return null;
		}

		public void Update(CategoryVM categoryVM)
		{
			var category = _context.Categories.SingleOrDefault(c => c.CategoryId == categoryVM.CategoryId);
			if (category != null)
			{
				category.CategoryName = categoryVM.CategoryName;
			}
		}
	}
}
