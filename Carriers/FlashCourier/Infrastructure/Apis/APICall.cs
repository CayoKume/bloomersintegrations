namespace BloomersCarriersIntegrations.FlashCourier.Infrastructure.Apis
{
    public class APICall
    {
        public static HAWBResponseModel? GetHAWB(string[] numEncCli)
        {
            try
            {
                var authResponse = GetAuth();

                var request = CreateRequest("/padrao/v2/consulta", Method.Post, authResponse.access_token);

                int clienteId = 0;
                List<int> cttId = new List<int>();

                //OPEN ERA
                clienteId = 6801;
                cttId.Add(8912);

                var model = new HAWBRequestModel(clienteId, cttId.ToArray(), numEncCli);
                request.AddHeader("Authorization", authResponse.access_token);
                request.AddJsonBody(model);

                var client = new RestClient();
                var response = client.Execute(request);
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonSerializer.Deserialize<HAWBResponseModel>(response?.Content);
                }

                return new HAWBResponseModel();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<InsertHAWBSuccessResponseModel> PostHAWB(Pedido model)
        {
            try
            {
                var list = new List<InsertHAWBRequestModel.Root>();

                var modelRequest = new InsertHAWBRequestModel.Root()
                {
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
                    num_enc_cli = model.nr_pedido, //codigo de rastreio alfanumerico com ate 30 digitos, pode ser um sequencial nosso
                    num_cliente = model.notaFiscal.numero_nf,
                    nome_rem = "OPEN ERA",
                    endHawbs = new InsertHAWBRequestModel.EndHawbs()
                    {
                        nome_des = model.cliente.razao_cliente,
                        logr_dest = model.cliente.endereco_cliente,
                        bairro_des = model.cliente.bairro_cliente,
                        num_des = model.cliente.numero_rua_cliente,
                        fone1_des = model.cliente.fone_cliente,
                        cid_dest = model.cliente.cidade_cliente,
                        uf_dest = model.cliente.uf_cliente,
                        cep_dest = Convert.ToInt32(model.cliente.cep_cliente.Replace("-", "")),
                        compl_end_dest = model.cliente.complemento_endereco_cliente,
                        //fone2_des = "",
                        //fone3_des = ""
                    },
                    cod_lote = "1234567", //caso nos tenhamos o codigo do lote, caso não tenha pode enviar a data do despacho do pedido
                    peso_declarado = model.peso_bruto,
                    qtde_itens = model.quantidade,
                    valor = Convert.ToDouble(model.notaFiscal.total_nf),
                    cpf_des = model.cliente.doc_cliente,
                    email_des = model.cliente.email_cliente,
                    chave_nf = model.notaFiscal.chave_nfe_nf,
                    //cnpj_des = model.cnpj_emp, //enviar ou cnpj ou cpf
                    //ie_des = model.inscricao_estadual, //inscricao estadual so é obrigatoria caso cnpj seja informado

                    endHawbs2 = new InsertHAWBRequestModel.EndHawbs2
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
                };

                list.Add(modelRequest);

                var request = CreateRequest("/padrao/importacao", Method.Post, "");
                request.AddHeader("Cookie", "ROUTEID=.1");
                request.AddJsonBody(list);

                var options = new RestClientOptions("http://example.com")
                {
                    Authenticator = new HttpBasicAuthenticator("ws.OpenEra", "9Hc#W3J2`6BE")
                };

                var client = new RestClient(options);

                var response = client.Execute(request);
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonSerializer.Deserialize<List<InsertHAWBSuccessResponseModel>>(response?.Content);
                }
                return new List<InsertHAWBSuccessResponseModel>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static AuthResponseModel? GetAuth()
        {
            var model = new AuthRequestModel("ws.OpenEra", "9Hc#W3J2`6BE");
            var token = "8f1ef4f5cd3989238192cf1e3306d06d88398b8521a7f4ec8c6a9d55c1429f0b";
            var request = CreateRequest("/api/v1/token", Method.Post, token);
            request.AddJsonBody(model);

            var client = new RestClient();
            var response = client.Execute(request);

            if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            {
                var result = JsonSerializer.Deserialize<AuthResponseModel>(response?.Content);
                return result;
            }

            return new AuthResponseModel();
        }

        private static RestRequest CreateRequest(string route, Method method, string token)
        {
            //HOMOLOG
            //https://homolog.flashpegasus.com.br/FlashPegasus/rest

            var baseUrl = $"https://webservice.flashpegasus.com.br/FlashPegasus/rest";
            var request = new RestRequest(baseUrl + route, method);
            request.AddHeader("Authorization", token);
            return request;
        }
    }
}
