using NoventiqAssignment.DB.Models;

namespace NoventiqAssignment.Services
{
    public interface ITokenService
    {
        public string CreateAccessToken(ApplicationUser user, IList<string> userRoles);
        public string CreateRefreshToken(string userId);
    }
}
