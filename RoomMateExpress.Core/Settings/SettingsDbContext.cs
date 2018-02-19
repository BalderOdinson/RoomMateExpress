using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace RoomMateExpress.Core.Settings
{
    public class SettingsDbContext : DbContext
    {
        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "settings.db3");
            optionsBuilder.UseSqlite($"Data Source={path}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserData>().HasKey(k => k.UserId);
            modelBuilder.Entity<UserData>().HasMany(h => h.SearchHistories).WithOne(u => u.User);

            modelBuilder.Entity<SearchHistory>().HasKey(k => k.Id);
        }
    }
}
