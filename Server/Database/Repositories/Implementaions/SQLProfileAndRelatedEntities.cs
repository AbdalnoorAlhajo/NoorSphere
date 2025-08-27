using Database.Models.Domain;
using Database.Models.DTOs.ProfileAndRelatedEntities.Profile;
using Database.Models.DTOs.UserAndRelatedEntities.Follow;
using Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories.Implementaions
{
    public class SQLProfileAndRelatedEntities : IProfileAndRelatedEntities
    {
        private readonly NoorSphere _dbApp;

        public SQLProfileAndRelatedEntities(NoorSphere dbApp)
        {
            _dbApp = dbApp;
        }

        public async Task<Education> AddEducation(Education newEducation)
        {
            _dbApp.education.Add(newEducation);
            await _dbApp.SaveChangesAsync();
            return newEducation;
        }

        public async Task<Experience> AddExperience(Experience newExperience)
        {
            _dbApp.Experiences.Add(newExperience);
            await _dbApp.SaveChangesAsync();
            return newExperience;
        }

        public async Task<List<Education>> GetAllEducation(int ProfileId)
        {
            return await _dbApp.education.Where(p => p.ProfileId == ProfileId).ToListAsync();
        }

        public async Task<List<Experience>> GetAllExperiences(int ProfileId)
        {
            return await _dbApp.Experiences.Where(p => p.ProfileId == ProfileId).ToListAsync();
        }

        public async Task<List<FollowingSuggestionsDTO>> GetAllProfiles(string currentUserId)
        {
            var followingIds = await _dbApp.follow
               .Where(f => f.FollowerUserId == currentUserId)
               .Select(f => f.FollowedUserId)
               .ToListAsync();

            var suggestions = await _dbApp.Users
                .Where(u => u.Id != currentUserId && !followingIds.Contains(u.Id))
                .Join(
                    _dbApp.profiles,
                    user => user.Id,
                    profile => profile.UserId,
                    (user, profile) => new FollowingSuggestionsDTO
                    {
                        UserId = user.Id,
                        Name = user.UserName,
                        AvatarUrl = profile.AvatarUrl
                    }
                ).ToListAsync();
            return suggestions;
        }

        public async Task<Profile> AddProfile(Profile newProfile)
        {
            _dbApp.profiles.Add(newProfile);
            await _dbApp.SaveChangesAsync();
            return newProfile;
        }
        public async Task<Profile> UpdateProfile(Profile UpdatedProfile)
        {
            var profile = await _dbApp.profiles.FirstOrDefaultAsync(p => p.UserId == UpdatedProfile.UserId);

            if (profile != null)
            {
                profile.Website = UpdatedProfile.Website;
                profile.Skills = UpdatedProfile.Skills;
                profile.Status = UpdatedProfile.Status;
                profile.Bio = UpdatedProfile.Bio;
                profile.Country = UpdatedProfile.Country;
                profile.Company = UpdatedProfile.Company;
                profile.AvatarUrl = UpdatedProfile.AvatarUrl;

                _dbApp.profiles.Update(profile);
                await _dbApp.SaveChangesAsync();
            }

            return UpdatedProfile;
        }

        public async Task<Profile?> GetProfile(string id)
        {
            return await _dbApp.profiles.Include(p => p.Educations).Include(p => p.Experiences).FirstOrDefaultAsync(p => p.UserId == id);
        }

        public async Task<Profile?> GetProfileByUserId(string UserId)
        {
            return await _dbApp.profiles.Include(p => p.Educations).Include(p => p.Experiences).FirstOrDefaultAsync(p => p.UserId == UserId);
        }

        public async Task<FollowingSuggestionsDTO[]> GetFollowingsSuggestions(string currentUserId)
        {
            var followingIds = await _dbApp.follow
            .Where(f => f.FollowerUserId == currentUserId)
            .Select(f => f.FollowedUserId)
            .ToListAsync();

            var suggestions = await _dbApp.Users
                .Where(u => u.Id != currentUserId && !followingIds.Contains(u.Id))
                .Join(
                    _dbApp.profiles,
                    user => user.Id,
                    profile => profile.UserId,
                    (user, profile) => new FollowingSuggestionsDTO
                    {
                        UserId = user.Id,
                        Name = user.UserName,
                        AvatarUrl = profile.AvatarUrl
                    }
                )
                .Take(3)
                .ToArrayAsync();

            return suggestions;
        }

        public async Task<List<FollowingSuggestionsDTO>> GetFollowingsSuggestionsByName(string currentUserId, string Username)
        {
            var followingIds = await _dbApp.follow
             .Where(f => f.FollowerUserId == currentUserId)
             .Select(f => f.FollowedUserId)
             .ToListAsync();

            var suggestions = await _dbApp.Users
                .Where(u => u.Id != currentUserId &&
                            !followingIds.Contains(u.Id) &&
                            u.UserName.Contains(Username))
                        .Join(
                    _dbApp.profiles,
                    user => user.Id,
                    profile => profile.UserId,
                    (user, profile) => new FollowingSuggestionsDTO
                    {
                        UserId = user.Id,
                        Name = user.UserName,
                        AvatarUrl = profile.AvatarUrl
                    }
                )
                .ToListAsync();

            return suggestions;
        }


        public async Task<string> GetImageURL(string UserId)
        {
            string imageURL = await _dbApp.profiles.Where(p => p.UserId == UserId).Select(p => p.AvatarUrl).FirstOrDefaultAsync();

            return string.IsNullOrEmpty(imageURL) ? "" : imageURL;
        }

    }
}
