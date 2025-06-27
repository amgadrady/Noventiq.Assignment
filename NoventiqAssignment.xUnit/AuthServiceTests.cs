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

        [Fact]

        public async Task Login_WithInvalidCredentials_ReturnsErrorResponse()
        {
            //Arrange
            var loginDto = new LoginDTO { Email = "wrong@example.com", Password = "WrongPass123!" };

            userManagerMock.Setup(x => x.FindByEmailAsync(loginDto.Email))
                      .ReturnsAsync((ApplicationUser)null);
            //Act
            var result = await authService.Login(loginDto);
            //Assert


            Assert.Equal("Invalid Email or Password", result.Message);
        }

        [Fact]
        public async Task CreateRole_WithNewRole_ReturnsSuccessResponse()
        {
            // Arrange
            var newRoleDto = new NewRoleDTO { Name = "Manager", Description = "Manager role" };

            roleManagerMock.Setup(x => x.RoleExistsAsync(newRoleDto.Name))
                .ReturnsAsync(false);
            roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationRole>()))
                .ReturnsAsync(IdentityResult.Success);
            roleManagerMock.Setup(x => x.FindByNameAsync(newRoleDto.Name))
                .ReturnsAsync(new ApplicationRole { Id = "1", Name = newRoleDto.Name, Description = newRoleDto.Description });

            // Act
            var result = await authService.CreateRole(newRoleDto);

            // Assert

            Assert.Equal("Manager", result.Data.Name);
        }
        [Fact]
        public async Task CreateRole_WithExistingRole_ReturnsErrorResponse()
        {
            // Arrange
            var newRoleDto = new NewRoleDTO { Name = "Admin", Description = "Admin role" };

            roleManagerMock.Setup(x => x.RoleExistsAsync(newRoleDto.Name))
                .ReturnsAsync(true);

            // Act
            var result = await authService.CreateRole(newRoleDto);

            // Assert
            Assert.Equal("Role already exists", result.Message);
        }

        [Fact]
        public async Task AssignUserToRole_WithValidData_ReturnsSuccessResponse()
        {
            // Arrange
            var assignRoleDto = new AssignRoleDTO { UserId = "1", RoleName = "Admin" };
            var testUser = new ApplicationUser { Id = "1" };

            roleManagerMock.Setup(x => x.RoleExistsAsync(assignRoleDto.RoleName))
                .ReturnsAsync(true);
            userManagerMock.Setup(x => x.FindByIdAsync(assignRoleDto.UserId))
                .ReturnsAsync(testUser);
            userManagerMock.Setup(x => x.IsInRoleAsync(testUser, assignRoleDto.RoleName))
                .ReturnsAsync(false);
            userManagerMock.Setup(x => x.AddToRoleAsync(testUser, assignRoleDto.RoleName))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await authService.AssignUserToRole(assignRoleDto);

            // Assert
            Assert.True(result.Data.Status);
        }

        [Fact]
        public async Task AssignUserToRole_WithInvalidRole_ReturnsErrorResponse()
        {
            // Arrange
            var assignRoleDto = new AssignRoleDTO { UserId = "1", RoleName = "InvalidRole" };

            roleManagerMock.Setup(x => x.RoleExistsAsync(assignRoleDto.RoleName))
                .ReturnsAsync(false);

            // Act
            var result = await authService.AssignUserToRole(assignRoleDto);

            // Assert
            Assert.False(result.Data.Status);
            Assert.Equal("Invalid Role", result.Message);
        }
    

    [Fact]
        public async Task SignUp_WithNewUser_ReturnsSuccessResponse()
        {
            // Arrange
            var signUpDto = new SignUpDTO
            {
                Email = "new@example.com",
                Password = "Pass123!",
                FirstName = "New",
                LastName = "User"
            };

            userManagerMock.Setup(x => x.FindByEmailAsync(signUpDto.Email))
                .ReturnsAsync((ApplicationUser)null);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), signUpDto.Password))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await authService.SignUp(signUpDto);

            // Assert

            Assert.True(result.Data.Status);
        }

        [Fact]
        public async Task SignUp_WithExistingUser_ReturnsErrorResponse()
        {
            // Arrange
            var signUpDto = new SignUpDTO
            {
                Email = "existing@example.com",
                Password = "Pass123!",
                FirstName = "Existing",
                LastName = "User"
            };

            userManagerMock.Setup(x => x.FindByEmailAsync(signUpDto.Email))
                .ReturnsAsync(new ApplicationUser());

            // Act
            var result = await authService.SignUp(signUpDto);

            // Assert
            Assert.False(result.Data.Status);
            Assert.Equal("User Mail Exist", result.Message);
        }
    }
}
