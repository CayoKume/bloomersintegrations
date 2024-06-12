using NewBloomersWebApplication.Domain.Entities.Picking;
using NewBloomersWebApplication.Infrastructure.Apis;
using Newtonsoft.Json;

namespace NewBloomersWebApplication.Application.Services
{
    public class HomeService : IHomeService
    {
        private readonly IAPICall _apiCall;

        public HomeService(IAPICall apiCall) =>
            (_apiCall) = (apiCall);
    }
}
