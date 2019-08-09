using System;
using System.Collections.Generic;
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
        private readonly DealsService _dealsService;
        private readonly OfferCrudWithUpdateService<FriendInvite> _friendsCrudService;

        public DealsController(UserService userService, DealsService dealsService,
            OfferCrudWithUpdateService<FriendInvite> friendsCrudService
        )

        {
            _userService = userService;
            _friendsCrudService = friendsCrudService;
            _dealsService = dealsService;
        }

        private async Task<ActionResult> Validate(RequestUsersOffer requestUsersOffer)
        {
            var fromUser = await _userService.Get(requestUsersOffer.SenderUserId);
            if (fromUser == null)
                return NotFound();
            var toUser = await _userService.Get(requestUsersOffer.TargetUserId);
            if (toUser == null)
                return NotFound();

            var friends = new FriendInvite(requestUsersOffer.SenderUserId, requestUsersOffer.TargetUserId);
            var alreadyFriends = await _friendsCrudService.Get(friends);
            if (alreadyFriends == null)
                return BadRequest();

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<Deal>> Get([FromQuery] RequestUsersOffer requestUsersOffer)
        {
            var deal = await _dealsService.Get(requestUsersOffer);
            if (deal == null)
                return NotFound();

            return deal;
        }


        [HttpGet("[action]")]
        public async Task<ActionResult<List<Deal>>> GetSent(long id) =>
            await GetDealsByType(id, _dealsService.GetSentOffers);

        [HttpGet("[action]")]
        public async Task<ActionResult<List<Deal>>> GetReceived(long id) =>
            await GetDealsByType(id, _dealsService.GetReceivedOffers);


        private delegate Task<List<Deal>> GetDealsDelegate(long id);


        private async Task<ActionResult<List<Deal>>> GetDealsByType(long id, GetDealsDelegate getInvitesDelegate)
        {
            var user = await _userService.Get(id);
            if (user == null)
                return NotFound();

            var deals = await getInvitesDelegate(user.Id);
            return deals;
        }


        [HttpPost]
        public async Task<ActionResult<Deal>> Post([FromQuery] RequestUsersOffer requestUsersOffer,
            [FromBody] decimal sum)
        {
            var validateResult = await Validate(requestUsersOffer);
            if (!(validateResult is OkResult))
                return validateResult;

            if (sum < 0)
                return BadRequest();

            var deal = new Deal(new Offer(requestUsersOffer), sum);

            await _dealsService.Create(deal);
            return CreatedAtAction(nameof(UsersController.Get), new {offer = requestUsersOffer}, deal
            );
        }

        private delegate Task ChangeStatusDelegate<in T>(T invite);

        private async Task<ActionResult> Change(RequestUsersOffer requestUsersOffer,
            ChangeStatusDelegate<Deal> changeStatusDelegate)
        {
            var validateResult = await Validate(requestUsersOffer);
            if (!(validateResult is OkResult))
                return validateResult;

            var deal = await _dealsService.Get(requestUsersOffer);

            if (deal.Status != Status.Pending)
                return BadRequest();

            deal.LastStatusChangeDate = DateTime.Now;
            await changeStatusDelegate(deal);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Accept([FromQuery] RequestUsersOffer requestUsersOffer) =>
            await Change(requestUsersOffer, _dealsService.Accept);

        [HttpPost("[action]")]
        public async Task<ActionResult> Decline([FromQuery] RequestUsersOffer requestUsersOffer) =>
            await Change(requestUsersOffer, _dealsService.Decline);

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] RequestUsersOffer requestUsersOffer) =>
            await Change(requestUsersOffer, _dealsService.Delete);
    }
}