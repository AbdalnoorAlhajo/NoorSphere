using Database.Models.Domain;


namespace Database.Repositories.Interfaces
{
    public interface IPostAndRelatedEntitiesRepository
    {
        Task<Post> AddPost(Post newPost);
        Task<List<Post>> GetAllPosts();
        Task<Comment> AddComment(Comment newComment);
        Task<Like> AddLike(Like newLike);

        Task<List<Comment>> GetAllComments(int PostID);
        Task<Post?> GetPost(int PostID);
    }
}
