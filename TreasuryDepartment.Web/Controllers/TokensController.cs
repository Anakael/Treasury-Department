using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TreasureDepartment.Logic.Tokens.Models;
using TreasureDepartment.Logic.Tokens.Services;

namespace TreasureDepartment.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public TokensController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<AccessToken>> Refresh(string token, string refreshToken) =>
            await _tokenService.RefreshToken(token, refreshToken);
    }
}