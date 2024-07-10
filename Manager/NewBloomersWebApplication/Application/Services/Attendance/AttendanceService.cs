using NewBloomersWebApplication.Domain.Entities.Attendance;
using NewBloomersWebApplication.Infrastructure.Apis;
using System.Text.Json;

namespace NewBloomersWebApplication.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAPICall _apiCall;

        public AttendanceService(IAPICall apiCall) =>
            (_apiCall) = (apiCall);

        public async Task<List<Order>> GetOrdersToContact(string serie, string doc_company)
        {
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    { "serie", serie },
                    { "doc_company", doc_company }
                };
                var encodedParameters = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();
                var result = await _apiCall.GetAsync("GetOrdersToContact", encodedParameters);
                return JsonSerializer.Deserialize<List<Order>>(result);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateDateContacted(string number, string atendente, string inputObs)
        {
            try
            {
                return await _apiCall.PutAsync("UpdateDateContacted", System.Text.Json.JsonSerializer.Serialize(new { number = number, atendente = atendente, obs = inputObs }));
            }
            catch
            {
                throw;
            }
        }
    }
}
