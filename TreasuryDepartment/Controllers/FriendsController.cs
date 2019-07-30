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
        public async Task<ActionResult<List<User>>> Get(long id)
        {
            var user = await _userService.Get(id);

            if (user == null)
                return NotFound();

            return await _friendService.GetFriends(user.Id);
        }

        [HttpDelete("from/{user1Id}/to/{user2Id}")]
        public async Task<ActionResult> Delete(long user1Id, long user2Id)
        {
            var fromUser = await _userService.Get(user1Id);
            var toUser = await _userService.Get(user2Id);
            var friends = await _friendService.Get(user1Id, user2Id);
            if (fromUser == null || toUser == null || friends == null)
                return NotFound();

            await _friendService.Delete(friends);
            return NoContent();
        }
    }
}