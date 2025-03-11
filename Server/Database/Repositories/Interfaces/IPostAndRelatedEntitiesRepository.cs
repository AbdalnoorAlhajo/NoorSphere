using Database.Models;


namespace Database.Repositories.Interfaces
{
    public interface IPostAndRelatedEntitiesRepository
    {
        Task<Post> AddPost(Post newPost);
        Task<List<Post>> GetAllPosts();
        Task<Comment> AddComment(Comment newComment);
        Task<List<Comment>> GetAllComments(int PostID);
        Task<Post?> GetPost(int PostID);
    }
}
