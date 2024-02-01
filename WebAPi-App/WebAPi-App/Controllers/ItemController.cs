using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPi_App.Models;

namespace WebAPi_App.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemController : ControllerBase
	{
		public static List<Item> ListItem = new List<Item>();

		[HttpGet] 
		public IActionResult GetAll() {
			return Ok(ListItem);
		}

		[HttpGet("{id}")]
		public IActionResult GetById(string id)
		{
			try
			{
				//LINQ [object] query
				var item = ListItem.SingleOrDefault(hh => hh.IdItem == Guid.Parse(id));
				if (item == null)
				{
					return NotFound();
				}
				return Ok(item);
			}catch 
			{
				return BadRequest();
			}
		}

		[HttpPost]
		public IActionResult Create(ItemVM itemVM)
		{
			var item = new Item
			{
				IdItem = Guid.NewGuid(),
				Name = itemVM.Name,
				Price = itemVM.Price,
			};
			ListItem.Add(item);
			return Ok(new
			{
				Success = true,
				Data = item
			});
		}

		[HttpPut("{id}")]
		public IActionResult Update(string id, Item itemUpdate)
		{
			try
			{
				var item = ListItem.SingleOrDefault(hh => hh.IdItem == Guid.Parse(id));
				if (item == null)
				{
					return NotFound();
				}
				if (id != itemUpdate.IdItem.ToString())
				{
					return BadRequest();
				}
				item.Name = itemUpdate.Name;
				item.Price = itemUpdate.Price;
				return Ok(item);
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(string id)
		{
			try
			{
				var item = ListItem.SingleOrDefault(hh => hh.IdItem == Guid.Parse(id));
				if (item == null)
				{
					return NotFound();
				}
				
				ListItem.Remove(item);
				
				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}

	}
}
