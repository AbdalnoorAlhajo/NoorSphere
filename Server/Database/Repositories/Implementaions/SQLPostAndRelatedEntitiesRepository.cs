using Database.Models.Domain;
using Database.Models.DTOs.PostAndRelatedEntities.Post;
using Database.Models.DTOs.UserAndRelatedEntities.Follow;
using Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Database.Repositories.Implementaions
{
    public class SQLPostAndRelatedEntitiesRepository : IPostAndRelatedEntitiesRepository
    {
        private readonly NoorSphere _dbApp;

        public SQLPostAndRelatedEntitiesRepository(NoorSphere dbApp)
        {
            _dbApp = dbApp;
        }

        public async Task<Like> AddLike(Like newLike)
        {
            _dbApp.likes.Add(newLike);
            await _dbApp.SaveChangesAsync();
            return newLike;
        }

        public async Task<Post> AddPost(Post newPost)
        {
            _dbApp.posts.Add(newPost);
            await _dbApp.SaveChangesAsync();
            return newPost;
        }

        public async Task<List<GetPostDTO>> GetAllComments(int PostId, string currentUserId)
        {
            return await (from posts in _dbApp.posts
                          join profile in _dbApp.profiles
                              on posts.UserId equals profile.UserId
                          where posts.PostId == PostId
                          select new GetPostDTO
                          {
                              Id = posts.Id,
                              Text = posts.Text,
                              Date = posts.Date,
                              Name = posts.Name,
                              ImageURL = posts.ImageURL,
                              AvatarURL = profile.AvatarUrl,
                              likes = posts.likes.Count,
                              IsLiked = posts.likes.Any(l => l.UserId == currentUserId),
                              comments = _dbApp.posts.Count(c => c.PostId == posts.Id)
                          }).ToListAsync();
        }



        public async Task<TrendingTopicDTO[]> GetTrendingTopics()
        {
            return new TrendingTopicDTO[]
            {
                new TrendingTopicDTO
                {
                    Id = 1,
                    Topic = "NoorSphere",
                    Count = await _dbApp.posts.CountAsync(p => p.Text.Contains("NoorSphere"))
                },
                new TrendingTopicDTO
                {
                    Id = 2,
                    Topic = "Social Media",
                    Count = await _dbApp.posts.CountAsync(p => p.Text.Contains("Social Media"))
                },
                new TrendingTopicDTO
                {
                    Id = 3,
                    Topic = "Software Engineer",
                    Count = await _dbApp.posts.CountAsync(p => p.Text.Contains("Software Engineer"))
                }
            }
            .OrderByDescending(t => t.Count)
            .ToArray();
        }

        public async Task<List<GetPostDTO>> GetAllPosts(string currentUserId, int lastSeenId)
        {
            return await (from post in _dbApp.posts
                               join profile in _dbApp.profiles
                                   on post.UserId equals profile.UserId
                                   where post.PostId == null
                               select new GetPostDTO
                               {
                                   Id = post.Id,
                                   Text = post.Text,
                                   Date = post.Date,
                                   Name = post.Name,
                                   ImageURL = post.ImageURL,
                                   AvatarURL = profile.AvatarUrl,
                                   likes = post.likes.Count,
                                   IsLiked = post.likes.Any(l => l.UserId == currentUserId),
                                   comments = _dbApp.posts.Count(c => c.PostId == post.Id)
                               }).OrderBy(p => p.Id).Where(p => p.Id > lastSeenId)
    .Take(2).ToListAsync(); 

        }


        public async Task<List<GetPostDTO>> GetPostsForSpecificUser(string userId, string currentUserId)
        {
            return await (from post in _dbApp.posts
                          join profile in _dbApp.profiles
                              on post.UserId equals profile.UserId
                          where post.UserId == userId && post.PostId == null
                          select new GetPostDTO
                          {
                              Id = post.Id,
                              Text = post.Text,
                              Date = post.Date,
                              Name = post.Name,
                              ImageURL = post.ImageURL,
                              AvatarURL = profile.AvatarUrl,
                              likes = post.likes.Count,
                              IsLiked = post.likes.Any(l => l.UserId == currentUserId),
                              comments = _dbApp.posts.Count(c => c.PostId == post.Id)
                          }).ToListAsync();
        }



        public async Task<List<GetPostDTO>> GetPostsByText(string searchText, string currentUserId)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<GetPostDTO>();

            return await (from post in _dbApp.posts
                          join profile in _dbApp.profiles
                              on post.UserId equals profile.UserId
                          where post.Text.Contains(searchText) && post.PostId == null
                          select new GetPostDTO
                          {
                              Id = post.Id,
                              Text = post.Text,
                              Date = post.Date,
                              Name = post.Name,
                              ImageURL = post.ImageURL,
                              AvatarURL = profile.AvatarUrl,
                              likes = post.likes.Count,
                              IsLiked = post.likes.Any(l => l.UserId == currentUserId),
                              comments = _dbApp.posts.Count(c => c.PostId == post.Id)
                          }).ToListAsync();
        }



        public async Task<GetPostDTO?> GetPost(int PostID, string currentUserId)
        {
            return await (from post in _dbApp.posts
                          join profile in _dbApp.profiles
                              on post.UserId equals profile.UserId
                          where post.Id == PostID
                          select new GetPostDTO
                          {
                              Id = post.Id,
                              Text = post.Text,
                              Date = post.Date,
                              Name = post.Name,
                              ImageURL = post.ImageURL,
                              AvatarURL = profile.AvatarUrl,
                              likes = post.likes.Count,
                              IsLiked = post.likes.Any(l => l.UserId == currentUserId),
                              comments = _dbApp.posts.Count(c => c.PostId == post.Id)
                          }).FirstOrDefaultAsync();
        }

        public async Task<List<GetPostDTO>> GetPostsLikedByUser(string userId, string currentUserId)
        {
            var likedPostIds = await _dbApp.likes
                .Where(l => l.UserId == userId)
                .Select(l => l.PostId)
                .ToListAsync();

            return await (from post in _dbApp.posts
                          join profile in _dbApp.profiles
                              on post.UserId equals profile.UserId
                          where likedPostIds.Contains(post.Id) && post.PostId == null
                          select new GetPostDTO
                          {
                              Id = post.Id,
                              Text = post.Text,
                              Date = post.Date,
                              Name = post.Name,
                              ImageURL = post.ImageURL,
                              AvatarURL = profile.AvatarUrl,
                              likes = post.likes.Count,
                              IsLiked = post.likes.Any(l => l.UserId == currentUserId),
                              comments = _dbApp.posts.Count(c => c.PostId == post.Id)
                          }).ToListAsync();
        }


        public async Task<List<GetPostDTO>> GetPostsByFollowedUsers(string currentUserId)
        {
            var followedUserIds = await _dbApp.follow
                .Where(f => f.FollowerUserId == currentUserId)
                .Select(f => f.FollowedUserId)
                .ToListAsync();

            return await (from post in _dbApp.posts
                          join profile in _dbApp.profiles
                              on post.UserId equals profile.UserId
                          where followedUserIds.Contains(post.UserId) && post.PostId == null
                          select new GetPostDTO
                          {
                              Id = post.Id,
                              Text = post.Text,
                              Date = post.Date,
                              Name = post.Name,
                              ImageURL = post.ImageURL,
                              AvatarURL = profile.AvatarUrl,
                              likes = post.likes.Count,
                              IsLiked = post.likes.Any(l => l.UserId == currentUserId),
                              comments = _dbApp.posts.Count(c => c.PostId == post.Id)

                          }).ToListAsync();
        }

    }
}
