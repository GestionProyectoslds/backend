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
    }
}
