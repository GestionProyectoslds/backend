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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<ExpertUser>()
            .HasOne(eu => eu.User)
            .WithOne()
            .HasForeignKey<ExpertUser>(eu => eu.UserId);
        }
    }
}
