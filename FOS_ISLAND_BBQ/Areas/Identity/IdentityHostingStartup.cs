using System;
using FOS_ISLAND_BBQ.Areas.Identity.Data;
using FOS_ISLAND_BBQ.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FOS_ISLAND_BBQ.Areas.Identity.IdentityHostingStartup))]
namespace FOS_ISLAND_BBQ.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<FOS_ISLAND_BBQContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("FOS_ISLAND_BBQContextConnection")));

                
                services.AddIdentity<FOS_ISLAND_BBQUser, IdentityRole>()
        .AddRoleManager<RoleManager<IdentityRole>>()
        .AddDefaultUI()
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<FOS_ISLAND_BBQContext>();
            });
        }
    }
}