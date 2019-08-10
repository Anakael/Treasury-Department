using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TreasuryDepartment.Models;
using TreasuryDepartment.Models.Enums;
using TreasuryDepartment.Models.RequestModels;
using TreasuryDepartment.Services;

namespace TreasuryDepartment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : Controller
    {
        private readonly UserService _userService;
        private readonly FriendService _friendService;

        public FriendsController(UserService userService,
            FriendService friendService)
        {
            _userService = userService;
            _friendService = friendService;
        }

        /// <summary>
        /// Retrieve friends list
        /// </summary>
        /// <param name="id">User's ID</param>
        /// <returns>Friends list</returns>
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get(long id)
        {
            var user = await _userService.Get(id);

            if (user == null)
                return NotFound();

            return await _friendService.GetFriends(user.Id);
        }


        private delegate Task<List<FriendInvite>> GetInvitesDelegate(long id);


        private async Task<ActionResult<List<FriendInvite>>> GetByType(long id, GetInvitesDelegate getInvitesDelegate)
        {
            var user = await _userService.Get(id);
            if (user == null)
                return NotFound();

            var invites = await getInvitesDelegate(user.Id);
            return new OkObjectResult(invites.Select(i => new
            {
                user = getInvitesDelegate == _friendService.GetReceivedOffers ? i.SenderUser : i.TargetUser,
                status = i.Status
            }));
        }


        [HttpGet("[action]")]
        public async Task<ActionResult<List<FriendInvite>>> GetSent(long id) =>
            await GetByType(id, _friendService.GetSentOffers);


        [HttpGet("[action]")]
        public async Task<ActionResult<List<FriendInvite>>> GetReceived(long id) =>
            await GetByType(id, _friendService.GetReceivedOffers);

        /// <summary>
        /// Create friend's invite
        /// </summary>
        /// <param name="requestUsersOffer"></param>
        /// <returns>New friend's invite</returns>
        [HttpPost]
        public async Task<ActionResult<FriendInvite>> Post([FromQuery] RequestUsersOffer requestUsersOffer)
        {
            var fromUser = await _userService.Get(requestUsersOffer.SenderUserId);
            if (fromUser == null)
                return NotFound();
            var toUser = await _userService.Get(requestUsersOffer.TargetUserId);
            if (toUser == null)
                return NotFound();

            var friends = new FriendInvite(requestUsersOffer.SenderUserId, requestUsersOffer.TargetUserId);
            var alreadyFriends = await _friendService.Get(friends);
            if (alreadyFriends != null)
                return BadRequest();

            await _friendService.Create(friends);
            var outputUser = friends.TargetUserId == requestUsersOffer.TargetUserId
                ? friends.TargetUser
                : friends.SenderUser;
            return CreatedAtAction(nameof(UsersController.Get), new {id = outputUser.Id}, outputUser
            );
        }

        private delegate Task ChangeStatusDelegate<in T>(T invite);

        private async Task<ActionResult> Change(RequestUsersOffer requestUsersOffer,
            ChangeStatusDelegate<FriendInvite> changeStatusDelegate)
        {
            var invite = await _friendService.Get(requestUsersOffer);
            if (invite == null)
                return NotFound();

            if (invite.Status != Status.Pending
                || (invite.Status == Status.Accepted
                    && changeStatusDelegate == _friendService.Delete
                    && invite.Sum != 0))
            {
                return BadRequest();
            }

            await changeStatusDelegate(invite);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Accept([FromQuery] RequestUsersOffer requestUsersOffer) =>
            await Change(requestUsersOffer, _friendService.Accept);

        [HttpPost("[action]")]
        public async Task<ActionResult> Decline([FromQuery] RequestUsersOffer requestUsersOffer) =>
            await Change(requestUsersOffer, _friendService.Decline);

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] RequestUsersOffer requestUsersOffer) =>
            await Change(requestUsersOffer, _friendService.Delete);
    }
}