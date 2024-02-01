using WebAPi_App.Data;
using WebAPi_App.Models;

namespace WebAPi_App.Services
{
	public class CategoryRepositoryInMemory : ICategoryRepository
	{
		static List<CategoryVM> categories = new List<CategoryVM>
		{
			new CategoryVM{CategoryId=1, CategoryName="Figure"},
			new CategoryVM{CategoryId=2,CategoryName="CSM"},
			new CategoryVM{CategoryId=3,CategoryName="DX robo"},
			new CategoryVM{CategoryId=4,CategoryName="Driver"}
		};
		public CategoryVM Add(CategoryModel categoryModel)
		{
			var _data = new CategoryVM
			{
				CategoryId = categories.Max(c => c.CategoryId) + 1,
				CategoryName = categoryModel.CategoryName,
			};
			categories.Add(_data);
			return _data;
		}

		public void Delete(int id)
		{
			var _category = categories.SingleOrDefault(c => c.CategoryId == id);
			categories.Remove(_category);
		}

		public List<CategoryVM> GetAll()
		{
			return categories;
		}

		public CategoryVM getById(int id)
		{
			return categories.SingleOrDefault(c => c.CategoryId == id);
		}

		public void Update(CategoryVM categoryVM)
		{
			var _category = categories.SingleOrDefault(c => c.CategoryId == categoryVM.CategoryId);
			if(_category != null)
			{
				_category.CategoryName = categoryVM.CategoryName;
			}
		}
	}
}
