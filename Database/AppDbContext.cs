using KiwiTaskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KiwiTaskAPI.Database
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Tasks> tasks { get; set; }
        public DbSet<TaskCategory> task_categories { get; set; }
        public DbSet<TaskAttachment> task_attachments { get; set; }
        public DbSet<Users> users { get; set; }
        public DbSet<UserPassword> user_password { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasOne(u => u.user_password).WithOne(up => up.user).HasForeignKey<UserPassword>(up => up.user_id);
        }
    }
}
