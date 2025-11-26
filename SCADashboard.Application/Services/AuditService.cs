using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Application.Services
{
    public class AuditService:BaseService<AuditLog>,IAuditService
    {
        private readonly IAuditRepository _auditRepository;
        public AuditService(IAuditRepository auditRepository)
            : base(auditRepository)
        {
            _auditRepository = auditRepository;
        }
    }
}
