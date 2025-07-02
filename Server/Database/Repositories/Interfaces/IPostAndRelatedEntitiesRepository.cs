using Database.Models.Domain;
using Database.Models.DTOs.PostAndRelatedEntities.Post;
using static Database.Repositories.Implementaions.SQLPostAndRelatedEntitiesRepository;


namespace Database.Repositories.Interfaces
{
    public interface IPostAndRelatedEntitiesRepository
    {
        Task<Post> AddPost(Post newPost);
        Task<List<GetPostDTO>> GetAllPosts(string currentUserId);
        Task<Like> AddLike(Like newLike);

        Task<TrendingTopicDTO[]> GetTrendingTopics();

        Task<List<GetPostDTO>> GetAllComments(int PostID, string currentUserId);
        Task<GetPostDTO?> GetPost(int PostID, string currentUserId);

        Task<List<GetPostDTO>> GetPostsByText(string searchText, string currentUserId);

        Task<List<GetPostDTO>> GetPostsForSpecificUser(string userId, string currentUserId);

        Task<List<GetPostDTO>> GetPostsByFollowedUsers(string currentUserId);

        Task<List<GetPostDTO>> GetPostsLikedByUser(string userId, string currentUserId);
    }
}
