using Database.Models.Domain;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Server.Tests.Integration
{
    public class ProfilesControllerTests : TestBase
    {
        [Fact]
        public async Task GetProfile_WithValidId_ShouldReturnProfile()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Profiles/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var profile = JsonSerializer.Deserialize<JsonElement>(content);
            profile.TryGetProperty("id", out var id).Should().BeTrue();
            id.GetInt32().Should().Be(1);
        }

        [Fact]
        public async Task GetProfile_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Profiles/999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetProfileByUserId_WithValidUserId_ShouldReturnProfile()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Profiles/user/test-user-1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var profile = JsonSerializer.Deserialize<JsonElement>(content);
            profile.TryGetProperty("userId", out var userId).Should().BeTrue();
            userId.GetString().Should().Be("test-user-1");
        }

        [Fact]
        public async Task GetProfileByUserId_WithInvalidUserId_ShouldReturnNotFound()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Profiles/user/nonexistent-user");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateProfile_WithValidData_ShouldUpdateProfile()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);
            
            var updateData = new
            {
                id = 1,
                userId = "test-user-1",
                bio = "Updated bio",
                avatarUrl = "https://example.com/new-avatar.jpg"
            };

            // Act
            var response = await client.PutAsJsonAsync("/api/Profiles/1", updateData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("message", out var message).Should().BeTrue();
            message.GetString().Should().Contain("Profile updated successfully");
        }

        [Fact]
        public async Task UpdateProfile_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);
            
            var updateData = new
            {
                id = 999,
                userId = "test-user-1",
                bio = "Updated bio"
            };

            // Act
            var response = await client.PutAsJsonAsync("/api/Profiles/999", updateData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAllProfiles_WithValidToken_ShouldReturnProfiles()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Profiles");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var profiles = JsonSerializer.Deserialize<JsonElement[]>(content);
            profiles.Should().NotBeNull();
            profiles!.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetAllProfiles_WithoutToken_ShouldReturnUnauthorized()
        {
            // Arrange
            var client = CreateClient();

            // Act
            var response = await client.GetAsync("/api/Profiles");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetFollowingsSuggestions_WithValidToken_ShouldReturnSuggestions()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Profiles/suggestions");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var suggestions = JsonSerializer.Deserialize<JsonElement[]>(content);
            suggestions.Should().NotBeNull();
        }

        [Fact]
        public async Task GetFollowingsSuggestionsByName_WithValidName_ShouldReturnMatchingSuggestions()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Profiles/suggestions/search?username=test");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var suggestions = JsonSerializer.Deserialize<JsonElement[]>(content);
            suggestions.Should().NotBeNull();
        }

        [Fact]
        public async Task GetImageURL_WithValidUserId_ShouldReturnImageUrl()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Profiles/image/test-user-1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("imageUrl", out var imageUrl).Should().BeTrue();
            imageUrl.GetString().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetImageURL_WithInvalidUserId_ShouldReturnEmptyString()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);

            // Act
            var response = await client.GetAsync("/api/Profiles/image/nonexistent-user");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("imageUrl", out var imageUrl).Should().BeTrue();
            imageUrl.GetString().Should().BeEmpty();
        }

        [Fact]
        public async Task AddExperience_WithValidData_ShouldAddExperience()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);
            
            var experienceData = new
            {
                profileId = 1,
                title = "Software Engineer",
                company = "Test Company",
                startDate = DateTime.Now.AddYears(-2),
                endDate = DateTime.Now
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Profiles/1/experience", experienceData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("message", out var message).Should().BeTrue();
            message.GetString().Should().Contain("Experience added successfully");
        }

        [Fact]
        public async Task AddEducation_WithValidData_ShouldAddEducation()
        {
            // Arrange
            var user = await UserManager.FindByIdAsync("test-user-1");
            var client = CreateAuthenticatedClient(user!);
            
            var educationData = new
            {
                profileId = 1,
                school = "Test University",
                degree = "Bachelor of Science",
                startDate = DateTime.Now.AddYears(-4),
                endDate = DateTime.Now
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Profiles/1/education", educationData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("message", out var message).Should().BeTrue();
            message.GetString().Should().Contain("Education added successfully");
        }
    }
}

