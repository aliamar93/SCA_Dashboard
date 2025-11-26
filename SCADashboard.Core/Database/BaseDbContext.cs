using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.Database
{
    public abstract class BaseDbContext : DbContext
    {
        private readonly IDatabaseConfiguration _databaseConfig;

        protected BaseDbContext(IDatabaseConfiguration databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        protected BaseDbContext(IDatabaseConfiguration databaseConfig, DbContextOptions options)
            : base(options)
        {
            _databaseConfig = databaseConfig;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _databaseConfig.GetDatabaseSettings().ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
