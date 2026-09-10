using Microsoft.EntityFrameworkCore;
using TravelConnect.Models;

namespace TravelConnect.Data
{
    public class TravelConnectDbContext : DbContext
    {
        public TravelConnectDbContext(
            DbContextOptions<TravelConnectDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostLike> PostLikes { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<Itinerary> Itineraries { get; set; }

        public DbSet<ItineraryItem> ItineraryItems { get; set; }
    }
}