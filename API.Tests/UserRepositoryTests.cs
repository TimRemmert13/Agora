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
    public class UserRepositoryTests : SetUpTests
    {
        private readonly ITestOutputHelper _output;
        private readonly IUserRepository _userRepository;

        public UserRepositoryTests(ITestOutputHelper output) : base(output)
        {
            _output = output;
            _userRepository = new UserRepository(GetTestDbContext());
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

        [Theory]
        [MemberData(nameof(GetUserPositive))]
        public async Task CreateUserAsync_ShouldCreateNewUser(AppUser user)
        {
            var result = await _userRepository.CreateUserAsync(user);
            var newUser = await Context.Users.Where(u => u.Id == user.Id).SingleOrDefaultAsync();
            Assert.True(result);
            Assert.Equal(user, newUser);
        }
        
    }
}