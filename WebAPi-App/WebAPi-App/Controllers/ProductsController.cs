using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WebAPi_App.Data;
using WebAPi_App.Services;

namespace WebAPi_App.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IItemRepository _itemRepository;

		public ProductsController(IItemRepository itemRepository)
		{
			_itemRepository = itemRepository;
		}

		[HttpGet]
		public IActionResult GetAllProduct(string? search, double? from, double? to, string? sortBy, int page = 1)
		{
			try
			{
				var result = _itemRepository.GetAll(search, from, to, sortBy, page); 
				return Ok(result);
			}
			catch
			{
				return BadRequest("We can not get product.");
			}
		}
	}
}
