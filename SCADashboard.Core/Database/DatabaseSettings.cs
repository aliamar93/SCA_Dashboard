using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.Database
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public DatabaseProvider Provider { get; set; } = DatabaseProvider.SqlServer;
        public int CommandTimeout { get; set; } = 30;
    }
}
