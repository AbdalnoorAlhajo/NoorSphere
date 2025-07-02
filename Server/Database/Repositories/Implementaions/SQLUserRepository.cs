using AutoMapper;
using Database.Models.Domain;
using Database.Models.DTOs.ProfileAndRelatedEntities.Profile;
using Database.Models.DTOs.UserAndRelatedEntities.Follow;
using Database.Repositories.Interfaces;
using Database.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;


namespace Database.Repositories.Implementaions
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly NoorSphere _dbApp;
        private readonly IMapper _mapper;
        public SQLUserRepository(NoorSphere dbApp, IMapper mapper)
        {
            _dbApp = dbApp;
            _mapper = mapper;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _dbApp.Users.ToListAsync();
        }

        public async Task<User?> GetUser(string id)
        {
            return await _dbApp.Users.FindAsync(id);
        }

        public async Task<bool> DeleteUser(User user)
        {
            using var transcation = _dbApp.Database.BeginTransaction();

            try
            {
                var posts = await _dbApp.posts.Where(p => p.UserId == user.Id).ToListAsync();

                _dbApp.posts.RemoveRange(posts);
                _dbApp.Users.Remove(user);

                transcation.Commit();

                return await _dbApp.SaveChangesAsync() > 0;
            }
            catch(Exception ex)
            {
                transcation.Rollback();
                Console.WriteLine($"Error while deleting user: {ex.Message}");
                return false;
            }
        }

        public async Task<GetFollowDTO> AddFollow(AddNewFollowDTO newFollow)
        {
            var follow = _mapper.Map<Follow>(newFollow);

            await _dbApp.follow.AddAsync(follow);
            await _dbApp.SaveChangesAsync();
            return _mapper.Map<GetFollowDTO>(follow);
        }

        public async Task<bool> IsFollowingExist(string followerId, string followedId)
        {
            return await _dbApp.follow.AnyAsync(f => f.FollowerUserId == followerId && f.FollowedUserId == followedId);
        }

        public async Task<List<Follow>> GetAllFollows(string UserID)
        {
           return await _dbApp.follow.Where(f => f.FollowedUserId == UserID).ToListAsync();

        }

        public async Task<FollowsAndFollwoingDTO> GetFollowsAndFollowers(string UserID)
        {
            return new FollowsAndFollwoingDTO
            {
                Followers = await _dbApp.follow.CountAsync(f => f.FollowedUserId == UserID),
                Following = await _dbApp.follow.CountAsync(f => f.FollowerUserId == UserID)
            };
        }
    }
}
