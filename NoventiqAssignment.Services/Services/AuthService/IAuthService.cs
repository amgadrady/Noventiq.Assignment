using NoventiqAssignment.Services.DTOModels;
using NoventiqAssignment.Services.Utility;

namespace NoventiqAssignment.Services
{
    public interface IAuthService
    {
        Task<GenericResponseModel<LoginResponseDTO>> Login(LoginDTO loginDTO);
    }
}
