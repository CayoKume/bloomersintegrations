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

        public async Task<bool> CreateCancellationRequest(Order order)
        {
            try
            {
                await _apiCall.PostAsync("CreateCancellationRequest", JsonSerializer.Serialize(new { serializeOrder = order }));
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Order> GetOrderToCancel(string number)
        {
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    { "number", number }
                };
                var encodedParameters = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();
                var result = await _apiCall.GetAsync("GetOrderToCancel", encodedParameters);
                return JsonSerializer.Deserialize<Order>(result);
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
