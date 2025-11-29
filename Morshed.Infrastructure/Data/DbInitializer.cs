using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Morshed.Core.Entities;
using Morshed.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Morshed.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // Seed Roles
            if (!await roleManager.RoleExistsAsync(Core.Constants.Roles.Admin))
                await roleManager.CreateAsync(new IdentityRole(Core.Constants.Roles.Admin));
            if (!await roleManager.RoleExistsAsync(Core.Constants.Roles.User))
                await roleManager.CreateAsync(new IdentityRole(Core.Constants.Roles.User));

            // Seed Default Admin
            var adminUser = await userManager.FindByEmailAsync("admin@morshed.com");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@morshed.com",
                    Email = "admin@morshed.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Password123!");
                await userManager.AddToRoleAsync(adminUser, Core.Constants.Roles.Admin);
            }

            // Seed Provinces
            if (!context.Provinces.Any())
            {
                var provinces = new List<Province>
                {
                    new Province
                    {
                        NameAr = "الأقصر",
                        NameEn = "Luxor",
                        Description = "The world's greatest open-air museum.",
                        ThumbnailUrl = "/images/luxor_thumb.jpg"
                    },
                    new Province
                    {
                        NameAr = "شرم الشيخ",
                        NameEn = "Sharm El-Sheikh",
                        Description = "City of Peace, famous for diving and beaches.",
                        ThumbnailUrl = "/images/sharm_thumb.jpg"
                    }
                };
                await context.Provinces.AddRangeAsync(provinces);
                await context.SaveChangesAsync();
            }

            // Seed Places for Luxor
            if (!context.Places.Any())
            {
                var luxor = await context.Provinces.FirstOrDefaultAsync(p => p.NameEn == "Luxor");
                var sharm = await context.Provinces.FirstOrDefaultAsync(p => p.NameEn == "Sharm El-Sheikh");

                var places = new List<Place>
                {
                    new Place
                    {
                        ProvinceId = luxor.Id,
                        Name = "Karnak Temple",
                        Category = "Temple",
                        Address = "Karnak, Luxor",
                        Description = "A vast mix of decayed temples, chapels, pylons, and other buildings.",
                        Lat = 25.7188,
                        Lng = 32.6573,
                        Images = new List<PlaceImage> { new PlaceImage { Url = "/images/karnak.jpg" } }
                    },
                    new Place
                    {
                        ProvinceId = luxor.Id,
                        Name = "Valley of the Kings",
                        Category = "Tomb",
                        Address = "Luxor",
                        Description = "Tombs of the Pharaohs of the New Kingdom.",
                        Lat = 25.7402,
                        Lng = 32.6014,
                        Images = new List<PlaceImage> { new PlaceImage { Url = "/images/valley_kings.jpg" } }
                    },
                    new Place
                    {
                        ProvinceId = sharm.Id,
                        Name = "Naama Bay",
                        Category = "Beach",
                        Address = "Sharm El-Sheikh",
                        Description = "A natural bay and the main hub for tourists.",
                        Lat = 27.9158,
                        Lng = 34.3299,
                        Images = new List<PlaceImage> { new PlaceImage { Url = "/images/naama.jpg" } }
                    }
                };
                await context.Places.AddRangeAsync(places);
                await context.SaveChangesAsync();
            }
        }
    }
}
