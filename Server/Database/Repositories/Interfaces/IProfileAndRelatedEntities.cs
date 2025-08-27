using Database.Models;
using Database.Models.Domain;
using Database.Models.DTOs.ProfileAndRelatedEntities.Profile;
using Database.Models.DTOs.UserAndRelatedEntities.Follow;


namespace Database.Repositories.Interfaces
{
    public interface IProfileAndRelatedEntities
    {
        Task<Profile> AddProfile(Profile newProfile);
        Task<Profile> UpdateProfile(Profile UpdatedProfile);

        Task<List<FollowingSuggestionsDTO>> GetAllProfiles(string currentUserId);
        Task<Profile?> GetProfile(string id);
        Task<Profile?> GetProfileByUserId(string UserId);

        Task<Experience> AddExperience(Experience newExperience);
        Task<List<Experience>> GetAllExperiences(int ProfileId);
        Task<Education> AddEducation(Education newEducation);
        Task<List<Education>> GetAllEducation(int ProfileId);

        Task<FollowingSuggestionsDTO[]> GetFollowingsSuggestions(string currentUserId);

        Task<List<FollowingSuggestionsDTO>> GetFollowingsSuggestionsByName(string currentUserId, string Username);

        Task<string> GetImageURL(string UserId);
    }
}
