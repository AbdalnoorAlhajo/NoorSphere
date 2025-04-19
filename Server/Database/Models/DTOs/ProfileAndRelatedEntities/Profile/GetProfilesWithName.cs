using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.ProfileAndRelatedEntities.Profile
{
    public class GetProfilesWithName
    {
        public int ProfileId { get; set; }

        public string Name { get; set; }
    }
}
