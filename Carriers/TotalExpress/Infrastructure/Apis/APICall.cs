using BloomersCarriersIntegrations.TotalExpress.Domain.Entities;
using BloomersCarriersIntegrations.TotalExpress.Infrastructure.Repositorys;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace BloomersCarriersIntegrations.TotalExpress.Infrastructure.Apis
{
    public class APICall : IAPICall
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITotalExpressRepository _totalExpressRepository;

        public APICall(ITotalExpressRepository totalExpressRepository, IHttpClientFactory httpClientFactory) =>
            (_totalExpressRepository, _httpClientFactory) = (totalExpressRepository, httpClientFactory);

        public async Task<string> PostAWB(Order order)
        {
            try
            {
                var jArrayObj = new JArray();

                foreach (var item in order.itens)
                {
                    var jObject = new JObject
                    {
                        { "remetenteId", order.REMETENTEID },
                        { "cnpj", order.company.doc_company },
                        { "encomendas", new JArray(
                                new JObject
                                {
                                    { "servicoTipoInfo", null },
                                    { "servicoTipo", order.SERVICOTIPO },
                                    { "coletaInfo", String.Empty },
                                    { "condFrete", "CIF" },
                                    { "entregaTipo", 0 },
                                    { "icmsIsencao", 0 },
                                    { "natureza", item.description_product.Replace("Ç","C").Replace("Ú","U").PadRight(25).Substring(0, item.description_product.Length > 24 ? 24 : item.description_product.Length) },
                                    { "pedido", order.number.Trim() },
                                    { "peso", 0 },
                                    { "volumes", order.volumes },
                                    { "volumesTipo", "CX" },
                                    { "clienteCodigo", int.Parse(order.client.cod_client) },
                                    { "destinatario", new JObject 
                                        {
                                            { "nome", order.client.reason_client },    
                                            { "cpfCnpj", order.client.doc_client },    
                                            { "ie", order.client.state_registration_client },    
                                            { "email", order.client.email_client },    
                                            { "telefone1", order.client.fone_client.Replace("(","").Replace(")","").Replace("-","").Replace(" ","") },    
                                            { "telefone2", "" },    
                                            { "telefone3", "" },    
                                            { "endereco", new JObject
                                                {
                                                    { "bairro", order.client.neighborhood_client },
                                                    { "cep", order.client.zip_code_client },
                                                    { "cidade", order.client.city_client },
                                                    { "complemento", order.client.complement_address_client },
                                                    { "estado", order.client.uf_client },
                                                    { "logradouro", order.client.address_client },
                                                    { "numero", order.client.street_number_client },
                                                    { "pais", "BR" },
                                                    { "pontoReferencia", "" }
                                                }
                                            }
                                        }
                                    },
                                    { "docFiscal", new JObject
                                        { 
                                            "nfe", new JArray(
                                                new JObject
                                                {
                                                    { "nfeCfop", order.cfop.Replace(".","") },
                                                    { "nfeChave", order.invoice.key_nfe_nf },
                                                    { "nfeData", order.invoice.date_emission_nf.ToString("dd-MM-yyyy") },
                                                    { "nfeNumero", int.Parse(order.invoice.number_nf) },
                                                    { "nfeSerie", order.invoice.serie_nf },
                                                    { "nfeValProd", order.invoice.amount_nf },
                                                    { "nfeValTotal", order.invoice.amount_nf }
                                                }
                                            )
                                        }
                                    }
                                }
                            )
                        },
                    };

                    jArrayObj.Add(jObject);
                }

                await _totalExpressRepository.GeraRequestLog(order.number, Newtonsoft.Json.JsonConvert.SerializeObject(jArrayObj));

                var token = await GetAuthToken("api-newbloomers", "He7weir@o");

                var client = CreateClientToSendAWB(token);

                var response = await client.PostAsync(client + "ics-edi/v1/coleta/smartLabel/registrar", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jArrayObj), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result.ToString();
                }
                else
                    throw new Exception($"{response.StatusCode}");
            }
            catch (Exception ex) when (ex.Message.Contains("GetAuthToken"))
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - SendData - Erro ao enviar o pedido: {order.number} para total - {ex.Message}");
            }
        }

        public async Task<Status> GetAWB(string senderID, string orderNumber)
        {
            try
            {
                //get senha e usuario EDI
                //var authentication = await _flashCourierRepository.GetAuthenticationUser(model.company.doc_company);

                var jObject = new JObject 
                {
                    { "remetenteId", senderID },
                    { "pedido", orderNumber }
                };

                var client = CreateClientToGetAWB("apistatusnew-prod", "GttTBS8x");

                var response = await client.PostAsync(client.BaseAddress + "previsao_entrega_atualizada.php", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jObject), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    var token = JToken.Parse(result);

                    if (token is JArray)
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Status>>(result).First();
                    else if (token is JObject)
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<Status>(result);
                    else
                        return null;
                }
                else
                    throw new Exception($"{response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - GetAWBStatus - Erro ao obter atualizacoes de status do pedido: {orderNumber} - {ex.Message}");
            }
        }

        private async Task<string> GetAuthToken(string username, string password)
        {
            try
            {
                //Get senha postawb
                //var authentication = await _flashCourierRepository.GetAuthenticationUser(model.company.doc_company);

                var jObject = new JObject
                {
                    { "username", username },
                    { "password", password }
                };

                var client = CreateClientToAuthToken("SUNTOnRvdGFs");

                var response = await client.PostAsync(client.BaseAddress + "ics-seguranca/v1/oauth2/tokenGerar", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jObject), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                {
                    var res = response.Content.ReadAsStringAsync().Result;

                    var token = (((Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(res.ToString())).First).Last.ToString();

                    return token;
                }
                else
                    throw new Exception($"{response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception(@$"TotalExpress - GetAuthToken - Erro ao obter token de authenticação para enviar o pedido:  - {ex.Message}");
            }
        }

        private HttpClient CreateClientToGetAWB(string username, string password)
        {
            var client = _httpClientFactory.CreateClient("TotalExpressAPI");
            client.DefaultRequestHeaders.Add("ContentType", "application/xml");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes($"{username}:{password}")));

            return client;
        }

        private HttpClient CreateClientToSendAWB(string token)
        {
            var client = _httpClientFactory.CreateClient("TotalExpressEdiAPI");
            client.DefaultRequestHeaders.Add("ContentType", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            return client;
        }

        private HttpClient CreateClientToAuthToken(string password)
        {
            var client = _httpClientFactory.CreateClient("TotalExpressAPI");
            client.DefaultRequestHeaders.Add("ContentType", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + password);

            return client;
        }
    }
}
