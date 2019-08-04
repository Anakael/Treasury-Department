using System.Collections.Generic;
using System.Linq;
using TreasuryDepartment.Models;
using TreasuryDepartment.Models.RequestModels;
using System.Threading.Tasks;
using TreasuryDepartment.Services;
using TreasuryDepartment.Services.OfferService;
using Microsoft.AspNetCore.Mvc;
using TreasuryDepartment.Models.Enums;

namespace TreasuryDepartment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : Controller
    {
        private readonly UserService _userService;
        private readonly OfferCrudService<FriendInvite> _friendCrudService;
        private readonly FriendService _friendService;

        public FriendsController(UserService userService, OfferCrudService<FriendInvite> friendCrudService,
            FriendService friendService)
        {
            _userService = userService;
            _friendCrudService = friendCrudService;
            _friendService = friendService;
        }

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
                user = getInvitesDelegate == _friendCrudService.GetReceivedOffers ? i.SenderUser : i.TargetUser,
                status = i.Status
            }));
        }


        [HttpGet("[action]")]
        public async Task<ActionResult<List<FriendInvite>>> GetSent(long id) =>
            await GetByType(id, _friendCrudService.GetSentOffers);


        [HttpGet("[action]")]
        public async Task<ActionResult<List<FriendInvite>>> GetReceived(long id) =>
            await GetByType(id, _friendCrudService.GetReceivedOffers);


        [HttpPost]
        public async Task<ActionResult<FriendInvite>> Post([FromQuery] RequestUsersOffer offer)
        {
            var fromUser = await _userService.Get(offer.SenderUserId);
            if (fromUser == null)
                return NotFound();
            var toUser = await _userService.Get(offer.TargetUserId);
            if (toUser == null)
                return NotFound();

            var friends = new FriendInvite(offer.SenderUserId, offer.TargetUserId);
            var alreadyFriends = await _friendCrudService.Get(friends);
            if (alreadyFriends != null)
                return BadRequest();

            await _friendCrudService.Create(friends);
            var outputUser = friends.TargetUserId == offer.TargetUserId ? friends.TargetUser : friends.SenderUser;
            return CreatedAtAction(nameof(UsersController.Get), new {id = outputUser.Id}, outputUser
            );
        }

        private delegate Task ChangeStatusDelegate<in T>(T invite);

        private async Task<ActionResult> Change(RequestUsersOffer usersOffer,
            ChangeStatusDelegate<FriendInvite> changeStatusDelegate)
        {
            var invite = await _friendCrudService.Get(usersOffer);
            if (invite == null)
                return NotFound();

            if (invite.Status != Status.Pending)
                return BadRequest();

            await changeStatusDelegate(invite);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Accept([FromQuery] RequestUsersOffer offer) =>
            await Change(offer, _friendCrudService.Accept);

        [HttpPost("[action]")]
        public async Task<ActionResult> Decline([FromQuery] RequestUsersOffer offer) =>
            await Change(offer, _friendCrudService.Decline);

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] RequestUsersOffer offer) =>
            await Change(offer, _friendCrudService.Delete);
    }
}