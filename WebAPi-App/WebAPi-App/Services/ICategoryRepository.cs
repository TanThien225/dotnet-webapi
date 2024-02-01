using WebAPi_App.Models;

namespace WebAPi_App.Services
{
	public interface ICategoryRepository
	{
		List<CategoryVM> GetAll();
		CategoryVM getById(int id);
		CategoryVM Add(CategoryModel categoryModel);
		void Update(CategoryVM categoryVM);
		void Delete(int id);
	}
}
