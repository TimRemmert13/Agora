using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Respositories;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace API.Tests
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly IUserRepository _userRepository;
        private readonly ISetupTests _setup;

        public UserRepositoryTests(ITestOutputHelper output) 
        {
            _output = output;
            _setup = new SetUpTests();
            _userRepository = new UserRepository(_setup.GetTestDbContext(this.GetType().Name));
        }
        
        public static IEnumerable<object[]> GetUserPositive()
        {
            var newUser = new AppUser
            {
                Email = "newuser@exmaple.com",
                EmailVerified = false,
                Username = "Jim",
                GivenName = "Jim",
                FamilyName = "Jimbo",
                Name = "Jim Jimbo",
                Nickname = "Jim"
            };
            yield return new object[] {newUser};
        }
        
        public static IEnumerable<object[]> GetUserNegative()
        {
            var newUser = new AppUser
            {
                EmailVerified = false,
                Username = "Jim",
                GivenName = "Jim",
                FamilyName = "Jimbo",
                Name = "Jim Jimbo",
                Nickname = "Jim"
            };
            yield return new object[] {newUser};
        }

        [Theory]
        [MemberData(nameof(GetUserPositive))]
        public async Task CreateUserAsync_ShouldCreateNewUser(AppUser user)
        {
            var result = await _userRepository.CreateUserAsync(user);
            var newUser = await _setup.GetContext().Users.Where(u => u.Id == user.Id).SingleOrDefaultAsync();
            Assert.True(result);
            Assert.Equal(user, newUser);
        }
        
        [Theory]
        [MemberData(nameof(GetUserNegative))]
        public async Task CreateUserAsync_ShouldRejectNewUser(AppUser user)
        {
            await Assert.ThrowsAsync<DbUpdateException>(() => _userRepository.CreateUserAsync(user));
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnUserList()
        {
            var userList = await _userRepository.GetUsersAsync();
            Assert.NotEmpty(userList);
        }
        
        [Theory]
        [InlineData("tremmert93@gmail.com")]
        public async Task GetUserAsync_ShouldReturnUser(string email)
        {
            var user = await _userRepository.GetUserAsync(email);
            Assert.Equal(email, user.Email);
        }

        [Theory]
        [InlineData("tremmert93@gmail.com")]
        public async Task DeleteUser_ShouldDeleteUser(string email)
        {
            _userRepository.DeleteUser(email);
            var result = await _userRepository.SaveAllAsync();
            var deletedUser = await _setup.GetContext().Users.Where(u => u.Email == email).SingleOrDefaultAsync();
            Assert.True(result);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task UpdateUser_ShouldMakeUserUpdate()
        {
            var users = await _userRepository.GetUsersAsync();
            var user = users.FirstOrDefault();
            user.Name = "Jimbo";
            _userRepository.UpdateUser(user);
            var result = await _userRepository.SaveAllAsync();
            var updated = await _setup.GetContext().Users.Where(u => u.Email == user.Email).SingleOrDefaultAsync();
            Assert.True(result);
            Assert.Equal(user.Name, updated.Name);
        }

        public void Dispose()
        {
            _setup.Dispose();
        }
    }
}