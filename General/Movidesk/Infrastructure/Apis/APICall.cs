namespace BloomersGeneralIntegrations.Movidesk.Infrastructure.Apis
{
    public class APICall : IAPICall
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APICall(IHttpClientFactory httpClientFactory) =>
            (_httpClientFactory) = (httpClientFactory);

        public async Task<string> GetAsync(string filter)
        {
            try
            {
                //get token
                var token = "7b034c69-b176-46f7-bc71-d6a290f18b73";
                var client = CreateClient();
                var response = await client.GetAsync(client + $"/public/v1/tickets?token={token}&$select=id, subject, createdDate, resolvedIn, serviceFirstLevel&$filter={filter}&$expand= createdBy($select=businessName, email, phone), customFieldValues($expand=items), actions($select=id, description)");

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient("MovideskAPI");
            client.DefaultRequestHeaders.Add("accept", "application/json");

            return client;
        }
    }
}
