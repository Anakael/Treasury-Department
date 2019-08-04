using System;
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

        [HttpGet]
        public async Task<ActionResult<User>> Get(long id)
        {
            var user = await _userService.Get(id);
            if (user == null)
                NotFound();

            return user; // TODO: Add balances
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            return CreatedAtAction(nameof(Get), new {Id = user.Id}, await _userService.Create(user));
        }
    }
}