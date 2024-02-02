using WebAPi_App.Models;

namespace WebAPi_App.Services
{
	public interface IItemRepository
	{
		List<ItemModel> GetAll(string? search, double? from, double? to, string? sortBy, int page = 1);
	}
}
