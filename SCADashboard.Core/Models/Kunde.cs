using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class Kunde
{
    public int KundeId { get; set; }

    public int TenantId { get; set; }

    public string KundeNo { get; set; } = null!;

    public string KundeName { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual ICollection<Userkunde> Userkundes { get; set; } = new List<Userkunde>();
}
