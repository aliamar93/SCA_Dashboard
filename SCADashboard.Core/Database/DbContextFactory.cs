using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.Database
{
    public interface IDbContextFactory<TContext> where TContext : DbContext
    {
        TContext CreateDbContext(string databaseName = "Default");
    }

    public class DbContextFactory<TContext> : IDbContextFactory<TContext> where TContext : DbContext
    {
        private readonly IDatabaseConfiguration _databaseConfig;
        private readonly IServiceProvider _serviceProvider;

        public DbContextFactory(IDatabaseConfiguration databaseConfig, IServiceProvider serviceProvider)
        {
            _databaseConfig = databaseConfig;
            _serviceProvider = serviceProvider;
        }

        public TContext CreateDbContext(string databaseName = "Default")
        {
            var optionsBuilder = _databaseConfig.ConfigureDbContext(typeof(TContext), databaseName);
            return (TContext)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(TContext), optionsBuilder.Options);
        }
    }
}
