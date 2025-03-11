using Database.Models;


namespace Database.Repositories.Interfaces
{
    public interface IProfileAndRelatedEntities
    {
        Task<Profile> AddProfile(Profile newProfile);
        Task<List<Profile>> GetAllProfiles();
        Task<Profile?> GetProfile(int id);
        Task<Experience> AddExperience(Experience newExperience);
        Task<List<Experience>> GetAllExperiences(int ProfileId);
        Task<Education> AddEducation(Education newEducation);
        Task<List<Education>> GetAllEducation(int ProfileId);
    }
}
