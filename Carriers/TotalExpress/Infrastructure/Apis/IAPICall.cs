using BloomersCarriersIntegrations.TotalExpress.Domain.Entities;

namespace BloomersCarriersIntegrations.TotalExpress.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<Status?> GetAWB(string senderID, string orderNumber);
        public Task<string> PostAWB(Order model);
    }
}
