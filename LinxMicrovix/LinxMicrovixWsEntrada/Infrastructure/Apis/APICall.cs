using System.Net;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsEntrada.Infrastructure.Apis
{
    public class APICall : IAPICall
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APICall(IHttpClientFactory httpClientFactory) =>
            (_httpClientFactory) = (httpClientFactory);

        public string BuildBodyRequest(string parametersList, string? endPointName, string authentication, string key, string? doc_company)
        {
            return $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
                        xmlns:tem=""http://tempuri.org/""
                        xmlns:linx=""http://schemas.datacontract.org/2004/07/Linx.Microvix.WebApi.Importacao.Requests""
                        xmlns:linx1=""http://schemas.datacontract.org/2004/07/Linx.Microvix.WebApi.Business.Api""
                        xmlns:linx2=""http://schemas.datacontract.org/2004/07/Linx.Microvix.WebApi.Importacao"">
                            <soapenv:Header/>
                            <soapenv:Body>
                                <tem:Importar>
                                    <tem:request>
                                        <linx:ParamsSeletorDestino>
                                            <linx1:CommandParameter>
                                                <linx1:Name>chave</linx1:Name>
                                                <linx1:Value>{key}</linx1:Value>
                                            </linx1:CommandParameter>
                                            <linx1:CommandParameter>
                                                <linx1:Name>cnpjEmp</linx1:Name>
                                                <linx1:Value>{doc_company}</linx1:Value>
                                            </linx1:CommandParameter>
                                            <linx1:CommandParameter>
                                                <linx1:Name>IdPortal</linx1:Name>
                                                <linx1:Value>15133</linx1:Value>
                                            </linx1:CommandParameter>
                                        </linx:ParamsSeletorDestino>
                                        <linx:Tabela>
                                            <linx2:Comando>{endPointName}</linx2:Comando>
                                            <linx2:Registros>
                                                <linx:Registros>
                                                    <linx:Colunas>
                                                        {parametersList}
                                                    </linx:Colunas>
                                                </linx:Registros>
                                            </linx2:Registros>
                                        </linx:Tabela>
                                        <linx:UserAuth>
                                            <linx2:Pass>{authentication}</linx2:Pass>
                                            <linx2:User>{authentication}</linx2:User>
                                        </linx:UserAuth>
                                    </tem:request>
                                </tem:Importar>
                            </soapenv:Body>
                        </soapenv:Envelope>";
        }

        public async Task<string> CallAPI(string body)
        {
            try
            {
                var client = CreateClient();

                var response = await client.PostAsync(System.String.Empty, new StringContent(body, System.Text.Encoding.UTF8, "text/xml"));

                if (response.StatusCode == HttpStatusCode.OK)
                    return await response.Content.ReadAsStringAsync();
                else
                    throw new Exception($"{response.StatusCode}");
            }
            catch
            {
                throw;
            }
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient("LinxMicrovixWsEntradaAPI");
            client.DefaultRequestHeaders.Add("Content-Type", "text/xml;charset=UTF-8");
            client.DefaultRequestHeaders.Add("SOAPAction", "http://tempuri.org/IImportador/Importar");
            client.DefaultRequestHeaders.Add("Accept", "text/xml");

            return client;
        }

        public List<Dictionary<string, string>> DeserializeXML(string response)
        {
            throw new NotImplementedException();
        }
    }
}
