using BloomersCarriersIntegrations.Jadlog.Domain.Entities;
using BloomersCarriersIntegrations.Jadlog.Infrastructure.Apis;
using BloomersCarriersIntegrations.Jadlog.Infrastructure.Repositorys;
using Newtonsoft.Json.Linq;

namespace BloomersCarriersIntegrations.Jadlog.Application.Services
{
    public class JadlogService : IJadlogService
    {
        private readonly IAPICall _apiCall;
        private readonly IJadlogRepository _jadlogRepository;

        public JadlogService(IJadlogRepository jadlogRepository, IAPICall apiCall) =>
            (_jadlogRepository, _apiCall) = (jadlogRepository, apiCall);

        //public Task<bool> UpdateShippedOrdersLog()
        //{
        //    throw new NotImplementedException();
        //    //try
        //    //{

        //    //}
        //    //catch
        //    //{
        //    //    throw;
        //    //}
        //}

        public async Task<bool> SendOrderJadlog(string order_number)
        {
            try
            {
                var order = await _jadlogRepository.GetInvoicedOrder(order_number);
                if (order is not null)
                {
                    var codUnidadeOri = String.Empty;
                    var parameter = await _jadlogRepository.GetParameters(order.tomador.doc_company, order.TIPO_SERVICO);

                    if (order.company.doc_company == "38367316001080")
                        codUnidadeOri = "1420";

                    else if (order.company.doc_company == "38367316000601")
                        codUnidadeOri = "792";

                    else if (order.company.doc_company == "38367316000946" || order.company.doc_company == "42538267000500")
                        codUnidadeOri = "2362";

                    else
                        codUnidadeOri = "1099";

                    var jObject = new JObject
                    {
                        { "codCliente", parameter.cod_client },
                        { "conteudo", order.itens.FirstOrDefault().description_product.Replace("Ç","C").Replace("Ú","U").PadRight(25).Substring(0, order.itens.FirstOrDefault().description_product.Length > 24 ? 24 : order.itens.FirstOrDefault().description_product.Length) },
                        { "pedido", new JArray (order.number) },
                        { "totPeso", order.itens.FirstOrDefault().weight_product },
                        { "totValor", order.invoice.amount_nf },
                        { "obs", null },
                        { "modalidade", parameter.modality },
                        { "contaCorrente", parameter.conta },
                        { "tpColeta", "K" },
                        { "tipoFrete", null },
                        { "cdUnidadeOri", codUnidadeOri },
                        { "cdUnidadeDes", null },
                        { "cdPickupOri", null },
                        { "cdPickupDes", null },
                        { "nrContrato", null },
                        { "servico", null },
                        { "shipmentId", null },
                        { "vlColeta", null },
                        { "des", new JObject {
                                { "nome", order.client.reason_client },
                                { "cnpjCpf", order.client.doc_client },
                                { "ie", order.client.state_registration_client.Replace(".", "") },
                                { "endereco", order.client.address_client },
                                { "numero", order.client.street_number_client },
                                { "compl", order.client.complement_address_client.PadRight(39).Substring(0, order.client.complement_address_client.Length > 38 ? 38 : order.client.complement_address_client.Length) },
                                { "bairro", order.client.neighborhood_client },
                                { "cidade", order.client.city_client },
                                { "uf", order.client.uf_client },
                                { "cep", order.client.zip_code_client },
                                { "fone", null },
                                { "cel", order.client.fone_client },
                                { "email", order.client.email_client },
                                { "contato", order.client.reason_client.PadRight(40).Substring(0, order.client.reason_client.Length > 39 ? 39 : order.client.reason_client.Length) }
                            }
                        },
                        { "rem", new JObject {
                                { "nome", order.company.reason_company },
                                { "cnpjCpf", order.company.doc_company },
                                { "ie", order.company.state_registration_company.Replace(".", "") },
                                { "endereco", order.company.address_company },
                                { "numero", order.company.street_number_company },
                                { "compl", order.company.complement_address_company.PadRight(20).Substring(0, order.company.complement_address_company.Length > 19 ? 19 : order.company.complement_address_company.Length) },
                                { "bairro", order.company.neighborhood_company },
                                { "cidade", order.company.city_company },
                                { "uf", order.company.uf_company },
                                { "cep", order.company.zip_code_company },
                                { "fone", null },
                                { "cel", order.company.fone_company },
                                { "email", order.company.email_company },
                                { "contato", order.company.name_company.PadRight(40).Substring(0, order.company.name_company.Length > 39 ? 39 : order.company.name_company.Length) }
                            }
                        },
                        { "tomador", new JObject {
                                { "nome", order.tomador.reason_company },
                                { "cnpjCpf", order.tomador.doc_company },
                                { "ie", order.tomador.state_registration_company.Replace(".", "") },
                                { "endereco", order.tomador.address_company },
                                { "numero", order.tomador.street_number_company },
                                { "compl", order.tomador.complement_address_company },
                                { "bairro", order.tomador.neighborhood_company },
                                { "cidade", order.tomador.city_company },
                                { "uf", order.tomador.uf_company },
                                { "cep", order.tomador.zip_code_company },
                                { "fone", null },
                                { "cel", order.tomador.fone_company },
                                { "email", order.tomador.email_company },
                                { "contato", order.tomador.name_company.PadRight(40).Substring(0, order.tomador.name_company.Length > 39 ? 39 : order.tomador.name_company.Length) }
                            }
                        },
                        { "dfe", new JArray(
                            new JObject {
                                { "cfop", order.cfop.Replace(".", "") },
                                { "danfeCte", order.invoice.key_nfe_nf },
                                { "nrDoc", order.invoice.number_nf },
                                { "serie", order.invoice.serie_nf },
                                { "tpDocumento", 2 },
                                { "valor", order.invoice.amount_nf },
                            })
                        },
                        { "volume", new JArray(
                            new JObject {
                                { "altura", 1 },
                                { "comprimento", 1 },
                                { "identificador", order.number },
                                { "largura", 1 },
                                { "peso", 1 }
                            })
                        },
                    };

                    var response = await _apiCall.PostAsync(order.number, "pedido/incluir", parameter.token, jObject);
                    await _jadlogRepository.GenerateResponseLog(order.number, System.Text.Json.JsonSerializer.Deserialize<Response>(response));
                    
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SendOrdersJadlog()
        {
            try
            {
                var orders = await _jadlogRepository.GetInvoicedOrders();
                if (orders.Count() > 0)
                {
                    foreach (var order in orders)
                    {
                        var codUnidadeOri = String.Empty;
                        var parameter = await _jadlogRepository.GetParameters(order.tomador.doc_company, order.TIPO_SERVICO);

                        if (order.company.doc_company == "38367316001080")
                            codUnidadeOri = "1420";

                        else if (order.company.doc_company == "38367316000601")
                            codUnidadeOri = "792";

                        else if (order.company.doc_company == "38367316000946" || order.company.doc_company == "42538267000500")
                            codUnidadeOri = "2362";

                        else
                            codUnidadeOri = "1099";

                        var jObject = new JObject
                        {
                            { "codCliente", parameter.cod_client },
                            { "conteudo", order.itens.FirstOrDefault().description_product.Replace("Ç","C").Replace("Ú","U").PadRight(25).Substring(0, order.itens.FirstOrDefault().description_product.Length > 24 ? 24 : order.itens.FirstOrDefault().description_product.Length) },
                            { "pedido", new JArray (order.number) },
                            { "totPeso", order.itens.FirstOrDefault().weight_product },
                            { "totValor", order.invoice.amount_nf },
                            { "obs", null },
                            { "modalidade", parameter.modality },
                            { "contaCorrente", parameter.conta },
                            { "tpColeta", "K" },
                            { "tipoFrete", null },
                            { "cdUnidadeOri", codUnidadeOri },
                            { "cdUnidadeDes", null },
                            { "cdPickupOri", null },
                            { "cdPickupDes", null },
                            { "nrContrato", null },
                            { "servico", null },
                            { "shipmentId", null },
                            { "vlColeta", null },
                            { "des", new JObject {
                                    { "nome", order.client.reason_client },
                                    { "cnpjCpf", order.client.doc_client },
                                    { "ie", order.client.state_registration_client.Replace(".", "") },
                                    { "endereco", order.client.address_client },
                                    { "numero", order.client.street_number_client },
                                    { "compl", order.client.complement_address_client.PadRight(39).Substring(0, order.client.complement_address_client.Length > 38 ? 38 : order.client.complement_address_client.Length) },
                                    { "bairro", order.client.neighborhood_client },
                                    { "cidade", order.client.city_client },
                                    { "uf", order.client.uf_client },
                                    { "cep", order.client.zip_code_client },
                                    { "fone", null },
                                    { "cel", order.client.fone_client },
                                    { "email", order.client.email_client },
                                    { "contato", order.client.reason_client.PadRight(40).Substring(0, order.client.reason_client.Length > 39 ? 39 : order.client.reason_client.Length) }
                                }
                            },
                            { "rem", new JObject {
                                    { "nome", order.company.reason_company },
                                    { "cnpjCpf", order.company.doc_company },
                                    { "ie", order.company.state_registration_company.Replace(".", "") },
                                    { "endereco", order.company.address_company },
                                    { "numero", order.company.street_number_company },
                                    { "compl", order.company.complement_address_company.PadRight(20).Substring(0, order.company.complement_address_company.Length > 19 ? 19 : order.company.complement_address_company.Length) },
                                    { "bairro", order.company.neighborhood_company },
                                    { "cidade", order.company.city_company },
                                    { "uf", order.company.uf_company },
                                    { "cep", order.company.zip_code_company },
                                    { "fone", null },
                                    { "cel", order.company.fone_company },
                                    { "email", order.company.email_company },
                                    { "contato", order.company.name_company.PadRight(40).Substring(0, order.company.name_company.Length > 39 ? 39 : order.company.name_company.Length) }
                                }
                            },
                            { "tomador", new JObject {
                                    { "nome", order.tomador.reason_company },
                                    { "cnpjCpf", order.tomador.doc_company },
                                    { "ie", order.tomador.state_registration_company.Replace(".", "") },
                                    { "endereco", order.tomador.address_company },
                                    { "numero", order.tomador.street_number_company },
                                    { "compl", order.tomador.complement_address_company },
                                    { "bairro", order.tomador.neighborhood_company },
                                    { "cidade", order.tomador.city_company },
                                    { "uf", order.tomador.uf_company },
                                    { "cep", order.tomador.zip_code_company },
                                    { "fone", null },
                                    { "cel", order.tomador.fone_company },
                                    { "email", order.tomador.email_company },
                                    { "contato", order.tomador.name_company.PadRight(40).Substring(0, order.tomador.name_company.Length > 39 ? 39 : order.tomador.name_company.Length) }
                                }
                            },
                            { "dfe", new JArray(
                                new JObject {
                                    { "cfop", order.cfop.Replace(".", "") },
                                    { "danfeCte", order.invoice.key_nfe_nf },
                                    { "nrDoc", order.invoice.number_nf },
                                    { "serie", order.invoice.serie_nf },
                                    { "tpDocumento", 2 },
                                    { "valor", order.invoice.amount_nf },
                                })
                            },
                            { "volume", new JArray(
                                new JObject {
                                    { "altura", 1 },
                                    { "comprimento", 1 },
                                    { "identificador", order.number },
                                    { "largura", 1 },
                                    { "peso", 1 }
                                }) 
                            },
                        };

                        var response = await _apiCall.PostAsync(order.number, "pedido/incluir", parameter.token, jObject);
                        await _jadlogRepository.GenerateResponseLog(order.number, System.Text.Json.JsonSerializer.Deserialize<Response>(response));
                    }
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
