using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoventiqAssignment.Services;
using NoventiqAssignment.Services.DTOModels;

namespace NoventiqAssignment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService authService;
        public UserController(IAuthService authService)
        {
            this.authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO  signUpDTO)
        {
            var result = await authService.SignUp(signUpDTO);

            if (result.ErrorList.Any())
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await authService.Login(loginDTO);

            if (result.ErrorList.Any())
                return BadRequest(result);

            return Ok(result);
        }
    }
}
