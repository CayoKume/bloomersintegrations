using System.Net;
using System.Text;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Apis
{
    public class APICall : IAPICall
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APICall(IHttpClientFactory httpClientFactory) =>
            (_httpClientFactory) = (httpClientFactory);

        public async Task<string?> PostRequest(object jObject, string? route, string authentication, string chave)
        {
            try
            {
                var client = CreateClient(
                    authentication,
                    chave,
                    route
                );

                var response = await client.PostAsync(client.BaseAddress + route, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jObject), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                    return await response.Content.ReadAsStringAsync();
                else
                    throw new Exception($"{response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception($"{route} - PostRequest - Erro ao consultar end-point {route} na microvix - {ex}");
            }
        }

        public HttpClient CreateClient(string authentication, string chave, string route)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{authentication}:{chave}");
                var client = _httpClientFactory.CreateClient("LinxCommerceAPI");
                client.DefaultRequestHeaders.Add("ContentType", "application/json");
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + System.Convert.ToBase64String(plainTextBytes));

                return client;
            }
            catch (Exception ex)
            {
                throw new Exception($"{route} - CreateClient - Erro ao criar HttpClientRequest para o end-point {route} na microvix - {ex}");
            }
        }
    }
}
