using AutoMapper;
using Database.Models.Domain;
using Database.Models.DTOs.UserAndRelatedEntities.Follow;
using Database.Repositories.Implementaions;
using Database.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Database.Tests.Repositories
{
    public class SQLUserRepositoryTests : IDisposable
    {
        private readonly NoorSphere _context;
        private readonly SQLUserRepository _repository;
        private readonly Mock<IMapper> _mockMapper;

        public SQLUserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<NoorSphere>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new NoorSphere(options);
            _mockMapper = new Mock<IMapper>();
            _repository = new SQLUserRepository(_context, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", UserName = "user1", Email = "user1@test.com" },
                new User { Id = "2", UserName = "user2", Email = "user2@test.com" }
            };

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllUsers();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(u => u.UserName == "user1");
            result.Should().Contain(u => u.UserName == "user2");
        }

        [Fact]
        public async Task GetUser_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "testuser", Email = "test@test.com" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUser("1");

            // Assert
            result.Should().NotBeNull();
            result!.UserName.Should().Be("testuser");
            result.Email.Should().Be("test@test.com");
        }

        [Fact]
        public async Task GetUser_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetUser("nonexistent");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteUser_WithValidUser_ShouldDeleteUserAndPosts()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "testuser", Email = "test@test.com" };
            var post = new Post { Id = 1, UserId = "1", Text = "Test post" };
            
            _context.Users.Add(user);
            _context.posts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.DeleteUser(user);

            // Assert
            result.Should().BeTrue();
            _context.Users.Should().BeEmpty();
            _context.posts.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteUser_WithException_ShouldReturnFalse()
        {
            // Arrange
            var user = new User { Id = "1", UserName = "testuser", Email = "test@test.com" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Dispose context to simulate database error
            _context.Dispose();

            // Act
            var result = await _repository.DeleteUser(user);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task AddFollow_WithValidData_ShouldAddFollow()
        {
            // Arrange
            var addFollowDto = new AddNewFollowDTO
            {
                FollowerUserId = "1",
                FollowedUserId = "2"
            };

            var follow = new Follow
            {
                Id = 1,
                FollowerUserId = "1",
                FollowedUserId = "2"
            };

            var getFollowDto = new GetFollowDTO
            {
                Id = 1,
                FollowerUserId = "1",
                FollowedUserId = "2"
            };

            _mockMapper.Setup(m => m.Map<Follow>(addFollowDto)).Returns(follow);
            _mockMapper.Setup(m => m.Map<GetFollowDTO>(follow)).Returns(getFollowDto);

            // Act
            var result = await _repository.AddFollow(addFollowDto);

            // Assert
            result.Should().NotBeNull();
            result.FollowerUserId.Should().Be("1");
            result.FollowedUserId.Should().Be("2");
            
            var savedFollow = await _context.follow.FirstAsync();
            savedFollow.FollowerUserId.Should().Be("1");
            savedFollow.FollowedUserId.Should().Be("2");
        }

        [Fact]
        public async Task IsFollowingExist_WithExistingFollow_ShouldReturnTrue()
        {
            // Arrange
            var follow = new Follow
            {
                Id = 1,
                FollowerUserId = "1",
                FollowedUserId = "2"
            };

            _context.follow.Add(follow);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.IsFollowingExist("1", "2");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsFollowingExist_WithNonExistingFollow_ShouldReturnFalse()
        {
            // Act
            var result = await _repository.IsFollowingExist("1", "2");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllFollows_WithValidUserId_ShouldReturnFollows()
        {
            // Arrange
            var follows = new List<Follow>
            {
                new Follow { Id = 1, FollowerUserId = "1", FollowedUserId = "2" },
                new Follow { Id = 2, FollowerUserId = "3", FollowedUserId = "2" }
            };

            _context.follow.AddRange(follows);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllFollows("2");

            // Assert
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(f => f.FollowedUserId.Should().Be("2"));
        }

        [Fact]
        public async Task GetFollowsAndFollowers_WithValidUserId_ShouldReturnCounts()
        {
            // Arrange
            var follows = new List<Follow>
            {
                new Follow { Id = 1, FollowerUserId = "1", FollowedUserId = "2" }, // User 2 is followed by User 1
                new Follow { Id = 2, FollowerUserId = "3", FollowedUserId = "2" }, // User 2 is followed by User 3
                new Follow { Id = 3, FollowerUserId = "2", FollowedUserId = "4" }  // User 2 follows User 4
            };

            _context.follow.AddRange(follows);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetFollowsAndFollowers("2");

            // Assert
            result.Should().NotBeNull();
            result.Followers.Should().Be(2); // User 2 has 2 followers
            result.Following.Should().Be(1); // User 2 follows 1 user
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

