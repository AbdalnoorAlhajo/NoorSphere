using Database.Models;


namespace Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUser(User newUser);
        Task<List<User>> GetAllUsers();
        Task<User?> GetUser(int id);
        Task<User?> GetUser(string email);
        Task<User?> GetUserByCredentials(string email, string password);
    }
}
