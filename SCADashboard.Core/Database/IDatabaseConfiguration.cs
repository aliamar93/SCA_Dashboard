using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.Database
{
    public interface IDatabaseConfiguration
    {
        DatabaseSettings GetDatabaseSettings(string name = "Default");
        void UpdateDatabaseSettings(string name, DatabaseSettings settings);
        DbContextOptionsBuilder ConfigureDbContext(Type dbContextType, string databaseName = "Default");
    }
}
