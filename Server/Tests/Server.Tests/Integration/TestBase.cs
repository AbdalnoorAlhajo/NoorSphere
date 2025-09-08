using Database;
using Database.Models.Domain;
using Database.Repositories.Implementaions;
using Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Server.Tests.Integration
{
    public class TestBase : WebApplicationFactory<Program>, IAsyncLifetime
    {
        protected NoorSphere TestDbContext { get; private set; } = null!;
        protected UserManager<User> UserManager { get; private set; } = null!;
        protected ITokenRepository TokenRepository { get; private set; } = null!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the real database
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<NoorSphere>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Add in-memory database
                services.AddDbContext<NoorSphere>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Configure JWT for testing
                services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TestKeyForJWTTokenGeneration123456789"))
                    };
                });

                // Build the service provider
                var serviceProvider = services.BuildServiceProvider();
                TestDbContext = serviceProvider.GetRequiredService<NoorSphere>();
                UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
                TokenRepository = serviceProvider.GetRequiredService<ITokenRepository>();
            });

            builder.UseEnvironment("Testing");
        }

        public async Task InitializeAsync()
        {
            await TestDbContext.Database.EnsureCreatedAsync();
            await SeedTestDataAsync();
        }

        public new async Task DisposeAsync()
        {
            await TestDbContext.Database.EnsureDeletedAsync();
            TestDbContext.Dispose();
        }

        protected virtual async Task SeedTestDataAsync()
        {
            // Create test users
            var testUser1 = new User
            {
                Id = "test-user-1",
                UserName = "testuser1",
                Email = "test1@example.com",
                EmailConfirmed = true
            };

            var testUser2 = new User
            {
                Id = "test-user-2",
                UserName = "testuser2",
                Email = "test2@example.com",
                EmailConfirmed = true
            };

            await UserManager.CreateAsync(testUser1, "TestPassword123!");
            await UserManager.CreateAsync(testUser2, "TestPassword123!");

            // Create test profiles
            var profile1 = new Profile
            {
                Id = 1,
                UserId = "test-user-1",
                AvatarUrl = "https://example.com/avatar1.jpg",
                Bio = "Test user 1 bio"
            };

            var profile2 = new Profile
            {
                Id = 2,
                UserId = "test-user-2",
                AvatarUrl = "https://example.com/avatar2.jpg",
                Bio = "Test user 2 bio"
            };

            TestDbContext.profiles.AddRange(profile1, profile2);

            // Create test posts
            var post1 = new Post
            {
                Id = 1,
                Text = "Test post 1",
                UserId = "test-user-1",
                Name = "Test User 1",
                Date = DateTime.Now,
                PostId = null
            };

            var post2 = new Post
            {
                Id = 2,
                Text = "Test post 2",
                UserId = "test-user-2",
                Name = "Test User 2",
                Date = DateTime.Now,
                PostId = null
            };

            TestDbContext.posts.AddRange(post1, post2);

            await TestDbContext.SaveChangesAsync();
        }

        protected string GenerateJwtToken(User user)
        {
            return TokenRepository.CreateJWTToken(user);
        }

        protected HttpClient CreateAuthenticatedClient(User user)
        {
            var client = CreateClient();
            var token = GenerateJwtToken(user);
            client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}

