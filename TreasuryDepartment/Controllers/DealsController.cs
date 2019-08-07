using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TreasuryDepartment.Models;
using TreasuryDepartment.Models.Enums;
using TreasuryDepartment.Models.RequestModels;
using TreasuryDepartment.Services;
using TreasuryDepartment.Services.OfferService;

namespace TreasuryDepartment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealsController : Controller
    {
        private readonly UserService _userService;
        private readonly OfferCrudService<Deal> _dealsCrudService;
        private readonly OfferCrudService<FriendInvite> _friendsCrudService;

        public DealsController(UserService userService, OfferCrudService<Deal> dealsCrudService,
            OfferCrudService<FriendInvite> friendsCrudService,
            FriendService friendService)
        {
            _userService = userService;
            _dealsCrudService = dealsCrudService;
            _friendsCrudService = friendsCrudService;
        }

        [HttpGet]
        public async Task<ActionResult<Deal>> Get([FromQuery] RequestUsersOffer offer) =>
            await _dealsCrudService.Get(offer);

        [HttpGet("[action]")]
        public async Task<ActionResult<List<Deal>>> GetSent(long id) =>
            await GetDealsByType(id, _dealsCrudService.GetSentOffers);

        [HttpGet("[action]")]
        public async Task<ActionResult<List<Deal>>> GetReceived(long id) =>
            await GetDealsByType(id, _dealsCrudService.GetReceivedOffers);


        private delegate Task<List<Deal>> GetDealsDelegate(long id);


        private async Task<ActionResult<List<Deal>>> GetDealsByType(long id, GetDealsDelegate getInvitesDelegate)
        {
            var user = await _userService.Get(id);
            if (user == null)
                return NotFound();

            var deals = await getInvitesDelegate(user.Id);
            return new OkObjectResult(deals.Select(i => new
            {
                user = getInvitesDelegate == _dealsCrudService.GetReceivedOffers ? i.SenderUser : i.TargetUser,
                status = i.Status
            }));
        }


        [HttpPost]
        public async Task<ActionResult<Deal>> Post([FromQuery] RequestUsersOffer offer, [FromBody] decimal sum)
        {
            var fromUser = await _userService.Get(offer.SenderUserId);
            if (fromUser == null)
                return NotFound();
            var toUser = await _userService.Get(offer.TargetUserId);
            if (toUser == null)
                return NotFound();

            var friends = new FriendInvite(offer.SenderUserId, offer.TargetUserId);
            var alreadyFriends = await _friendsCrudService.Get(friends);
            if (alreadyFriends == null)
                return Forbid();

            var deal = new Deal(new Offer(offer), sum);

            await _dealsCrudService.Create(deal);
            return CreatedAtAction(nameof(UsersController.Get), new {offer = offer}, deal
            );
        }

        private delegate Task ChangeStatusDelegate<in T>(T invite);

        private async Task<ActionResult> Change(RequestUsersOffer usersOffer,
            ChangeStatusDelegate<Deal> changeStatusDelegate)
        {
            var fromUser = await _userService.Get(usersOffer.TargetUserId);
            if (fromUser == null)
                return NotFound();
            var toUser = await _userService.Get(usersOffer.TargetUserId);
            if (toUser == null)
                return NotFound();
            var friends = new FriendInvite(usersOffer.SenderUserId, usersOffer.TargetUserId);
            var alreadyFriends = await _friendsCrudService.Get(friends);
            if (alreadyFriends == null)
                return Forbid();

            var deal = await _dealsCrudService.Get(new Offer(usersOffer));

            if (deal.Status != Status.Pending)
                return BadRequest();

            await changeStatusDelegate(deal);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Accept([FromQuery] RequestUsersOffer offer) =>
            await Change(offer, _dealsCrudService.Accept);

        [HttpPost("[action]")]
        public async Task<ActionResult> Decline([FromQuery] RequestUsersOffer offer) =>
            await Change(offer, _dealsCrudService.Decline);

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] RequestUsersOffer offer) =>
            await Change(offer, _dealsCrudService.Delete);
    }
}