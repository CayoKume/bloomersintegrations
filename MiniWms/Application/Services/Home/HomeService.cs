using BloomersMiniWmsIntegrations.Infrastructure.Repositorys;
using Newtonsoft.Json;

namespace BloomersMiniWmsIntegrations.Application.Services
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _homeRepository;

        public HomeService(IHomeRepository homeRepository) =>
            (_homeRepository) = (homeRepository);

        public async Task<string> GetPickupOrders(string doc_company)       
        {
            var list = await _homeRepository.GetPickupOrders(doc_company);
            return JsonConvert.SerializeObject(list);
        }
    }
}
