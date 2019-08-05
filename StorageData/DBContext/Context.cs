using System;
using Microsoft.EntityFrameworkCore;
using StorageData.Model;

namespace StorageData.DBContext
{
    public class Context : DbContext
    {
        public Configuration configuration;

        public Context()
        {
            this.configuration = new Configuration();
            Database.EnsureCreated();
        }

        public DbSet<FrameParameter> FrameParameters { get; set; }
        public DbSet<Frame> Frames { get; set; }
        public DbSet<Parameter> Parameters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConfiguration().DatabaseConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parameter>().HasData(
            new Parameter[]
            {
                new Parameter { Id = Guid.NewGuid(), Name = "Type" },
                new Parameter { Id = Guid.NewGuid(), Name = "CameraId" },
                new Parameter { Id = Guid.NewGuid(), Name = "Coordinate_X" },
                new Parameter { Id = Guid.NewGuid(), Name = "Coordinate_Y" },
                new Parameter { Id = Guid.NewGuid(), Name = "BackgroundId" },
                new Parameter { Id = Guid.NewGuid(), Name = "Width"},
                new Parameter { Id = Guid.NewGuid(), Name = "Height"}
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
