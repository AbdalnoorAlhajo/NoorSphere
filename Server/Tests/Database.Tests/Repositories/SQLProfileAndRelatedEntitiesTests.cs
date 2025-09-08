using Database.Models.Domain;
using Database.Models.DTOs.ProfileAndRelatedEntities.Profile;
using Database.Repositories.Implementaions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Database.Tests.Repositories
{
    public class SQLProfileAndRelatedEntitiesTests : IDisposable
    {
        private readonly NoorSphere _context;
        private readonly SQLProfileAndRelatedEntities _repository;

        public SQLProfileAndRelatedEntitiesTests()
        {
            var options = new DbContextOptionsBuilder<NoorSphere>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new NoorSphere(options);
            _repository = new SQLProfileAndRelatedEntities(_context);
        }

        [Fact]
        public async Task AddProfile_WithValidProfile_ShouldAddProfile()
        {
            // Arrange
            var profile = new Profile
            {
                Id = 1,
                UserId = "user1",
                AvatarUrl = "avatar.jpg",
                Bio = "Test bio"
            };

            // Act
            var result = await _repository.AddProfile(profile);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be("user1");
            result.Bio.Should().Be("Test bio");

            var savedProfile = await _context.profiles.FirstAsync();
            savedProfile.UserId.Should().Be("user1");
        }

        [Fact]
        public async Task UpdateProfile_WithValidProfile_ShouldUpdateProfile()
        {
            // Arrange
            var profile = new Profile
            {
                Id = 1,
                UserId = "user1",
                AvatarUrl = "avatar.jpg",
                Bio = "Original bio"
            };

            _context.profiles.Add(profile);
            await _context.SaveChangesAsync();

            profile.Bio = "Updated bio";

            // Act
            var result = await _repository.UpdateProfile(profile);

            // Assert
            result.Should().NotBeNull();
            result.Bio.Should().Be("Updated bio");

            var updatedProfile = await _context.profiles.FirstAsync();
            updatedProfile.Bio.Should().Be("Updated bio");
        }

        [Fact]
        public async Task GetProfile_WithValidId_ShouldReturnProfile()
        {
            // Arrange
            var profile = new Profile
            {
                Id = 1,
                UserId = "user1",
                AvatarUrl = "avatar.jpg",
                Bio = "Test bio"
            };

            _context.profiles.Add(profile);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetProfile("1");

            // Assert
            result.Should().NotBeNull();
            result!.UserId.Should().Be("user1");
            result.Bio.Should().Be("Test bio");
        }

        [Fact]
        public async Task GetProfile_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetProfile("999");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetProfileByUserId_WithValidUserId_ShouldReturnProfile()
        {
            // Arrange
            var profile = new Profile
            {
                Id = 1,
                UserId = "user1",
                AvatarUrl = "avatar.jpg",
                Bio = "Test bio"
            };

            _context.profiles.Add(profile);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetProfileByUserId("user1");

            // Assert
            result.Should().NotBeNull();
            result!.UserId.Should().Be("user1");
        }

        [Fact]
        public async Task AddExperience_WithValidExperience_ShouldAddExperience()
        {
            // Arrange
            var experience = new Experience
            {
                Id = 1,
                ProfileId = 1,
                Title = "Software Engineer",
                Company = "Test Company",
                StartDate = DateTime.Now.AddYears(-2),
                EndDate = DateTime.Now
            };

            // Act
            var result = await _repository.AddExperience(experience);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Software Engineer");
            result.Company.Should().Be("Test Company");

            var savedExperience = await _context.Experiences.FirstAsync();
            savedExperience.Title.Should().Be("Software Engineer");
        }

        [Fact]
        public async Task GetAllExperiences_WithValidProfileId_ShouldReturnExperiences()
        {
            // Arrange
            var experiences = new List<Experience>
            {
                new Experience { Id = 1, ProfileId = 1, Title = "Engineer", Company = "Company 1" },
                new Experience { Id = 2, ProfileId = 1, Title = "Senior Engineer", Company = "Company 2" },
                new Experience { Id = 3, ProfileId = 2, Title = "Manager", Company = "Company 3" }
            };

            _context.Experiences.AddRange(experiences);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllExperiences(1);

            // Assert
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(e => e.ProfileId.Should().Be(1));
        }

        [Fact]
        public async Task AddEducation_WithValidEducation_ShouldAddEducation()
        {
            // Arrange
            var education = new Education
            {
                Id = 1,
                ProfileId = 1,
                School = "Test University",
                Degree = "Bachelor of Science",
                StartDate = DateTime.Now.AddYears(-4),
                EndDate = DateTime.Now
            };

            // Act
            var result = await _repository.AddEducation(education);

            // Assert
            result.Should().NotBeNull();
            result.School.Should().Be("Test University");
            result.Degree.Should().Be("Bachelor of Science");

            var savedEducation = await _context.education.FirstAsync();
            savedEducation.School.Should().Be("Test University");
        }

        [Fact]
        public async Task GetAllEducation_WithValidProfileId_ShouldReturnEducation()
        {
            // Arrange
            var education = new List<Education>
            {
                new Education { Id = 1, ProfileId = 1, School = "University 1", Degree = "Bachelor" },
                new Education { Id = 2, ProfileId = 1, School = "University 2", Degree = "Master" },
                new Education { Id = 3, ProfileId = 2, School = "University 3", Degree = "PhD" }
            };

            _context.education.AddRange(education);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllEducation(1);

            // Assert
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(e => e.ProfileId.Should().Be(1));
        }

        [Fact]
        public async Task GetAllProfiles_WithCurrentUserId_ShouldReturnNonFollowingUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "user1", UserName = "user1", Email = "user1@test.com" },
                new User { Id = "user2", UserName = "user2", Email = "user2@test.com" },
                new User { Id = "user3", UserName = "user3", Email = "user3@test.com" }
            };

            var profiles = new List<Profile>
            {
                new Profile { Id = 1, UserId = "user1", AvatarUrl = "avatar1.jpg" },
                new Profile { Id = 2, UserId = "user2", AvatarUrl = "avatar2.jpg" },
                new Profile { Id = 3, UserId = "user3", AvatarUrl = "avatar3.jpg" }
            };

            var follow = new Follow { Id = 1, FollowerUserId = "currentUser", FollowedUserId = "user2" };

            _context.Users.AddRange(users);
            _context.profiles.AddRange(profiles);
            _context.follow.Add(follow);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllProfiles("currentUser");

            // Assert
            result.Should().HaveCount(2); // user1 and user3 (excluding currentUser and user2 who is followed)
            result.Should().NotContain(p => p.UserId == "currentUser");
            result.Should().NotContain(p => p.UserId == "user2");
        }

        [Fact]
        public async Task GetFollowingsSuggestions_WithValidUserId_ShouldReturnSuggestions()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "user1", UserName = "user1", Email = "user1@test.com" },
                new User { Id = "user2", UserName = "user2", Email = "user2@test.com" },
                new User { Id = "user3", UserName = "user3", Email = "user3@test.com" }
            };

            var profiles = new List<Profile>
            {
                new Profile { Id = 1, UserId = "user1", AvatarUrl = "avatar1.jpg" },
                new Profile { Id = 2, UserId = "user2", AvatarUrl = "avatar2.jpg" },
                new Profile { Id = 3, UserId = "user3", AvatarUrl = "avatar3.jpg" }
            };

            _context.Users.AddRange(users);
            _context.profiles.AddRange(profiles);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetFollowingsSuggestions("currentUser");

            // Assert
            result.Should().HaveCount(3);
            result.Should().AllSatisfy(s => s.UserId.Should().NotBe("currentUser"));
        }

        [Fact]
        public async Task GetFollowingsSuggestionsByName_WithValidName_ShouldReturnMatchingSuggestions()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "user1", UserName = "john_doe", Email = "john@test.com" },
                new User { Id = "user2", UserName = "jane_smith", Email = "jane@test.com" },
                new User { Id = "user3", UserName = "bob_wilson", Email = "bob@test.com" }
            };

            var profiles = new List<Profile>
            {
                new Profile { Id = 1, UserId = "user1", AvatarUrl = "avatar1.jpg" },
                new Profile { Id = 2, UserId = "user2", AvatarUrl = "avatar2.jpg" },
                new Profile { Id = 3, UserId = "user3", AvatarUrl = "avatar3.jpg" }
            };

            _context.Users.AddRange(users);
            _context.profiles.AddRange(profiles);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetFollowingsSuggestionsByName("currentUser", "john");

            // Assert
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("john_doe");
        }

        [Fact]
        public async Task GetImageURL_WithValidUserId_ShouldReturnImageUrl()
        {
            // Arrange
            var profile = new Profile
            {
                Id = 1,
                UserId = "user1",
                AvatarUrl = "https://example.com/avatar.jpg"
            };

            _context.profiles.Add(profile);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetImageURL("user1");

            // Assert
            result.Should().Be("https://example.com/avatar.jpg");
        }

        [Fact]
        public async Task GetImageURL_WithInvalidUserId_ShouldReturnEmptyString()
        {
            // Act
            var result = await _repository.GetImageURL("nonexistent");

            // Assert
            result.Should().BeEmpty();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

