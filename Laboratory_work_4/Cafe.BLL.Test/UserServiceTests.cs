using Xunit;
using NSubstitute;
using Cafe.BLL.Services;
using Cafe.DAL.Repositories;
using Cafe.DAL.Entities;
using System.Threading.Tasks;

namespace Cafe.BLL.Test
{
    public class UserServiceTests
    {
        private readonly IUserRepository _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _userService = new UserService(_userRepositoryMock);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFalse_WhenUserAlreadyExists()
        {
            var username = "testuser";
            _userRepositoryMock.GetUserByUsernameAsync(username).Returns(new User { Username = username });

            var result = await _userService.RegisterAsync(username, "password");

            Assert.False(result);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnTrue_WhenUserIsNew()
        {
            var username = "newuser";
            _userRepositoryMock.GetUserByUsernameAsync(username).Returns((User)null);

            var result = await _userService.RegisterAsync(username, "password");

            Assert.True(result);
        }
    }
}