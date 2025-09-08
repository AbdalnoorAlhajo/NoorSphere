using Database.Models.Domain;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Server.Tests.Integration
{
    public class UsersControllerTests : TestBase
    {
        [Fact]
        public async Task GetAllUsers_WithValidToken_ShouldReturnUsers()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Users");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<JsonElement[]>(content);
            users.Should().NotBeNull();
            users!.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetAllUsers_WithoutToken_ShouldReturnUnauthorized()
        {
            // Arrange
            var client = CreateClient();

            // Act
            var response = await client.GetAsync("/api/Users");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetUserById_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Users/test-user-2");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var userResult = JsonSerializer.Deserialize<JsonElement>(content);
            userResult.TryGetProperty("id", out var id).Should().BeTrue();
            id.GetString().Should().Be("test-user-2");
        }

        [Fact]
        public async Task GetUserById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Users/nonexistent-user");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteUser_WithValidId_ShouldDeleteUser()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.DeleteAsync("/api/Users/test-user-2");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("message", out var message).Should().BeTrue();
            message.GetString().Should().Contain("User deleted successfully");
        }

        [Fact]
        public async Task DeleteUser_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.DeleteAsync("/api/Users/nonexistent-user");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddFollow_WithValidData_ShouldAddFollow()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);
            
            var followData = new
            {
                followerUserId = "test-user-1",
                followedUserId = "test-user-2"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Users/follow", followData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("message", out var message).Should().BeTrue();
            message.GetString().Should().Contain("Follow added successfully");
        }

        [Fact]
        public async Task AddFollow_WithSameUser_ShouldReturnBadRequest()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);
            
            var followData = new
            {
                followerUserId = "test-user-1",
                followedUserId = "test-user-1" // Same user
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Users/follow", followData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddFollow_WithExistingFollow_ShouldReturnBadRequest()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);
            
            // First, add a follow
            var followData = new
            {
                followerUserId = "test-user-1",
                followedUserId = "test-user-2"
            };

            await client.PostAsJsonAsync("/api/Users/follow", followData);

            // Try to add the same follow again
            var response = await client.PostAsJsonAsync("/api/Users/follow", followData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetFollowsAndFollowers_WithValidUserId_ShouldReturnCounts()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Users/test-user-1/follows");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("followers", out var followers).Should().BeTrue();
            result.TryGetProperty("following", out var following).Should().BeTrue();
            
            followers.GetInt32().Should().BeGreaterOrEqualTo(0);
            following.GetInt32().Should().BeGreaterOrEqualTo(0);
        }

        [Fact]
        public async Task GetFollowsAndFollowers_WithInvalidUserId_ShouldReturnNotFound()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Users/nonexistent-user/follows");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}

