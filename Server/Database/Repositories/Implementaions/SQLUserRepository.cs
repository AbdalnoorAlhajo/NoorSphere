using Database.Models;
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
            return await _dbApp.users.ToListAsync();
        }

        public async Task<User?> GetUserByCredentials(string email, string password)
        {
            var user  = await _dbApp.users.FirstOrDefaultAsync(b => b.Email == email);

            if (user == null)
                return null;

            return UserService.VerifyPassword(password, user.PasswordHash) ? user : null;
        }

        public async Task<User?> GetUser(int id)
        {
            return await _dbApp.users.FindAsync(id);
        }

        public async Task<User?> GetUser(string email)
        {
            return await _dbApp.users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddUser(User newUser)
        {
            _dbApp.users.Add(newUser);
            await _dbApp.SaveChangesAsync();
            return newUser;
        }

    }
}
