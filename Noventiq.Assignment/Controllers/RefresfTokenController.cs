using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoventiqAssignment.Services;
using NoventiqAssignment.Services.DTOModels;

namespace NoventiqAssignment.API.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "RefresfToken")]
    public class RefresfTokenController : ControllerBase
    {

        private readonly IAuthService authService;
        public RefresfTokenController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] LoginDTO loginDTO)
        {
            var result = await authService.RefreshTokenLogin(loginDTO);

            if (result.ErrorList.Any())
                return BadRequest(result);

            return Ok(result);
        }
    }
}
