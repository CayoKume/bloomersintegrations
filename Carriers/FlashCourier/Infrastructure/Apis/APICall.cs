using BloomersCarriersIntegrations.FlashCourier.Domain.Entities;
using BloomersCarriersIntegrations.FlashCourier.Infrastructure.Repositorys;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
//using System.Text.Json; testar sem

namespace BloomersCarriersIntegrations.FlashCourier.Infrastructure.Apis
{
    public class APICall : IAPICall
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IFlashCourierRepository _flashCourierRepository;

        public APICall(IFlashCourierRepository flashCourierRepository, IHttpClientFactory httpClientFactory) =>
            (_flashCourierRepository, _httpClientFactory) = (flashCourierRepository, httpClientFactory);

        public async Task<HAWBResponse?> GetHAWB(string[] numEncCli, string doc_company)
        {
            try
            {
                var authentication = await _flashCourierRepository.GetAuthenticationUser(doc_company);
                var authResponse = await GetAuth(authentication.login, authentication.senha);
                var awbResquest = await _flashCourierRepository.GetAWBRequest(doc_company);

                var jObject = new JObject
                {
                    { "clienteId", awbResquest.clienteId },
                    { "cttId", new JArray { awbResquest.cttId } },
                    { "numEncCli", new JArray { numEncCli } }
                };

                var client = CreateCliente(
                    authResponse.access_token
                );

                var content = new StringContent(JsonConvert.SerializeObject(jObject), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(client.BaseAddress + "/padrao/v2/consulta", content);

                if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<HAWBResponse>(result);
                }
                else
                    throw new Exception($"{response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - GetHAWB - Erro ao obter AWBs através da API ""/padrao/v2/consulta"" - {ex.Message}");
            }
        }

        public async Task<List<InsertHAWBSuccessResponse>> PostHAWB(Order model)
        {
            try
            {
                int ccusto_id = 0;
                int tipo_enc_id = 0;
                int id_local_rem = 0;
                int cliente_id = 0;
                int ctt_id = 0;

                if (model.number.Contains("MI-"))
                {
                    ccusto_id = 47993;
                    tipo_enc_id = 26687;
                    id_local_rem = 47809;
                    cliente_id = 12507;
                    ctt_id = 14511;
                }
                else //OPEN ERA
                {
                    ccusto_id = 32277;
                    tipo_enc_id = 18582;
                    id_local_rem = 39512;
                    cliente_id = 6801;
                    ctt_id = 8912;
                }

                var authentication = await _flashCourierRepository.GetAuthenticationUser(model.company.doc_company);

                var jObject = new JArray(
                            new JObject {
                                { "dna_hawb", 7 },
                                { "ccusto_id", ccusto_id },
                                { "tipo_enc_id", tipo_enc_id },
                                { "prod_flash_id", 152 },
                                { "frq_rec_id", "DSP" },
                                { "id_local_rem", id_local_rem },
                                { "cliente_id", cliente_id },
                                { "ctt_id", ctt_id },
                                //registro no banco
                                { "num_enc_cli", model.number }, //codigo de rastreio alfanumerico com ate 30 digitos, pode ser um sequencial nosso
                                { "num_cliente", model.invoice.number_nf },
                                { "nome_rem", "OPEN ERA" },
                                { "endHawbs", new JObject {
                                        { "nome_des", model.client.reason_client },
                                        { "logr_dest", model.client.address_client },
                                        { "bairro_des", model.client.neighborhood_client },
                                        { "num_des", model.client.street_number_client },
                                        { "fone1_des", model.client.fone_client },
                                        { "cid_dest", model.client.city_client },
                                        { "uf_dest", model.client.uf_client },
                                        { "cep_dest", model.client.zip_code_client },
                                        { "compl_end_dest", model.client.complement_address_client }
                                    } 
                                },
                                { "cod_lote", "1234567" },
                                { "peso_declarado", model.weight },
                                { "qtde_itens", model.quantity },
                                { "valor", Convert.ToDouble(model.invoice.amount_nf) },
                                { "cpf_des", model.client.doc_client },
                                { "email_des", model.client.email_client },
                                { "chave_nf", model.invoice.key_nfe_nf },
                                { "endHawbs2", new JObject {
                                        { "bairro_des", "" },
                                        { "cid_dest", "" },
                                        { "compl_end_dest", "" },
                                        { "fone1_des", "" },
                                        { "fone2_des", "" },
                                        { "fone3_des", "" },
                                        { "logr_dest", "" },
                                        { "nome_des", "" },
                                        { "num_des", "" },
                                        { "uf_dest", "" },
                                    } 
                                }
                            }
                        );

                await _flashCourierRepository.GenerateRequestLog(model.number, Newtonsoft.Json.JsonConvert.SerializeObject(jObject)); //devolver id de o log

                //HOMOLOG
                //var client = CreateCliente(userName: "sao.erick", password: "123");

                var client = CreateCliente(userName: authentication.login, password: authentication.senha);

                var response = await client.PostAsync(client.BaseAddress + "/padrao/importacao", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jObject), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<InsertHAWBSuccessResponse>>(result);
                }
                else
                    throw new Exception($"{response.StatusCode}"); //associar request com status code - ou retornar objeto para tratar erro
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - PostHAWB - Erro ao enviar o pedido: {model.number} através da API ""/padrao/importacao"" - {ex.Message}");
            }
        }

        private async Task<AuthResponse?> GetAuth(string usuario, string senha)
        {
            try
            {
                var jObject = new JObject
                {
                    { "login", usuario },
                    { "senha", senha }
                };

                var token = "8f1ef4f5cd3989238192cf1e3306d06d88398b8521a7f4ec8c6a9d55c1429f0b"; //HERE

                var client = CreateCliente(token);

                var response = await client.PostAsync(client.BaseAddress + "/api/v1/token", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jObject), Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AuthResponse>(result);
                }
                else
                    throw new Exception($"{response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - GetAuth - Erro ao obter token de autorização através da API ""/api/v1/token"" - {ex.Message}");
            }
        }

        private HttpClient CreateCliente(string token)
        {
            var client = _httpClientFactory.CreateClient("FlashCourierAPI");
            client.DefaultRequestHeaders.Add("Authorization", token);
            return client;
        }

        private HttpClient CreateCliente(string userName, string password)
        {
            var client = _httpClientFactory.CreateClient("FlashCourierAPI");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{userName}:{password}")));
            client.DefaultRequestHeaders.Add("Cookie", "ROUTEID=.1");

            return client;
        }  
    }
}
