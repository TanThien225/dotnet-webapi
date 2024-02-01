using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPi_App.Models;
using WebAPi_App.Services;

namespace WebAPi_App.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryRepository _categoryRepository;
		public CategoryController(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			try
			{
				return Ok(_categoryRepository.GetAll());
			}
			catch
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			try
			{
				var data = _categoryRepository.getById(id);
				if (data == null)
				{
					return NotFound();
				}
				return Ok(data);
			}
			catch
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, CategoryVM categoryVM)
		{
			if (id != categoryVM.CategoryId)
			{
				return BadRequest();
			}
			try
			{
				_categoryRepository.Update(categoryVM);
				return NoContent();
			}
			catch
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			try
			{
				_categoryRepository.Delete(id);
				return Ok();
			}
			catch
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpPost]
		public IActionResult Add(CategoryModel categoryModel)
		{
			try
			{
				var data = _categoryRepository.Add(categoryModel);
				return Ok(data);
			}
			catch
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
