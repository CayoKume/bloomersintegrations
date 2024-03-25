using BloomersCarriersIntegrations.FlashCourier.Domain.Entities;
using BloomersCarriersIntegrations.FlashCourier.Infrastructure.Repositorys;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;

namespace BloomersCarriersIntegrations.FlashCourier.Infrastructure.Apis
{
    public class APICall : IAPICall
    {
        private readonly IFlashCourierRepository _flashCourierRepository;

        public APICall(IFlashCourierRepository flashCourierRepository) =>
            (_flashCourierRepository) = (flashCourierRepository);

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

                var request = CreateRequest(
                    "/padrao/v2/consulta",
                    Method.Post,
                    authResponse.access_token,
                    jObject.ToString()
                );

                var client = new RestClient();
                var response = client.Execute(request);

                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                    return JsonSerializer.Deserialize<HAWBResponse>(response?.Content);
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
                var authentication = await _flashCourierRepository.GetAuthenticationUser(model.company.doc_company);

                var jObject = new JArray(
                            new JObject {
                                { "dn_hawb", 7 },
                                { "ccusto_id", 32277 },
                                { "tipo_enc_id", 18582 },
                                { "prod_flash_id", 152 },
                                { "frq_rec_id", "DSP" },
                                { "id_local_rem", 39512 },
                                { "cliente_id", 6801 },
                                { "ctt_id", 8912 },
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

                var list = new List<InsertHAWBRequest.Root> 
                {
                    new InsertHAWBRequest.Root {
                        //OPEN ERA
                        dna_hawb = 7,
                        ccusto_id = 32277,
                        tipo_enc_id = 18582,
                        prod_flash_id = 152,
                        frq_rec_id = "DSP",
                        id_local_rem = 39512,
                        cliente_id = 6801,
                        ctt_id = 8912,

                        //registro no banco
                        num_enc_cli = model.number, //codigo de rastreio alfanumerico com ate 30 digitos, pode ser um sequencial nosso
                        num_cliente = model.invoice.number_nf,
                        nome_rem = "OPEN ERA",
                        endHawbs = new InsertHAWBRequest.EndHawbs()
                        {
                            nome_des = model.client.reason_client,
                            logr_dest = model.client.address_client,
                            bairro_des = model.client.neighborhood_client,
                            num_des = model.client.street_number_client,
                            fone1_des = model.client.fone_client,
                            cid_dest = model.client.city_client,
                            uf_dest = model.client.uf_client,
                            cep_dest = Convert.ToInt32(model.client.zip_code_client.Replace("-", "")),
                            compl_end_dest = model.client.complement_address_client
                        },
                        cod_lote = "1234567", //caso nos tenhamos o codigo do lote, caso não tenha pode enviar a data do despacho do pedido
                        peso_declarado = model.weight,
                        qtde_itens = model.quantity,
                        valor = Convert.ToDouble(model.invoice.amount_nf),
                        cpf_des = model.client.doc_client,
                        email_des = model.client.email_client,
                        chave_nf = model.invoice.key_nfe_nf,

                        endHawbs2 = new InsertHAWBRequest.EndHawbs2
                        {
                            bairro_des = "",
                            cid_dest = "",
                            compl_end_dest = "",
                            fone1_des = "",
                            fone2_des = "",
                            fone3_des = "",
                            logr_dest = "",
                            nome_des = "",
                            num_des = "",
                            uf_dest = "",
                        },
                    }
                };

                var teste = JsonSerializer.Serialize(list);
                var _teste = JsonSerializer.Serialize(jObject.ToString());

                var request = CreateRequest(
                    "/padrao/importacao",
                    Method.Post,
                    "",
                    _teste
                );

                request.AddHeader("Cookie", "ROUTEID=.1");

                var options = new RestClientOptions("http://example.com")
                {
                    Authenticator = new HttpBasicAuthenticator(authentication.login, authentication.senha)
                };

                var client = new RestClient(options);
                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                    return JsonSerializer.Deserialize<List<InsertHAWBSuccessResponse>>(response?.Content);
                else
                    throw new Exception($"{response.StatusCode}");
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
                
                var request = CreateRequest(
                    "/api/v1/token", 
                    Method.Post, 
                    token,
                    jObject.ToString()
                );

                var client = new RestClient();
                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                    return JsonSerializer.Deserialize<AuthResponse>(response?.Content);
                else
                    throw new Exception($"{response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception(@$"FlashCourier - GetAuth - Erro ao obter token de autorização através da API ""/api/v1/token"" - {ex.Message}");
            }
        }

        private RestRequest CreateRequest(string route, Method method, string token, string jObject)
        {
            //HOMOLOG
            //https://homolog.flashpegasus.com.br/FlashPegasus/rest

            var baseUrl = $"https://webservice.flashpegasus.com.br/FlashPegasus/rest";
            var request = new RestRequest(baseUrl + route, method);
            request.AddHeader("Authorization", token);
            request.AddJsonBody(jObject);
            return request;
        }
    }
}
