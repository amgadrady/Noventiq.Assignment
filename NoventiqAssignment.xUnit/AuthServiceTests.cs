using Microsoft.AspNetCore.Identity;
using Moq;
using NoventiqAssignment.DB.Models;
using NoventiqAssignment.Services;
using NoventiqAssignment.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoventiqAssignment.xUnit
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManagerMock;
        private readonly Mock<RoleManager<ApplicationRole>> roleManagerMock;
        private readonly Mock<ITokenService> tokenServiceMock;
        private readonly AuthService authService;

        public AuthServiceTests()
        {
            userManagerMock = new Mock<UserManager<ApplicationUser>>(
                 Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            roleManagerMock = new Mock<RoleManager<ApplicationRole>>(
                Mock.Of<IRoleStore<ApplicationRole>>(), null, null, null, null);

            tokenServiceMock = new Mock<ITokenService>();

            authService = new AuthService(
                userManagerMock.Object,
                roleManagerMock.Object,
                tokenServiceMock.Object);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsSuccessResponse()
        {
            // Arrange
            var testUser = new ApplicationUser { Email = "test@example.com" };
            var loginDto = new LoginDTO { Email = "test@example.com", Password = "ValidPass123!" };

            userManagerMock.Setup(x => x.FindByEmailAsync(loginDto.Email))
                .ReturnsAsync(testUser);
            userManagerMock.Setup(x => x.CheckPasswordAsync(testUser, loginDto.Password))
                .ReturnsAsync(true);
            userManagerMock.Setup(x => x.GetRolesAsync(testUser))
                .ReturnsAsync(new List<string> { "User" });
            tokenServiceMock.Setup(x => x.CreateAccessToken(testUser, It.IsAny<List<string>>()))
                .Returns("valid_token");

            // Act
            var result = await authService.Login(loginDto);

            // Assert
            Assert.Equal("valid_token", result.Data.Token);
            Assert.Equal(testUser.Email, result.Data.Email);
        }

    }
}
