using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FOS_ISLAND_BBQ.Models;

namespace FOS_ISLAND_BBQ.Data
{
    public class FOS_ISLAND_BBQMenuContext : DbContext
    {
        public FOS_ISLAND_BBQMenuContext (DbContextOptions<FOS_ISLAND_BBQMenuContext> options)
            : base(options)
        {
        }

        public DbSet<FOS_ISLAND_BBQ.Models.MenuModel> MenuModel { get; set; }

        public DbSet<FOS_ISLAND_BBQ.Models.UploadAdveristment> UploadAdveristment { get; set; }

        public DbSet<FOS_ISLAND_BBQ.Models.UploadBills> UploadBills { get; set; }
    }
}
