using Newtonsoft.Json.Linq;
using BloomersCarriersIntegrations.Jadlog.Domain.Entities;

namespace BloomersCarriersIntegrations.Jadlog.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<string> GetAsync(string senderID, string orderNumber, string doc_company, string rote, JObject jArrayObj);
        public Task<string> PostAsync(string orderNumber, string rote, string token, JObject jArrayObj);
    }
}
