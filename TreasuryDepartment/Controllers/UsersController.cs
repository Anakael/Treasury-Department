using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TreasuryDepartment.Models;
using TreasuryDepartment.Models.ResponseModels;
using TreasuryDepartment.Services;
using TreasuryDepartment.Services.OfferService;
using TreasuryDepartment.Services.Utils;

namespace TreasuryDepartment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly FriendService _friendService;

        public UsersController(UserService service, FriendService friendService)
        {
            _userService = service;
            _friendService = friendService;
        }

        [HttpGet]
        public async Task<ActionResult<User>> Get(long id)
        {
            var user = await _userService.Get(id);
            if (user == null)
                NotFound();

            (ICollection<FriendInvite> outcomeBalances, ICollection<FriendInvite> incomeBalances) = await TaskWrapper
                .WhenAll(_friendService.GetSentOffers(id),
                    _friendService.GetReceivedOffers(id));

            return new OkObjectResult(new
            {
                User = user,
                IncomeBalances = incomeBalances.Select(b => new FriendBalance(b, ChooseFriendUserChoice.Sender)),
                OutcomeBalance = outcomeBalances.Select(b => new FriendBalance(b, ChooseFriendUserChoice.Target))
            });
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            return CreatedAtAction(nameof(Get), new {Id = user.Id}, await _userService.Create(user));
        }
    }
}