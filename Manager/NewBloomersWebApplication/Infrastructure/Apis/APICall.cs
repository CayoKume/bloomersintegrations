
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace NewBloomersWebApplication.Infrastructure.Apis
{
    public class APICall : IAPICall
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APICall(IHttpClientFactory httpClientFactory) =>
            (_httpClientFactory) = (httpClientFactory);

        public async Task<string> GetAsync(string route, string encodedParameters)
        {
            try
            {
                var client = CreateClient(route);
                var result = await client.GetAsync($"{client.BaseAddress}{route}?{encodedParameters}");
                return await result.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetAsync(string route)
        {
            try
            {
                var client = CreateClient(route);
                var result = await client.GetAsync($"{client.BaseAddress}{route}");
                return await result.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> PostAsync(string route, string jsonContent)
        {
            try
            {
                var client = CreateClient(route);
                var result = await client.PostAsync($"{client.BaseAddress}{route}", new StringContent(jsonContent, Encoding.UTF8, "application/json"));
                return await result.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private HttpClient CreateClient(string route)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MiniWMS");

                return client;
            }
            catch (Exception ex)
            {
                throw new Exception($"{route} - CreateClient - Erro ao criar HttpClientRequest para o end-point {route} - {ex}");
            }
        }
    }
}
