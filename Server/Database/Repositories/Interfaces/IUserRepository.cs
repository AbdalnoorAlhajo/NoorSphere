using Database.Models.Domain;


namespace Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User?> GetUser(string id);
        Task<bool> DeleteUser(User user);
    }
}
