using BloomersGeneralIntegrations.Mobsim.Domain.Entities;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace BloomersGeneralIntegrations.Mobsim.Infrastructure.Apis
{
    public class APICall : IAPICall
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APICall(IHttpClientFactory httpClientFactory) =>
            (_httpClientFactory) = (httpClientFactory);

        public async Task PostAsync(MobsimObject body)
        {
            try
            {
                //repository get token
                var client = CreateClient("GIyHSgdr0qZhWtuSjJc6PSgSXGhuhFdN");
                var response = await client.PostAsync(client + "/api/v2/doodoc/pagtrib/upload/import", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));

                if (response.StatusCode != HttpStatusCode.OK)
                {

                }
            }
            catch (Exception ex) when (ex.Message.Contains("CreateClient"))
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(@$"AfterSale - GetAsync - Erro ao obter reversas a partir do end-point: https://api.send4.com.br/v3/api/reverses? - {ex.Message}");
            }
        }

        private HttpClient CreateClient(string token) 
        {
            var client = _httpClientFactory.CreateClient("MobsimAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Basic " + token);

            return client;
        }
    }
}
