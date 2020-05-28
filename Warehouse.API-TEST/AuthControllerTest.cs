using AutoMapper;
using DatingApp.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.API.Data;
using Warehouse.API.Dtos;
using Warehouse.API.Models;
using Xunit;

namespace Warehouse.API_TEST
{
    public class AuthControllerTest
    {
        private readonly Mock<IConfiguration> configMock;

        public AuthControllerTest()
        {
            configMock = new Mock<IConfiguration>();
        }

        private Mock<UserManager<User>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<User>>();

            return new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private Mock<SignInManager<User>> GetMockSignInManager()
        {
            var _mockUserManager = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object,
                null, null, null, null, null, null, null, null);
            var _contextAccessor = new Mock<IHttpContextAccessor>();
            var _userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            return new Mock<SignInManager<User>>(_mockUserManager.Object,
                           _contextAccessor.Object, _userPrincipalFactory.Object, null, null, null, null);
        }

        [Fact]
        public async Task LoginFindUserUnauthorizedStatus()
        {
            //Arrange
            var userForLogin = new UserAuthDto { Username = "test" };
            var userManager = GetMockUserManager();

            AuthController controller = new AuthController(configMock.Object, userManager.Object,
                GetMockSignInManager().Object);

            userManager.Setup(um => um.FindByNameAsync("test"))
                .Returns(Task.FromResult((User)null));

            //Act
            var action = await controller.Login(userForLogin) as UnauthorizedResult;

            //Assert
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task LoginCheckPasswordUnauthorizedStatus()
        {
            //Arrange
            var userForLogin = new UserAuthDto { Username = "test", Password = "test" };
            var userManager = GetMockUserManager();
            var signInManager = GetMockSignInManager();
            var user = new User { Id = 1, UserName = "test" };

            AuthController controller = new AuthController(configMock.Object, userManager.Object,
                signInManager.Object);

            userManager.Setup(um => um.FindByNameAsync("test"))
                .Returns(Task.FromResult(user)).Verifiable();

            signInManager.Setup(sm => sm.CheckPasswordSignInAsync(user, userForLogin.Password, It.IsAny<bool>()))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed)).Verifiable();

            //Act
            var action = await controller.Login(userForLogin) as UnauthorizedResult;

            //Assert
            userManager.Verify(um => um.FindByNameAsync("test"), Times.Once);
            signInManager.Verify(sm => sm.CheckPasswordSignInAsync(user, userForLogin.Password, It.IsAny<bool>()), Times.Once);
            Assert.Equal(401, action.StatusCode);
        }

        [Fact]
        public async Task LoginOkStatus()
        {
            //Arrange
            var userForLogin = new UserAuthDto { Username = "test", Password = "test" };
            var userRoles = new List<UserRole> { new UserRole { RoleId = 1, UserId = 1 } };
            var user = new User { UserName = "test", Id = 1};
            var userManager = GetMockUserManager();
            var signInManager = GetMockSignInManager();
            var configurationSection = new Mock<IConfigurationSection>();

            AuthController controller = new AuthController(configMock.Object, userManager.Object,
                signInManager.Object);

            userManager.Setup(um => um.FindByNameAsync(user.UserName))
                 .Returns(Task.FromResult(user));

            signInManager.Setup(sm => sm.CheckPasswordSignInAsync(user, userForLogin.Password, false))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));

            configurationSection.Setup(a => a.Value).Returns("VeryLongKeyForTest");
            configMock.Setup(a => a.GetSection("AppSettings:Token")).Returns(configurationSection.Object);



            //Act
            var action = await controller.Login(userForLogin) as OkObjectResult;
            var item = action as Object;

            //Assert
            Assert.Equal(200, action.StatusCode);
            Assert.NotNull(item);
        }
    }
}
