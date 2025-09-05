using Database.Models.Domain;
using Database.Models.DTOs.UserAndRelatedEntities.User;
using Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Database.Repositories.Implementaions
{
    public class GuestCacheImplementation : IGuestCache
    {
        private readonly UserManager<User> _userManager;
        private readonly IMemoryCache _cache;
        private readonly IProfileAndRelatedEntities _profileAndRelatedEntities;

        public GuestCacheImplementation(IMemoryCache cache, UserManager<User> userManager, IProfileAndRelatedEntities profileAndRelatedEntities)
        {
            _cache = cache;
            _userManager = userManager;
            _profileAndRelatedEntities = profileAndRelatedEntities;
        }

        public async Task<User> GetGuestCatchedInfo(string Email)
        {
            const string cacheKey = "guest-user-info";

            if (_cache.TryGetValue(cacheKey, out User cachedUser))
            {
                return cachedUser;
            }

            var user = await _userManager.FindByEmailAsync(Email);

            _cache.Set(cacheKey, user, TimeSpan.FromDays(95));

            return user;
        }

        public async Task<string> GetImageURL(string userId)
        {
            const string cacheKey = "guest-user-avatar";

            if (_cache.TryGetValue(cacheKey, out string cachedUrl))
            {
                return cachedUrl;
            }


            var imageUrl = await _profileAndRelatedEntities.GetImageURL(userId);

            _cache.Set(cacheKey, imageUrl, TimeSpan.FromDays(95));


            return imageUrl;
        }
    }
}
