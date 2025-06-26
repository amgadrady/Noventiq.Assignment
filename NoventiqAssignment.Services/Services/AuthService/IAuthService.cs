using NoventiqAssignment.Services.DTOModels;
using NoventiqAssignment.Services.Utility;

namespace NoventiqAssignment.Services
{
    public interface IAuthService
    {
        Task<GenericResponseModel<LoginResponseDTO>> Login(LoginDTO loginDTO);
        Task<GenericResponseModel<NewRoleResponseDTO>> CreateRole(NewRoleDTO newRoleDTO);
        Task<GenericResponseModel<StatusMessageReturnDTO>> AssignUserToRole(AssignRoleDTO assignRoleDto);
        Task<GenericResponseModel<StatusMessageReturnDTO>> SignUp(SignUpDTO signUpDTO);
    }
}
