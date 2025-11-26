using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;

namespace SCADashboard.Application.Services
{
    public class KundeService:BaseService<Kunde>, IKundeService
    {
        private readonly IKundeRepository _kundeRepository;
        public KundeService(IKundeRepository kundeRepository) : base(kundeRepository)
        {
            _kundeRepository = kundeRepository;
        }
    }
}
