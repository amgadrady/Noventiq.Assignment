using Microsoft.AspNetCore.Identity;
using NoventiqAssignment.DB.Models;
using NoventiqAssignment.Services.DTOModels;
using NoventiqAssignment.Services.Utility;

namespace NoventiqAssignment.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ITokenService tokenService;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenService = tokenService;
        }
        public async Task<GenericResponseModel<LoginResponseDTO>> Login(LoginDTO loginDTO)
        {
            GenericResponseModel<LoginResponseDTO> responseModel = new ();
            var user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var token =  tokenService.CreateAccessToken(user, userRoles);

                responseModel.Data = new LoginResponseDTO
                {
                    userId = user.Id,
                    email = user.Email??string.Empty,
                    token = token,
                    fullName = user.UserName?? string.Empty,
                  
                };  
            }
            else {

                responseModel.Message = "Invalid Login Data";
                responseModel.ErrorList = new List<ErrorListModel>
                {
                    new ErrorListModel
                    {
                        Id = 1,
                        Message = "Invalid Email or Password"
                    }
                };
            }

                return responseModel;
        }
    }
}
