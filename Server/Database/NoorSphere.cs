using Database.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Database
{
    public class NoorSphere : IdentityDbContext<User>
    {
        public NoorSphere(DbContextOptions<NoorSphere> options) : base(options) { }
        public NoorSphere() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Server"))
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

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

            modelBuilder.Entity<Profile>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Profile>(p => p.UserId);

            // One User has many posts 
            modelBuilder.Entity<Post>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserId);

            // One User has many likes 
            modelBuilder.Entity<Like>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(l => l.UserId);

            // One Post has many likes
            modelBuilder.Entity<Like>()
                .HasOne<Post>()
                .WithMany(p => p.likes)
                .HasForeignKey(l => l.PostId);

            // One Post has many Comments 
            modelBuilder.Entity<Comment>()
                .HasOne<Post>()
                .WithMany(p => p.comments)
                .HasForeignKey(c => c.PostId);

            // One User has many Comments 
            modelBuilder.Entity<Comment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId);

            // Unique constraint for UserId and PostId in Likes
            modelBuilder.Entity<Like>()
                .HasIndex(Like => new { Like.UserId, Like.PostId })
                .IsUnique();
        }
    }
}