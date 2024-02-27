using GDP_API.Models;
using Microsoft.EntityFrameworkCore;

namespace GDP_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {


        }
        public DbSet<User> Users { get; set; }
        public DbSet<ExpertUser> ExpertUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ProjectCategory> ProjectCategories { get; set; }
        public DbSet<ProjectHasCategory> ProjectHasCategories { get; set; }
        public DbSet<UserHasProject> UserHasProjects { get; set; }
        public DbSet<UserHasActivity> UserHasActivities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User has a unqiue index on email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            // ExpertUser has a foreign key to User one to one relationship
            modelBuilder.Entity<ExpertUser>()
            .HasOne(eu => eu.User)
            .WithOne()
            .HasForeignKey<ExpertUser>(eu => eu.UserId);
            // Project's budget and cost are decimal(18, 2)
            modelBuilder.Entity<Project>()
            .Property(p => p.Budget)
            .HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Project>()
            .Property(p => p.Cost)
            .HasColumnType("decimal(18, 2)");
            // UserHasProject has a composite key of UserId and ProjectId
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
            // UserHasActivity has a composite key of UserId and ActivityId
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

            // Activity has a foreign key to Project
            modelBuilder.Entity<Activity>()
            .HasOne(a => a.Project)
            .WithMany(p => p.Activities)
            .HasForeignKey(a => a.ProjectId);
            // ProjectCategory has a unique index on name
            modelBuilder.Entity<ProjectCategory>()
            .HasIndex(pc => pc.Name)
            .IsUnique();
            // Category and Project have a many-to-many relationship 
            modelBuilder.Entity<ProjectHasCategory>()
            .HasKey(phc => new { phc.ProjectId, phc.CategoryId });
            modelBuilder.Entity<ProjectHasCategory>()
                .HasOne(phc => phc.Project)
                .WithMany(p => p.ProjectHasCategories)
                .HasForeignKey(phc => phc.ProjectId);

            modelBuilder.Entity<ProjectHasCategory>()
                .HasOne(phc => phc.Category)
                .WithMany(pc => pc.ProjectHasCategories)
                .HasForeignKey(phc => phc.CategoryId);
        }
    }
}
