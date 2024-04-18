using BloomersGeneralIntegrations.Pagarme.Domain.Entities;
using System.Text.Json;

namespace BloomersGeneralIntegrations.Pagarme.Infrastructure.Apis
{
    public class APICall : IAPICall
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APICall(IHttpClientFactory httpClientFactory) =>
            (_httpClientFactory) = (httpClientFactory);

        public async Task<Root?> GetAsync(string dataInicio, string dataFinal)
        {
            try
            {
                //get token
                var client = CreateClient("c2tfOWVhOTI1ZmQ4NmRkNDUzMWJhZThmYWU4MDBiODU2MWU6");
                var response = await client.GetAsync(client + $"{dataInicio}T00:00:00Z&created_until={dataFinal}T23:59:59Z&size=1000");

                return JsonSerializer.Deserialize<Root>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private HttpClient CreateClient(string token)
        {
            var client = _httpClientFactory.CreateClient("PagarmeAPI");
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("authorization", "Basic" + token);

            return client;
        }
    }
}
