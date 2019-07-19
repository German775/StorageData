using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        }

        public DbSet<Data> Datas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConfiguration().DatabaseConnectionString);
        }
    }
}
