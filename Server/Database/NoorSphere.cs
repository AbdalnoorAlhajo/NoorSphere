using Database.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class NoorSphere : DbContext
    {
       public NoorSphere(DbContextOptions<NoorSphere> options) : base(options) { }


        // Users
        public DbSet<User> users { get; set; }

        // Profiles and related entities
        public DbSet<Profile> profiles { get; set; }

        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Education> education { get; set; }
        public DbSet<SocialLinks> socialLinks { get; set; }

        // Posts and related entities
        public DbSet<Post> posts { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Like> likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Create a unique index for the Email field
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}