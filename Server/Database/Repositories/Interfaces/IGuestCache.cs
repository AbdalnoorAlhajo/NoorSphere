using Database.Models.Domain;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories.Interfaces
{
    public interface IGuestCache
    {
        Task<User> GetGuestCatchedInfo(string Email);

        Task<string> GetImageURL(string UserId);
    }
}
