using BloomersCarriersIntegrations.FlashCourier.Domain.Entities;

namespace BloomersCarriersIntegrations.FlashCourier.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<HAWBResponse?> GetHAWB(string[] numEncCli, string doc_company);
        public Task<List<InsertHAWBSuccessResponse>> PostHAWB(Order model);
    }
}
