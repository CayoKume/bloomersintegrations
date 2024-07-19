using BloomersCarriersIntegrations.Jadlog.Domain.Entities;
using BloomersCarriersIntegrations.Jadlog.Infrastructure.Repositorys;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net;

namespace BloomersCarriersIntegrations.Jadlog.Infrastructure.Apis
{
    public class APICall : IAPICall
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJadlogRepository _jadlogRepository;

        public APICall(IHttpClientFactory httpClientFactory, IJadlogRepository jadlogRepository) =>
            (_httpClientFactory, _jadlogRepository) = (httpClientFactory, jadlogRepository);

        public async Task<string> GetAsync(string senderID, string orderNumber, string doc_company, string rote, JObject jArrayObj)
        {
            try
            {
                var token = await _jadlogRepository.GetToken(doc_company);
                var client = CreateClientToGetAsync(token);

                var response = await client.PostAsync(client.BaseAddress + rote, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jObject), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    return result;
                }
                else
                    throw new Exception($"{response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Jadlog - GetAsync - Erro ao obter atualizacoes de status do pedido: {orderNumber} - {ex.Message}");
            }
        }

        public async Task<string> PostAsync(string orderNumber, string rote, string token, JObject jArrayObj)
        {
            try
            {
                await _jadlogRepository.GenerateRequestLog(orderNumber, Newtonsoft.Json.JsonConvert.SerializeObject(jArrayObj));

                var client = CreateClientToPostAsync(token);
                var response = await client.PostAsync(client.BaseAddress + rote, new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jArrayObj), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result.ToString();
                }
                else
                    throw new Exception($"{response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Jadlog - PostAsync - Erro ao enviar o pedido: {orderNumber} para jadlog - {ex.Message}");
            }
        }

        private HttpClient CreateClientToGetAsync(string token)
        {
            var client = _httpClientFactory.CreateClient("JadlogGetAsync");
            client.DefaultRequestHeaders.Add("ContentType", "application/xml");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            return client;
        }

        private HttpClient CreateClientToPostAsync(string token)
        {
            var client = _httpClientFactory.CreateClient("JadlogPostAsync");
            client.DefaultRequestHeaders.Add("ContentType", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            return client;
        }
    }
}
