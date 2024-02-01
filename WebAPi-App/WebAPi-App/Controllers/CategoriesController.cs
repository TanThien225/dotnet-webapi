using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPi_App.Data;
using WebAPi_App.Models;

namespace WebAPi_App.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly MyDBContext _context;
		public CategoriesController(MyDBContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult GetAllCategories()
		{
			try {
				var CategoriesList = _context.Categories.ToList();
				return Ok(CategoriesList);
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpGet("{id}")]
		public IActionResult GetCategoryById(int id)
		{
			var category = _context.Categories.SingleOrDefault(cate => cate.CategoryId == id);
			if (category is null)
			{
				return NotFound();
			}
			return Ok(category);
		}

		[HttpPost]
		[Authorize]
		public IActionResult CreateNewCategory(CategoryModel model)
		{
			try
			{
				var category = new Category
				{
					CategoryName = model.CategoryName,
				};
				_context.Add(category);
				_context.SaveChanges();
				return StatusCode(StatusCodes.Status201Created, category);
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPut("{id}")]
		public IActionResult UpdateCategory(int id, CategoryModel model)
		{
			var category = _context.Categories.SingleOrDefault(cate => cate.CategoryId == id);
			if (category is null)
			{
				return NotFound();
			}
			category.CategoryName = model.CategoryName;
			_context.SaveChanges();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteCategoryById(int id)
		{
			var category = _context.Categories.SingleOrDefault(cate => cate.CategoryId == id);
			if (category is null)
			{
				return NotFound();
			}
			_context.Remove(category);
			_context.SaveChanges();
			return StatusCode(StatusCodes.Status200OK);
		}

	}
}
