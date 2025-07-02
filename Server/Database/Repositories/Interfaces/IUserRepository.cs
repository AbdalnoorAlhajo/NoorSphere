using Database.Models.Domain;
using Database.Models.DTOs.UserAndRelatedEntities.Follow;


namespace Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User?> GetUser(string id);
        Task<bool> DeleteUser(User user);
        Task<GetFollowDTO> AddFollow(AddNewFollowDTO follow);
        Task<bool> IsFollowingExist(string followerId, string followedId);
        Task<List<Follow>> GetAllFollows(string UserID);

        Task<FollowsAndFollwoingDTO> GetFollowsAndFollowers(string UserID);
    }
}
