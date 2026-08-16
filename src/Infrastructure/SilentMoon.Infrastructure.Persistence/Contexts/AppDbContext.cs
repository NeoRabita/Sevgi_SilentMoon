using Microsoft.EntityFrameworkCore;
using SilentMoon.Domain.Entities;

namespace SilentMoon.Infrastructure.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<RefreshToken>RefreshTokens { get; set; }
        public DbSet<Topic>Topics { get; set; }
        public DbSet<UserTopic>UserTopics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Uncomment for read configurations:
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Topic>().HasData(
                new Topic { Id = 1, Slug = "Sleep",Title= "Better Sleep" , IconKey = "betterSleep.png" },
                new Topic { Id = 2, Slug = "Stress", Title = "Reduce Stressed", IconKey = "reduceStress.png" },
                new Topic { Id = 3, Slug = "Anxiety", Title = "Reduce Anxiety", IconKey = "reduceAnxiety.png" },
                new Topic { Id = 4, Slug = "Happiness", Title = "Increase Happiness" , IconKey = "increaseHappiness.png"},
                new Topic { Id = 5, Slug = "Performance", Title = "Improve Performance", IconKey = "improvePerformance.png" },
                new Topic { Id = 6, Slug = "Growth", Title = "Personal Growth", IconKey = "personalGrowth.png" }
            );
        }
    }
}
