using Database.Models.Domain;
using Database.Models.DTOs.PostAndRelatedEntities.Post;
using Database.Repositories.Implementaions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Database.Tests.Repositories
{
    public class SQLPostAndRelatedEntitiesRepositoryTests : IDisposable
    {
        private readonly NoorSphere _context;
        private readonly SQLPostAndRelatedEntitiesRepository _repository;

        public SQLPostAndRelatedEntitiesRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<NoorSphere>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new NoorSphere(options);
            _repository = new SQLPostAndRelatedEntitiesRepository(_context);
        }

        [Fact]
        public async Task AddPost_WithValidPost_ShouldAddPost()
        {
            // Arrange
            var post = new Post
            {
                Id = 1,
                Text = "Test post",
                UserId = "user1",
                Name = "Test User",
                Date = DateTime.Now
            };

            // Act
            var result = await _repository.AddPost(post);

            // Assert
            result.Should().NotBeNull();
            result.Text.Should().Be("Test post");
            result.UserId.Should().Be("user1");

            var savedPost = await _context.posts.FirstAsync();
            savedPost.Text.Should().Be("Test post");
        }

        [Fact]
        public async Task AddLike_WithValidLike_ShouldAddLike()
        {
            // Arrange
            var like = new Like
            {
                Id = 1,
                UserId = "user1",
                PostId = 1
            };

            // Act
            var result = await _repository.AddLike(like);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be("user1");
            result.PostId.Should().Be(1);

            var savedLike = await _context.likes.FirstAsync();
            savedLike.UserId.Should().Be("user1");
        }

        [Fact]
        public async Task GetAllPosts_WithValidData_ShouldReturnPosts()
        {
            // Arrange
            var user = new User { Id = "user1", UserName = "testuser", Email = "test@test.com" };
            var profile = new Profile { Id = 1, UserId = "user1", AvatarUrl = "avatar.jpg" };
            var posts = new List<Post>
            {
                new Post { Id = 1, Text = "Post 1", UserId = "user1", Name = "Test User", Date = DateTime.Now, PostId = null },
                new Post { Id = 2, Text = "Post 2", UserId = "user1", Name = "Test User", Date = DateTime.Now, PostId = null }
            };

            _context.Users.Add(user);
            _context.profiles.Add(profile);
            _context.posts.AddRange(posts);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllPosts("user1", 0);

            // Assert
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(p => p.Text.Should().StartWith("Post"));
        }

        [Fact]
        public async Task GetAllPosts_WithLastSeenId_ShouldReturnOnlyNewerPosts()
        {
            // Arrange
            var user = new User { Id = "user1", UserName = "testuser", Email = "test@test.com" };
            var profile = new Profile { Id = 1, UserId = "user1", AvatarUrl = "avatar.jpg" };
            var posts = new List<Post>
            {
                new Post { Id = 1, Text = "Post 1", UserId = "user1", Name = "Test User", Date = DateTime.Now, PostId = null },
                new Post { Id = 2, Text = "Post 2", UserId = "user1", Name = "Test User", Date = DateTime.Now, PostId = null },
                new Post { Id = 3, Text = "Post 3", UserId = "user1", Name = "Test User", Date = DateTime.Now, PostId = null }
            };

            _context.Users.Add(user);
            _context.profiles.Add(profile);
            _context.posts.AddRange(posts);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllPosts("user1", 1);

            // Assert
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(p => p.Id.Should().BeGreaterThan(1));
        }

        [Fact]
        public async Task GetPost_WithValidId_ShouldReturnPost()
        {
            // Arrange
            var user = new User { Id = "user1", UserName = "testuser", Email = "test@test.com" };
            var profile = new Profile { Id = 1, UserId = "user1", AvatarUrl = "avatar.jpg" };
            var post = new Post { Id = 1, Text = "Test post", UserId = "user1", Name = "Test User", Date = DateTime.Now };

            _context.Users.Add(user);
            _context.profiles.Add(profile);
            _context.posts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetPost(1, "user1");

            // Assert
            result.Should().NotBeNull();
            result!.Text.Should().Be("Test post");
            result.Name.Should().Be("Test User");
        }

        [Fact]
        public async Task GetPost_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetPost(999, "user1");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPostsForSpecificUser_WithValidUserId_ShouldReturnUserPosts()
        {
            // Arrange
            var user = new User { Id = "user1", UserName = "testuser", Email = "test@test.com" };
            var profile = new Profile { Id = 1, UserId = "user1", AvatarUrl = "avatar.jpg" };
            var posts = new List<Post>
            {
                new Post { Id = 1, Text = "User 1 Post", UserId = "user1", Name = "User 1", Date = DateTime.Now, PostId = null },
                new Post { Id = 2, Text = "User 2 Post", UserId = "user2", Name = "User 2", Date = DateTime.Now, PostId = null }
            };

            _context.Users.Add(user);
            _context.profiles.Add(profile);
            _context.posts.AddRange(posts);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetPostsForSpecificUser("user1", "currentUser");

            // Assert
            result.Should().HaveCount(1);
            result.First().Text.Should().Be("User 1 Post");
        }

        [Fact]
        public async Task GetPostsByText_WithValidSearchText_ShouldReturnMatchingPosts()
        {
            // Arrange
            var user = new User { Id = "user1", UserName = "testuser", Email = "test@test.com" };
            var profile = new Profile { Id = 1, UserId = "user1", AvatarUrl = "avatar.jpg" };
            var posts = new List<Post>
            {
                new Post { Id = 1, Text = "Software Engineer post", UserId = "user1", Name = "Test User", Date = DateTime.Now, PostId = null },
                new Post { Id = 2, Text = "Developer post", UserId = "user1", Name = "Test User", Date = DateTime.Now, PostId = null },
                new Post { Id = 3, Text = "Design post", UserId = "user1", Name = "Test User", Date = DateTime.Now, PostId = null }
            };

            _context.Users.Add(user);
            _context.profiles.Add(profile);
            _context.posts.AddRange(posts);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetPostsByText("Software", "user1");

            // Assert
            result.Should().HaveCount(1);
            result.First().Text.Should().Be("Software Engineer post");
        }

        [Fact]
        public async Task GetPostsByText_WithEmptySearchText_ShouldReturnEmptyList()
        {
            // Act
            var result = await _repository.GetPostsByText("", "user1");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllComments_WithValidPostId_ShouldReturnComments()
        {
            // Arrange
            var user = new User { Id = "user1", UserName = "testuser", Email = "test@test.com" };
            var profile = new Profile { Id = 1, UserId = "user1", AvatarUrl = "avatar.jpg" };
            var mainPost = new Post { Id = 1, Text = "Main post", UserId = "user1", Name = "Test User", Date = DateTime.Now, PostId = null };
            var comment = new Post { Id = 2, Text = "Comment", UserId = "user1", Name = "Test User", Date = DateTime.Now, PostId = 1 };

            _context.Users.Add(user);
            _context.profiles.Add(profile);
            _context.posts.Add(mainPost);
            _context.posts.Add(comment);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllComments(1, "user1");

            // Assert
            result.Should().HaveCount(1);
            result.First().Text.Should().Be("Comment");
        }

        [Fact]
        public async Task GetPostsByFollowedUsers_WithValidUserId_ShouldReturnFollowedUsersPosts()
        {
            // Arrange
            var user1 = new User { Id = "user1", UserName = "user1", Email = "user1@test.com" };
            var user2 = new User { Id = "user2", UserName = "user2", Email = "user2@test.com" };
            var profile1 = new Profile { Id = 1, UserId = "user1", AvatarUrl = "avatar1.jpg" };
            var profile2 = new Profile { Id = 2, UserId = "user2", AvatarUrl = "avatar2.jpg" };
            var follow = new Follow { Id = 1, FollowerUserId = "user1", FollowedUserId = "user2" };
            var post = new Post { Id = 1, Text = "Followed user post", UserId = "user2", Name = "User 2", Date = DateTime.Now, PostId = null };

            _context.Users.AddRange(user1, user2);
            _context.profiles.AddRange(profile1, profile2);
            _context.follow.Add(follow);
            _context.posts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetPostsByFollowedUsers("user1");

            // Assert
            result.Should().HaveCount(1);
            result.First().Text.Should().Be("Followed user post");
        }

        [Fact]
        public async Task GetPostsLikedByUser_WithValidUserId_ShouldReturnLikedPosts()
        {
            // Arrange
            var user1 = new User { Id = "user1", UserName = "user1", Email = "user1@test.com" };
            var user2 = new User { Id = "user2", UserName = "user2", Email = "user2@test.com" };
            var profile1 = new Profile { Id = 1, UserId = "user1", AvatarUrl = "avatar1.jpg" };
            var profile2 = new Profile { Id = 2, UserId = "user2", AvatarUrl = "avatar2.jpg" };
            var post = new Post { Id = 1, Text = "Liked post", UserId = "user2", Name = "User 2", Date = DateTime.Now, PostId = null };
            var like = new Like { Id = 1, UserId = "user1", PostId = 1 };

            _context.Users.AddRange(user1, user2);
            _context.profiles.AddRange(profile1, profile2);
            _context.posts.Add(post);
            _context.likes.Add(like);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetPostsLikedByUser("user1", "user1");

            // Assert
            result.Should().HaveCount(1);
            result.First().Text.Should().Be("Liked post");
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

