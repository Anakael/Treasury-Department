using TreasuryDepartment.Services;
using System.Threading.Tasks;
using TreasuryDepartment.Models;
using Microsoft.AspNetCore.Mvc;

namespace TreasuryDepartment.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly UserService _userService;
		public UsersController(UserService service)
		{
			_userService = service;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<User>> Get(long id)
		{
			var user = await _userService.Get(id);
			if (user == null)
			{
				return NotFound();
			}
			return user;
		}

		[HttpPost]
		public async Task<ActionResult<User>> Post(User user)
		{
			await _userService.Create(user);
			return CreatedAtAction(nameof(Get), new { Id = user.Id }, user);
		}

		[HttpPut("{id}")]
		public void Put(long id, string value)
		{
		}

		[HttpDelete("{id}")]
		public void Delete(long id)
		{
		}
	}
}
