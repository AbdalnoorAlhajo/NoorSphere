using Database.Models.Domain;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Server.Tests.Integration
{
    public class PostsControllerTests : TestBase
    {
        [Fact]
        public async Task GetPosts_WithValidToken_ShouldReturnPosts()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Posts/all?lastSeenId=0");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonSerializer.Deserialize<JsonElement[]>(content);
            posts.Should().NotBeNull();
            posts!.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetPosts_WithoutToken_ShouldReturnUnauthorized()
        {
            // Arrange
            var client = CreateClient();

            // Act
            var response = await client.GetAsync("/api/Posts/all?lastSeenId=0");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetPostById_WithValidId_ShouldReturnPost()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Posts/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var post = JsonSerializer.Deserialize<JsonElement>(content);
            post.TryGetProperty("id", out var id).Should().BeTrue();
            id.GetInt32().Should().Be(1);
        }

        [Fact]
        public async Task GetPostById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Posts/999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreatePost_WithValidData_ShouldCreatePost()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);
            
            var newPost = new
            {
                text = "New test post",
                name = "Test User 1"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Posts", newPost);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("message", out var message).Should().BeTrue();
            message.GetString().Should().Contain("Post created successfully");
        }

        [Fact]
        public async Task CreatePost_WithoutToken_ShouldReturnUnauthorized()
        {
            // Arrange
            var client = CreateClient();
            var newPost = new
            {
                text = "New test post",
                name = "Test User 1"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Posts", newPost);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task LikePost_WithValidPostId_ShouldToggleLike()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.PostAsync("/api/Posts/1/like", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("message", out var message).Should().BeTrue();
        }

        [Fact]
        public async Task LikePost_WithInvalidPostId_ShouldReturnNotFound()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.PostAsync("/api/Posts/999/like", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetPostsByText_WithValidSearchText_ShouldReturnMatchingPosts()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Posts/search?text=Test");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonSerializer.Deserialize<JsonElement[]>(content);
            posts.Should().NotBeNull();
        }

        [Fact]
        public async Task GetPostsByText_WithEmptySearchText_ShouldReturnBadRequest()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Posts/search?text=");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetPostsForSpecificUser_WithValidUserId_ShouldReturnUserPosts()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Posts/user/test-user-2");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonSerializer.Deserialize<JsonElement[]>(content);
            posts.Should().NotBeNull();
        }

        [Fact]
        public async Task GetPostsByFollowedUsers_WithValidToken_ShouldReturnFollowedUsersPosts()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Posts/following");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonSerializer.Deserialize<JsonElement[]>(content);
            posts.Should().NotBeNull();
        }

        [Fact]
        public async Task GetPostsLikedByUser_WithValidUserId_ShouldReturnLikedPosts()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Posts/liked/test-user-1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonSerializer.Deserialize<JsonElement[]>(content);
            posts.Should().NotBeNull();
        }
    }
}

