using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class Userkunde
{
    public int UserKundeId { get; set; }

    public int? UserId { get; set; }

    public int? KundeId { get; set; }

    public virtual Kunde? Kunde { get; set; }

    public virtual User? User { get; set; }
}
