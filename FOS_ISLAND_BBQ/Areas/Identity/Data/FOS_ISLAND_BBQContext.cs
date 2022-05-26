using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FOS_ISLAND_BBQ.Areas.Identity.Data;
using FOS_ISLAND_BBQ.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FOS_ISLAND_BBQ.Data
{
    public class FOS_ISLAND_BBQContext : IdentityDbContext<FOS_ISLAND_BBQUser>
    {
        public FOS_ISLAND_BBQContext(DbContextOptions<FOS_ISLAND_BBQContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
