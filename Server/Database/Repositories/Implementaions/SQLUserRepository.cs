using Database.Models.Domain;
using Database.Repositories.Interfaces;
using Database.Utils;
using Microsoft.EntityFrameworkCore;


namespace Database.Repositories.Implementaions
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly NoorSphere _dbApp;

        public SQLUserRepository(NoorSphere dbApp)
        {
            _dbApp = dbApp;
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
    }
}
