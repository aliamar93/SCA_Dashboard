using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;


namespace SCADashboard.Infrastructure.Repositories
{
    public class PasswordLogRepository : BaseRepository<PasswordLog>, IPasswordLogRepository
    {
        public PasswordLogRepository(SCADashboardDbContext context) : base(context)
        { }
    }
}
