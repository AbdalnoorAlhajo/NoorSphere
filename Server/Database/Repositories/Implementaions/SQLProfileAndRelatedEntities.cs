using Database.Models.Domain;
using Database.Models.DTOs.ProfileAndRelatedEntities.Profile;
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

        public async Task<List<GetProfilesWithName>> GetAllProfiles()
        {
            var profiles = _dbApp.profiles
                .Join(
                _dbApp.Users,
                profile => profile.UserId,
                user => user.Id,
                (profile, user) => new GetProfilesWithName
                {
                    ProfileId = profile.Id,
                    Name = user.UserName,
                }
                );

            return await profiles.ToListAsync();
        }

        public async Task<Profile> AddProfile(Profile newProfile)
        {
            _dbApp.profiles.Add(newProfile);
            await _dbApp.SaveChangesAsync();
            return newProfile;
        }
        public async Task<Profile> UpdateProfile(Profile UpdatedProfile)
        {
            var profile = _dbApp.profiles.FindAsync(UpdatedProfile.Id).Result;

            if (profile != null)
            {
                profile.Website = UpdatedProfile.Website;
                profile.Skills = UpdatedProfile.Skills;
                profile.Status = UpdatedProfile.Status;
                profile.Bio = UpdatedProfile.Bio;
                profile.Country = UpdatedProfile.Country;
                profile.Company = UpdatedProfile.Company;
            }

            _dbApp.profiles.Update(UpdatedProfile);
            await _dbApp.SaveChangesAsync();
            return UpdatedProfile;
        }
        public async Task<Profile?> GetProfile(int id)
        {
            return await _dbApp.profiles.Include(p => p.Educations).Include(p => p.Experiences).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Profile?> GetProfileByUserId(string UserId)
        {
            return await _dbApp.profiles.Include(p => p.Educations).Include(p => p.Experiences).FirstOrDefaultAsync(p => p.UserId == UserId);
        }
    }
}
