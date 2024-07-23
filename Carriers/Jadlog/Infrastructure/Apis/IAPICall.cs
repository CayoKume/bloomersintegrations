using Newtonsoft.Json.Linq;
using BloomersCarriersIntegrations.Jadlog.Domain.Entities;

namespace BloomersCarriersIntegrations.Jadlog.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<string> GetAsync(string senderID, string orderNumber, string token, string rote, JObject jObj);
        public Task<string> PostAsync(string orderNumber, string rote, string token, JObject jObj);
    }
}
