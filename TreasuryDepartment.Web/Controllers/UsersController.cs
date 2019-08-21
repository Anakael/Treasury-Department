using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TreasureDepartment.Data.Dbo;
using TreasureDepartment.Logic.CryptographyProcessor.Services;
using TreasureDepartment.Logic.Friends.Services;
using TreasureDepartment.Logic.Tokens.Services;
using TreasureDepartment.Logic.Users.Models;
using TreasureDepartment.Logic.Users.Services;
using TreasureDepartment.Logic.Utils;
using TreasureDepartment.Web.ResponseModels;
using TreasureDepartment.Web.Users.Models;

namespace TreasureDepartment.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly FriendService _friendService;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;

        public UsersController(UserService service, FriendService friendService, IMapper mapper,
            TokenService tokenService)
        {
            _userService = service;
            _friendService = friendService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get(long id)
        {
            var user = await _userService.Get(id);
            if (user == null)
                NotFound();

            (ICollection<FriendInviteDbo> outcomeBalances, ICollection<FriendInviteDbo> incomeBalances) =
                await TaskWrapper
                    .WhenAll(_friendService.GetSentOffers(id),
                        _friendService.GetReceivedOffers(id));

            return new OkObjectResult(new
            {
                User = user,
                IncomeBalances =
                    incomeBalances.Select(b => new FriendBalanceResponse(b, ChooseFriendUserChoice.Sender)),
                OutcomeBalance =
                    outcomeBalances.Select(b => new FriendBalanceResponse(b, ChooseFriendUserChoice.Target))
            });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UserAuthorizedResponse>> Authorize([FromForm] string login,
            [FromForm] string password)
        {
            var user = (await _userService.Find(x => x.Login == login)).SingleOrDefault();
            if (user == null)
                return BadRequest();

            if (!CryptographyProcessor.Validate(password, user.HashedPassword, user.Salt))
                return StatusCode((int) HttpStatusCode.Unauthorized);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var accessToken = _tokenService.GenerateToken(userClaims);
            await _tokenService.Save(new TokenDbo
            {
                UserId = user.Id,
                Token = accessToken.Token,
                RefreshToken = accessToken.RefreshToken
            });
            return new UserAuthorizedResponse
            {
                User = _mapper.Map<User>(user),
                Token = accessToken
            };
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UserAuthorizedResponse>> Register(
            [FromBody] UserRegisterRequest userRegisterRequest)
        {
            if ((await _userService.Find(x => x.Login == userRegisterRequest.Login)).Any())
                return BadRequest();

            await _userService.Register(userRegisterRequest);
            return await Authorize(userRegisterRequest.Login, userRegisterRequest.Password);
        }
    }
}