using Database.Models.Domain;
using Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Database.Repositories.Implementaions
{
    public class SQLPostAndRelatedEntitiesRepository : IPostAndRelatedEntitiesRepository
    {
        private readonly NoorSphere _dbApp;

        public SQLPostAndRelatedEntitiesRepository(NoorSphere dbApp)
        {
            _dbApp = dbApp;
        }

        public async Task<Comment> AddComment(Comment newComment)
        {
            _dbApp.comments.Add(newComment);
            await _dbApp.SaveChangesAsync();
            return newComment;
        }

        public async Task<Post> AddPost(Post newPost)
        {
            _dbApp.posts.Add(newPost);
            await _dbApp.SaveChangesAsync();
            return newPost;
        }

        public async Task<List<Comment>> GetAllComments(int PostId)
        {
            return await _dbApp.comments.Where(c=> c.PostId == PostId).ToListAsync();
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _dbApp.posts.ToListAsync();
        }

        public async Task<Post?> GetPost(int PostID)
        {
            return await _dbApp.posts.FindAsync(PostID);
        }
    }
}
