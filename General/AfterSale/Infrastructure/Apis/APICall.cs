using System.Net.Http.Headers;

namespace BloomersGeneralIntegrations.AfterSale.Infrastructure.Apis
{
    public class APICall : IAPICall
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APICall(IHttpClientFactory httpClientFactory) =>
            (_httpClientFactory) = (httpClientFactory);

        public async Task<string> GetReversesAsync(string token)
        {
            try
            {
                var parameters = new Dictionary<string, string>
                    {
                        { "start_date", $"{DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd")}" },
                        { "end_date", $"{DateTime.Now.ToString("yyyy-MM-dd")}" }
                    };
                var encodedParameters = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();

                var client = CreateClient(token);

                var response = await client.SendAsync(
                    new HttpRequestMessage(
                        HttpMethod.Get,
                        client.BaseAddress + encodedParameters
                    )
                );

                return await response.Content.ReadAsStringAsync();
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

        public async Task<string> GetReversesByPageAsync(string token, int page)
        {
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    { "start_date", $"{DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd")}" },
                    { "end_date", $"{DateTime.Now.ToString("yyyy-MM-dd")}" },
                    { "page", $"{page}" }
                };
                var encodedParameters = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();

                var client = CreateClient(token);

                var response = await client.SendAsync(
                    new HttpRequestMessage(
                        HttpMethod.Get,
                        client.BaseAddress + encodedParameters
                    )
                );

                return await response.Content.ReadAsStringAsync();
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
            var client = _httpClientFactory.CreateClient("AfterSaleAPI");
            client.DefaultRequestHeaders.Add("Accept", "application/pdf");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }
    }
}
