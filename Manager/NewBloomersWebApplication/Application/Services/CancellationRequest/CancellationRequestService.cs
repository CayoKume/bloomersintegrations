using NewBloomersWebApplication.Domain.Entities.CancellationRequest;
using NewBloomersWebApplication.Infrastructure.Apis;
using System.Text.Json;

namespace NewBloomersWebApplication.Application.Services
{
    public class CancellationRequestService : ICancellationRequestService
    {
        private readonly IAPICall _apiCall;

        public CancellationRequestService(IAPICall apiCall) =>
            (_apiCall) = (apiCall);

        public async Task CreateCancellationRequest(Order order)
        {
            try
            {
                await _apiCall.PostAsync("CreateCancellationRequest", JsonSerializer.Serialize(new { serializePedido = order }));
            }
            catch
            {
                throw;
            }
        }

        public async Task<Dictionary<int, string>> GetReasons()
        {
            try
            {
                var result = await _apiCall.GetAsync("GetReasons");
                return JsonSerializer.Deserialize<Dictionary<int, string>>(result);
            }
            catch
            {
                throw;
            }
        }
    }
}
