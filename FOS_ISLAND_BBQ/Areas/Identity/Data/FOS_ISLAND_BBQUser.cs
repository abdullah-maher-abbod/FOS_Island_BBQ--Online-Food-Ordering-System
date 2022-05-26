using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FOS_ISLAND_BBQ.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the FOS_ISLAND_BBQUser class
    public class FOS_ISLAND_BBQUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }

        [PersonalData]
        public DateTime DOB { get; set; }

    }
}
