using SCADashboard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.Database
{
    public class MultiDbService
    {
        private readonly IDbContextFactory<SCADashboardDbContext> _factory;

        public MultiDbService(IDbContextFactory<SCADashboardDbContext> factory)
        {
            _factory = factory;
        }

        public async Task ProcessDatabases()
        {
            using var primary = _factory.CreateDbContext("Primary");
            using var secondary = _factory.CreateDbContext("Secondary");
            // Work with different databases
        }
    }
}
