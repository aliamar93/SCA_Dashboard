using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class AuditLog
{
    public int AuditId { get; set; }

    public int TenantId { get; set; }

    public int UserId { get; set; }

    public string? Description { get; set; }

    public string? Action { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? Ipaddress { get; set; }

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
