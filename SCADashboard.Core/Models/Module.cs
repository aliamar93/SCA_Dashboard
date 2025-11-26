using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class Module
{
    public int ModuleId { get; set; }

    public int? TenantId { get; set; }

    public string ModuleName { get; set; } = null!;

    public string? Icon { get; set; }

    public int? OrderIndex { get; set; }

    public bool IsActive { get; set; }

    public int? IsChild { get; set; }

    public virtual ICollection<Page> Pages { get; set; } = new List<Page>();

    public virtual Tenant? Tenant { get; set; }
}
