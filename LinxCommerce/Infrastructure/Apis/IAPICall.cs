namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<string> PostRequest(object objRequest, string? endpoint, string authentication, string chave);
    }
}
