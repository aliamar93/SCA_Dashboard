using SCADashboard.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.Database
{
    public class ConnectionStringService
    {
        private readonly IDatabaseConfiguration _databaseConfig;

        public ConnectionStringService(IDatabaseConfiguration databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public void UpdateConnectionString(string databaseName, string newConnectionString, DatabaseProvider provider = DatabaseProvider.SqlServer)
        {
            _databaseConfig.UpdateDatabaseSettings(databaseName, new DatabaseSettings
            {
                ConnectionString = newConnectionString,
                Provider = provider
            });
        }
    }
}



#region Program.cs DI Registration Example

//builder.Services.AddSingleton<ConnectionStringService>();


//// Register DatabaseConfiguration
//builder.Services.AddSingleton<IDatabaseConfiguration, DatabaseConfiguration>();

//// Register ApplicationDbContext
//builder.Services.AddScoped<ApplicationDbContext>();

//// Register Generic Factory
//builder.Services.AddScoped(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));

//// Optional: Direct DI with DbContextOptions
//builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
//{
//    var dbConfig = sp.GetRequiredService<IDatabaseConfiguration>();
//    var settings = dbConfig.GetDatabaseSettings();
//    switch (settings.Provider)
//    {
//        case DatabaseProvider.SqlServer:
//            options.UseSqlServer(settings.ConnectionString);
//            break;
//        case DatabaseProvider.PostgreSQL:
//            options.UseNpgsql(settings.ConnectionString);
//            break;
//            // add other providers if needed
//    }
//});
#endregion