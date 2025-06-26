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
            var responseModel = new GenericResponseModel<LoginResponseDTO>();

            var user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                return GenericResponseModel<LoginResponseDTO>.ErrorResponse("Invalid Email or Password");
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var token = tokenService.CreateAccessToken(user, userRoles);

            responseModel.Data = new LoginResponseDTO
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                Token = token,
                FullName = user.UserName ?? string.Empty,
            };

            return responseModel;
        }



        public async Task<GenericResponseModel<NewRoleResponseDTO>> CreateRole(NewRoleDTO newRoleDTO)
        {
            var responseModel = new GenericResponseModel<NewRoleResponseDTO>();


            if (await roleManager.RoleExistsAsync(newRoleDTO.Name))
            {
                return GenericResponseModel<NewRoleResponseDTO>.ErrorResponse("Role already exists");
            }


            var createResult = await roleManager.CreateAsync(new ApplicationRole
            {
                Name = newRoleDTO.Name,
                Description = newRoleDTO.Description
            });

            if (!createResult.Succeeded)
            {
                return GenericResponseModel<NewRoleResponseDTO>.ErrorResponse("Failed to create the role");
            }


            var createdRole = await roleManager.FindByNameAsync(newRoleDTO.Name);

            responseModel.Data = new NewRoleResponseDTO
            {
                Id = createdRole.Id,
                Name = createdRole.Name,
                Description = createdRole.Description
            };

            return responseModel;
        }

        public async Task<GenericResponseModel<StatusMessageReturnDTO>> AssignUserToRole(AssignRoleDTO assignRoleDto)
        {
            var responseModel = new GenericResponseModel<StatusMessageReturnDTO>();

            if (!await roleManager.RoleExistsAsync(assignRoleDto.RoleName))
            {
                return GenericResponseModel<StatusMessageReturnDTO>.ErrorResponseForStatus("Invalid Role");

            }

            var user = await userManager.FindByIdAsync(assignRoleDto.UserId);
            if (user == null)
            {
                return GenericResponseModel<StatusMessageReturnDTO>.ErrorResponseForStatus("Invalid User");

            }

            if (await userManager.IsInRoleAsync(user, assignRoleDto.RoleName))
            {
                return GenericResponseModel<StatusMessageReturnDTO>.ErrorResponseForStatus("User already has the role");
            }

            var result = await userManager.AddToRoleAsync(user, assignRoleDto.RoleName);

            if (result.Succeeded)
            {
                responseModel.Data.Status = true;
            }
            else
            {
                return GenericResponseModel<StatusMessageReturnDTO>.ErrorResponseForStatus("Failed to assign user to role");
            }

            return responseModel;
        }

        public async Task<GenericResponseModel<StatusMessageReturnDTO>> SignUp(SignUpDTO signUpDTO)
        {
            var responseModel = new GenericResponseModel<StatusMessageReturnDTO>();

            var user = await userManager.FindByEmailAsync(signUpDTO.Email);
            if (user != null)
            {
                return GenericResponseModel<StatusMessageReturnDTO>.ErrorResponseForStatus("User Mail Exist");

            }

            else
            {
                user = new ApplicationUser
                {
                    UserName = signUpDTO.Email,
                    Email = signUpDTO.Email,
                    FirstName = signUpDTO.FirstName,
                    LastName = signUpDTO.LastName,
                    EmailConfirmed = true,
                    CreatedDate = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, signUpDTO.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                    responseModel.Data.Status = true;

                }
                else
                {
                    return GenericResponseModel<StatusMessageReturnDTO>.ErrorResponseForStatus("Error in User Creating");
                }

            }



                return responseModel;
        }
    }
}
