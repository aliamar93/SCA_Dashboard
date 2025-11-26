using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.Models
{
    public partial class User
    {
        // Extra field only for binding in the UI
        [NotMapped]   // EF Core will ignore this field
        public int RoleId { get; set; }

        // Extra field only for binding in the UI
        [NotMapped]   // EF Core will ignore this field
        public List<int> KundeId { get; set; }
    }
}
