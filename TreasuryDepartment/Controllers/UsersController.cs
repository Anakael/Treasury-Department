using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TreasuryDepartment.Models;
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
        private readonly OfferCrudService<Balance> _balanceService;

        public UsersController(UserService service, OfferCrudService<Balance> balanceService)
        {
            _userService = service;
            _balanceService = balanceService;
        }

        [HttpGet]
        public async Task<ActionResult<User>> Get(long id)
        {
            var user = await _userService.Get(id);
            if (user == null)
                NotFound();

            (ICollection<Balance> outcomeBalances, ICollection<Balance> incomeBalances) = await TaskWrapper
                .WhenAll(_balanceService.GetSentOffers(id),
                    _balanceService.GetReceivedOffers(id));

            return new OkObjectResult(new
            {
                User = user,
                IncomeBalances = incomeBalances,
                OutcomeBalance = outcomeBalances
            });
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            return CreatedAtAction(nameof(Get), new {Id = user.Id}, await _userService.Create(user));
        }
    }
}