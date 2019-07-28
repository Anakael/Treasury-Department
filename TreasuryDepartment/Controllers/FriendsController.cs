using System.Collections.Generic;
using TreasuryDepartment.Models;
using System.Threading.Tasks;
using TreasuryDepartment.Services;
using Microsoft.AspNetCore.Mvc;

namespace TreasuryDepartment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : Controller
    {
        private readonly UserService _userService;
        private readonly FriendService _friendService;
        public FriendsController(UserService userService, FriendService friendService)
        {
            _userService = userService;
            _friendService = friendService;
        }

        [HttpGet("for/{id}")]
        public async Task<ActionResult<List<Friend>>> Get(long id)
        {
            var user = await _userService.Get(id);

            if (user == null)
                return NotFound();

            return await _friendService.GetFriends(user.Id);
        }
        
        
    }
}