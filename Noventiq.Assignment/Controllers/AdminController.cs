using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using NoventiqAssignment.Services;
using NoventiqAssignment.Services.DTOModels;

namespace NoventiqAssignment.API.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : ControllerBase
    {
        private readonly IAuthService authService;
        public AdminController(IAuthService authService)
        {
            this.authService = authService;
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

        
        
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] NewRoleDTO newRoleDTO)
        {
            var result = await authService.CreateRole(newRoleDTO);

            if (result.ErrorList.Any())
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AssignUserToRole([FromBody] AssignRoleDTO assignRoleDto)
        {
            var result = await authService.AssignUserToRole(assignRoleDto);

            if (result.ErrorList.Any())
                return BadRequest(result);

            return Ok(result);
        }

    }
}
