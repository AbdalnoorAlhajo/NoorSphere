using Database.Models.Domain;
using Database.Models.DTOs.UserAndRelatedEntities.User;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Server.Tests.Integration
{
    public class AuthControllerTests : TestBase
    {
        [Fact]
        public async Task Register_WithValidData_ShouldReturnToken()
        {
            // Arrange
            var client = CreateClient();
            var newUser = new AddNewUserDTO
            {
                Name = "New User",
                Email = "newuser@example.com",
                Password = "NewPassword123!"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Auth/register", newUser);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("token", out var token).Should().BeTrue();
            token.GetString().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Register_WithExistingEmail_ShouldReturnBadRequest()
        {
            // Arrange
            var client = CreateClient();
            var existingUser = new AddNewUserDTO
            {
                Name = "Existing User",
                Email = "test1@example.com", // This email already exists in test data
                Password = "Password123!"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Auth/register", existingUser);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("message", out var message).Should().BeTrue();
            message.GetString().Should().Contain("Email is already in use");
        }

        [Fact]
        public async Task Register_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            var client = CreateClient();
            var invalidUser = new AddNewUserDTO
            {
                Name = "", // Invalid: empty name
                Email = "invalid-email", // Invalid: not a valid email
                Password = "123" // Invalid: too short
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Auth/register", invalidUser);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var client = CreateClient();
            var loginData = new LoginDTO
            {
                Email = "test1@example.com",
                Password = "TestPassword123!"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Auth/login", loginData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("token", out var token).Should().BeTrue();
            token.GetString().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldReturnBadRequest()
        {
            // Arrange
            var client = CreateClient();
            var invalidLogin = new LoginDTO
            {
                Email = "test1@example.com",
                Password = "WrongPassword"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Auth/login", invalidLogin);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_WithNonExistentUser_ShouldReturnNotFound()
        {
            // Arrange
            var client = CreateClient();
            var nonExistentUser = new LoginDTO
            {
                Email = "nonexistent@example.com",
                Password = "SomePassword123!"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Auth/login", nonExistentUser);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Login_WithGuestCredentials_ShouldReturnToken()
        {
            // Arrange
            var client = CreateClient();
            var guestLogin = new LoginDTO
            {
                Email = "Guest@noorsphere.com",
                Password = "ilovenoorsphere"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Auth/login", guestLogin);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            result.TryGetProperty("token", out var token).Should().BeTrue();
            token.GetString().Should().NotBeNullOrEmpty();
        }
    }
}

