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
        public DbSet<TaskCates> task_cates { get; set; }
        public DbSet<PreferredCategories> preferred_categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasOne(u => u.user_password).WithOne(up => up.user).HasForeignKey<UserPassword>(up => up.user_id);
            modelBuilder.Entity<TaskCates>().HasOne(x => x.task).WithMany(x => x.categories).HasForeignKey(x => x.task_id);
            modelBuilder.Entity<Tasks>().HasOne(x => x.poster).WithMany(x => x.tasks).HasForeignKey(x => x.poster_id);
            modelBuilder.Entity<PreferredCategories>().HasOne(x => x.user).WithMany(x => x.preferred_categories).HasForeignKey(x => x.user_id);
        }
    }
}
