using GDP_API.Models;
using Microsoft.EntityFrameworkCore;

namespace GDP_API.Data
{
    public class DataContext : DbContext
    { 
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {

        
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ExpertUser> ExpertUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Activity> Activities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<ExpertUser>()
            .HasOne(eu => eu.User)
            .WithOne()
            .HasForeignKey<ExpertUser>(eu => eu.UserId);
            
             modelBuilder.Entity<UserHasProject>()
            .HasKey(uhp => new { uhp.UserId, uhp.ProjectId });

            modelBuilder.Entity<UserHasProject>()
            .HasOne(uhp => uhp.User)
            .WithMany(u => u.UserHasProjects)
            .HasForeignKey(uhp => uhp.UserId);

            modelBuilder.Entity<UserHasProject>()
            .HasOne(uhp => uhp.Project)
            .WithMany(p => p.UserHasProjects)
            .HasForeignKey(uhp => uhp.ProjectId);

            modelBuilder.Entity<UserHasActivity>()
            .HasKey(uha => new { uha.UserId, uha.ActivityId });

            modelBuilder.Entity<UserHasActivity>()
            .HasOne(uha => uha.User)
            .WithMany(u => u.UserHasActivities)
            .HasForeignKey(uha => uha.UserId);

            modelBuilder.Entity<UserHasActivity>()
            .HasOne(uha => uha.Activity)
            .WithMany(a => a.UserHasActivities)
            .HasForeignKey(uha => uha.ActivityId);

            modelBuilder.Entity<Activity>()
            .HasOne(a => a.Project)
            .WithMany(p => p.Activities)
            .HasForeignKey(a => a.ProjectId);

        }
    }
}
