namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<string> PostRequest(object objRequest, string? endpoint, string authentication, string chave);
        public HttpClient CreateClient(string authentication, string chave, string route);
    }
}
