namespace BloomersGeneralIntegrations.Movidesk.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<string> GetAsync(string filter);
    }
}
