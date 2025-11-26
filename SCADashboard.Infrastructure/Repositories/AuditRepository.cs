using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Infrastructure.Repositories
{
    public class AuditRepository:BaseRepository<AuditLog>,IAuditRepository
    {
        public AuditRepository(SCADashboardDbContext context):base(context)
        { }
    }
}
