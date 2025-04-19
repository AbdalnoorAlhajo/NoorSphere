using Database.Models;
using Database.Models.Domain;
using Database.Models.DTOs.ProfileAndRelatedEntities.Profile;


namespace Database.Repositories.Interfaces
{
    public interface IProfileAndRelatedEntities
    {
        Task<Profile> AddProfile(Profile newProfile);
        Task<Profile> UpdateProfile(Profile UpdatedProfile);

        Task<List<GetProfilesWithName>> GetAllProfiles();
        Task<Profile?> GetProfile(int id);
        Task<Profile?> GetProfileByUserId(string UserId);

        Task<Experience> AddExperience(Experience newExperience);
        Task<List<Experience>> GetAllExperiences(int ProfileId);
        Task<Education> AddEducation(Education newEducation);
        Task<List<Education>> GetAllEducation(int ProfileId);
    }
}
