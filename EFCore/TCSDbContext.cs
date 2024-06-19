using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TCS.Entity;

namespace TCS.EFCore
{
    public class TCSDbContext:DbContext
    {
        public DbSet<TCSFileInfo> FileInfos { get; set; }

        public DbSet<RecordInfo> RecordInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + DatabaseInitializer.databasePath);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TCSFileInfo>(b => {
                b.HasKey(p => p.ID);
                b.HasIndex(p => p.FileId).IsUnique();
                b.HasIndex(p => p.FileName);
                b.Property(p => p.ID).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<RecordInfo>(b => {
                b.HasKey(p => p.ID);
                b.HasIndex(p => p.FileId);
                b.Property(p => p.ID).ValueGeneratedOnAdd();
            });
        }
    }
}
