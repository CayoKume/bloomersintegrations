namespace BloomersGeneralIntegrations.AfterSale.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<string> GetReversesAsync(string token);
        public Task<string> GetReversesByPageAsync(string token, int page);
    }
}
