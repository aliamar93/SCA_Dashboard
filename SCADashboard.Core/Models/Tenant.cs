using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class Tenant
{
    public int TenantId { get; set; }

    public string TenantName { get; set; } = null!;

    public string? Domain { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactPhone { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();

    public virtual ICollection<Page> Pages { get; set; } = new List<Page>();

    public virtual ICollection<RolePagePermission> RolePagePermissions { get; set; } = new List<RolePagePermission>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
