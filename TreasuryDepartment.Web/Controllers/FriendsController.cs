using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TreasureDepartment.Data.Dbo;
using TreasureDepartment.Data.Enums;
using TreasureDepartment.Logic.Friends.Services;
using TreasureDepartment.Logic.OfferCrud.Models;
using TreasureDepartment.Logic.Users.Services;

namespace TreasureDepartment.Web.Controllers
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
        /// <param name="id">UserDbo's ID</param>
        /// <returns>Friends list</returns>
        [HttpGet]
        public async Task<ActionResult<List<UserDbo>>> Get(long id)
        {
            var user = await _userService.Get(id);

            if (user == null)
                return NotFound();

            return await _friendService.GetFriends(user.Id);
        }


        private delegate Task<List<FriendInviteDbo>> GetInvitesDelegate(long id);


        private async Task<ActionResult<List<FriendInviteDbo>>> GetByType(long id,
            GetInvitesDelegate getInvitesDelegate)
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
        public async Task<ActionResult<List<FriendInviteDbo>>> GetSent(long id) =>
            await GetByType(id, _friendService.GetSentOffers);


        [HttpGet("[action]")]
        public async Task<ActionResult<List<FriendInviteDbo>>> GetReceived(long id) =>
            await GetByType(id, _friendService.GetReceivedOffers);

        /// <summary>
        /// Register friend's invite
        /// </summary>
        /// <param name="usersOfferRequest"></param>
        /// <returns>New friend's invite</returns>
        [HttpPost]
        public async Task<ActionResult<FriendInviteDbo>> Post([FromQuery] UsersOfferRequest usersOfferRequest)
        {
            var fromUser = await _userService.Get(usersOfferRequest.SenderUserId);
            if (fromUser == null)
                return NotFound();
            var toUser = await _userService.Get(usersOfferRequest.TargetUserId);
            if (toUser == null)
                return NotFound();

            var friends = new FriendInviteDbo(usersOfferRequest.SenderUserId, usersOfferRequest.TargetUserId);
            var alreadyFriends = await _friendService.Get(new UsersOfferRequest
            {
                SenderUserId = friends.SenderUserId,
                TargetUserId = friends.TargetUserId
            });
            if (alreadyFriends != null)
                return BadRequest();

            await _friendService.Create(friends);
            var outputUser = friends.TargetUserId == usersOfferRequest.TargetUserId
                ? friends.TargetUser
                : friends.SenderUser;
            return CreatedAtAction(nameof(UsersController.Get), new {id = outputUser.Id}, outputUser
            );
        }

        private delegate Task ChangeStatusDelegate<in T>(T invite);

        private async Task<ActionResult> Change(UsersOfferRequest usersOfferRequest,
            ChangeStatusDelegate<FriendInviteDbo> changeStatusDelegate)
        {
            var invite = await _friendService.Get(usersOfferRequest);
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
        public async Task<ActionResult> Accept([FromQuery] UsersOfferRequest usersOfferRequest) =>
            await Change(usersOfferRequest, _friendService.Accept);

        [HttpPost("[action]")]
        public async Task<ActionResult> Decline([FromQuery] UsersOfferRequest usersOfferRequest) =>
            await Change(usersOfferRequest, _friendService.Decline);

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] UsersOfferRequest usersOfferRequest) =>
            await Change(usersOfferRequest, _friendService.Delete);
    }
}