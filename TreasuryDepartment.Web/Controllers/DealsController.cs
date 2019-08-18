using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TreasureDepartment.Data.Dbo;
using TreasureDepartment.Data.Enums;
using TreasureDepartment.Logic.Deals.Services;
using TreasureDepartment.Logic.Friends.Services;
using TreasureDepartment.Logic.OfferCrud.Models;
using TreasureDepartment.Logic.Users.Services;
using TreasureDepartment.Web.ResponseModels;

namespace TreasureDepartment.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealsController : Controller
    {
        private readonly UserService _userService;
        private readonly DealsService _dealsService;
        private readonly FriendService _friendsService;

        public DealsController(UserService userService, DealsService dealsService,
            FriendService friendsService
        )

        {
            _userService = userService;
            _friendsService = friendsService;
            _dealsService = dealsService;
        }

        private async Task<ActionResult> Validate(UsersOfferRequest usersOfferRequest)
        {
            var fromUser = await _userService.Get(usersOfferRequest.SenderUserId);
            if (fromUser == null)
                return NotFound();
            var toUser = await _userService.Get(usersOfferRequest.TargetUserId);
            if (toUser == null)
                return NotFound();

            var friends = new FriendInviteDbo(usersOfferRequest.SenderUserId, usersOfferRequest.TargetUserId);
            var alreadyFriends = await _friendsService.Get(new UsersOfferRequest
            {
                SenderUserId = friends.SenderUserId,
                TargetUserId = friends.TargetUserId
            });
            if (alreadyFriends == null || alreadyFriends.Status != Status.Accepted)
                return BadRequest();

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<DealDbo>> Get([FromQuery] UsersOfferRequest usersOfferRequest)
        {
            var deal = await _dealsService.Get(usersOfferRequest);
            if (deal == null)
                return NotFound();

            return deal;
        }


        [HttpGet("[action]")]
        public async Task<ActionResult<List<DealResponse>>> GetSent(long id) =>
            await GetDealsByType(id, _dealsService.GetSentOffers);

        [HttpGet("[action]")]
        public async Task<ActionResult<List<DealResponse>>> GetReceived(long id) =>
            await GetDealsByType(id, _dealsService.GetReceivedOffers);


        private delegate Task<List<DealDbo>> GetDealsDelegate(long id);


        private async Task<ActionResult<List<DealResponse>>> GetDealsByType(long id,
            GetDealsDelegate getInvitesDelegate)
        {
            var user = await _userService.Get(id);
            if (user == null)
                return NotFound();

            var deals = await getInvitesDelegate(user.Id);
            return deals.Select(d =>
                new DealResponse(d,
                    getInvitesDelegate == _dealsService.GetReceivedOffers
                        ? ChooseFriendUserChoice.Sender
                        : ChooseFriendUserChoice.Target)).ToList();
        }


        [HttpPost]
        public async Task<ActionResult<DealDbo>> Post([FromQuery] UsersOfferRequest usersOfferRequest,
            [FromBody] decimal sum)
        {
            var validateResult = await Validate(usersOfferRequest);
            if (!(validateResult is OkResult))
                return validateResult;

            if (sum < 0)
                return BadRequest();

            var deal = new DealDbo
            {
                SenderUserId = usersOfferRequest.SenderUserId,
                TargetUserId = usersOfferRequest.TargetUserId,
                Sum = sum
            };

            await _dealsService.Create(deal);
            return CreatedAtAction(nameof(UsersController.Get), new {offer = usersOfferRequest}, deal
            );
        }

        private delegate Task ChangeStatusDelegate<in T>(T invite);

        private async Task<ActionResult> Change(UsersOfferRequest usersOfferRequest,
            ChangeStatusDelegate<DealDbo> changeStatusDelegate)
        {
            var validateResult = await Validate(usersOfferRequest);
            if (!(validateResult is OkResult))
                return validateResult;

            var deal = await _dealsService.Get(usersOfferRequest);

            if (deal == null || deal.Status != Status.Pending)
                return BadRequest();

            deal.LastStatusChangeDate = DateTime.Now;
            await changeStatusDelegate(deal);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Accept([FromQuery] UsersOfferRequest usersOfferRequest) =>
            await Change(usersOfferRequest, _dealsService.Accept);

        [HttpPost("[action]")]
        public async Task<ActionResult> Decline([FromQuery] UsersOfferRequest usersOfferRequest) =>
            await Change(usersOfferRequest, _dealsService.Decline);

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] UsersOfferRequest usersOfferRequest) =>
            await Change(usersOfferRequest, _dealsService.Delete);
    }
}