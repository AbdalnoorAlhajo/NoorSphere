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

        public async Task<List<Profile>> GetAllProfiles()
        {
            return await _dbApp.profiles.ToListAsync();
        }

        public async Task<Profile> AddProfile(Profile newProfile)
        {
            _dbApp.profiles.Add(newProfile);
            await _dbApp.SaveChangesAsync();
            return newProfile;
        }

        public async Task<Profile?> GetProfile(int id)
        {
            return await _dbApp.profiles.FindAsync(id);
        }
    }
}
