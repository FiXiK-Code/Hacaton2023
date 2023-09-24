using Microsoft.EntityFrameworkCore;
using MVP.Date.Models;

namespace MVP.Date
{
    public class AppDB : DbContext
    {
        public AppDB(DbContextOptions<AppDB> options) : base(options) { }
        
        public DbSet<Task> DBTask { get; set; }
        public DbSet<Material> DBMaterial { get; set; }
        public DbSet<Project> DBProject { get; set; }
        public DbSet<User> DBUser { get; set; }

    }
}
