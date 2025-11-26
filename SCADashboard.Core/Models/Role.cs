using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public int TenantId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<RolePagePermission> RolePagePermissions { get; set; } = new List<RolePagePermission>();

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
