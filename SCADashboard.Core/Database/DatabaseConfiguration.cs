using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.Database
{
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        private readonly IConfiguration _configuration;
        private readonly Dictionary<string, DatabaseSettings> _databases;

        public DatabaseConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            _databases = new Dictionary<string, DatabaseSettings>();
            InitializeDatabaseSettings();
        }

        private void InitializeDatabaseSettings()
        {
            var databasesSection = _configuration.GetSection("Databases");
            foreach (var db in databasesSection.GetChildren())
            {
                _databases[db.Key] = new DatabaseSettings
                {
                    ConnectionString = db["ConnectionString"] ?? "",
                    Provider = Enum.Parse<DatabaseProvider>(db["Provider"] ?? "SqlServer"),
                    CommandTimeout = int.Parse(db["CommandTimeout"] ?? "30")
                };
            }
        }

        public DatabaseSettings GetDatabaseSettings(string name = "Default")
        {
            if (_databases.TryGetValue(name, out var settings))
                return settings;

            throw new ArgumentException($"Database settings '{name}' not found.");
        }

        public void UpdateDatabaseSettings(string name, DatabaseSettings settings)
        {
            _databases[name] = settings;
        }

        public DbContextOptionsBuilder ConfigureDbContext(Type dbContextType, string databaseName = "Default")
        {
            var settings = GetDatabaseSettings(databaseName);
            var optionsBuilder = new DbContextOptionsBuilder();

            switch (settings.Provider)
            {
                case DatabaseProvider.SqlServer:
                    optionsBuilder.UseSqlServer(settings.ConnectionString,
                        opt => opt.CommandTimeout(settings.CommandTimeout));
                    break;
                //case DatabaseProvider.PostgreSQL:
                //    optionsBuilder.UseNpgsql(settings.ConnectionString,
                //        opt => opt.CommandTimeout(settings.CommandTimeout));
                //    break;
                //case DatabaseProvider.MySql:
                //    optionsBuilder.UseMySql(settings.ConnectionString,
                //        ServerVersion.AutoDetect(settings.ConnectionString),
                //        opt => opt.CommandTimeout(settings.CommandTimeout));
                //    break;
                //case DatabaseProvider.Sqlite:
                //    optionsBuilder.UseSqlite(settings.ConnectionString);
                //    break;
                default:
                    throw new NotSupportedException($"Database provider '{settings.Provider}' not supported.");
            }

            return optionsBuilder;
        }
    }
}
