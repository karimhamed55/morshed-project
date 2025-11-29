using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Morshed.Core.Entities;

namespace Morshed.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Province> Provinces { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<PlaceImage> PlaceImages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourStop> TourStops { get; set; }
        public DbSet<TransportOption> TransportOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // TransportOption relationships
            builder.Entity<TransportOption>()
                .HasOne(t => t.PlaceFrom)
                .WithMany()
                .HasForeignKey(t => t.PlaceFromId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TransportOption>()
                .HasOne(t => t.PlaceTo)
                .WithMany()
                .HasForeignKey(t => t.PlaceToId)
                .OnDelete(DeleteBehavior.Restrict);

            // TourStop relationships
            builder.Entity<TourStop>()
                .HasOne(ts => ts.Tour)
                .WithMany(t => t.Stops)
                .HasForeignKey(ts => ts.TourId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cycle

            builder.Entity<TourStop>()
                .HasOne(ts => ts.Place)
                .WithMany()
                .HasForeignKey(ts => ts.PlaceId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cycle
                
            // Review relationships
            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cycle

            builder.Entity<Review>()
                .HasOne(r => r.Place)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PlaceId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cycle

            // Bookmark relationships
            builder.Entity<Bookmark>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Bookmark>()
                .HasOne(b => b.Place)
                .WithMany()
                .HasForeignKey(b => b.PlaceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
