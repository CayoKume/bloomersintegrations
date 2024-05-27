using BloomersCarriersIntegrations.TotalExpress.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace BloomersCarriersIntegrations.TotalExpress.Infrastructure.Apis
{
    public interface IAPICall
    {
        public JArray BuildRegistro(Order order);
        public Task<Status?> GetAWB(string senderID, string orderNumber);
        public Task<string> PostAWB(string orderNumber, Newtonsoft.Json.Linq.JToken jArrayObj);
    }
}
