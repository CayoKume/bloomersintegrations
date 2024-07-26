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
                    var parameter = await _jadlogRepository.GetParameters(order.company.doc_company, order.TIPO_SERVICO);

                    foreach (var item in order.itens)
                    {
                        var jObject = new JObject
                        {
                            { "codCliente", parameter.cod_client },
                            { "conteudo", item.description_product.Replace("Ç","C").Replace("Ú","U").PadRight(25).Substring(0, item.description_product.Length > 24 ? 24 : item.description_product.Length) },
                            { "pedido", new JArray (order.number) },
                            { "totPeso", item.weight_product },
                            { "totValor", item.amount_product },
                            { "obs", null },
                            { "modalidade", parameter.modality },
                            { "contaCorrente", parameter.conta },
                            { "tpColeta", "K" },
                            { "tipoFrete", null },
                            { "cdUnidadeOri", "1099" },
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
                                    { "ie", order.client.state_registration_client },
                                    { "endereco", order.client.address_client },
                                    { "numero", order.client.street_number_client },
                                    { "compl", order.client.complement_address_client },
                                    { "bairro", order.client.neighborhood_client },
                                    { "cidade", order.client.city_client },
                                    { "uf", order.client.uf_client },
                                    { "cep", order.client.zip_code_client },
                                    { "fone", null },
                                    { "cel", order.client.fone_client },
                                    { "email", order.client.email_client },
                                    { "contato", order.client.reason_client }
                                }
                            },
                            { "rem", new JObject {
                                    { "nome", order.company.reason_company },
                                    { "cnpjCpf", order.company.doc_company },
                                    { "ie", order.company.state_registration_company },
                                    { "endereco", order.company.address_company },
                                    { "numero", order.company.street_number_company },
                                    { "compl", order.company.complement_address_company },
                                    { "bairro", order.company.neighborhood_company },
                                    { "cidade", order.company.city_company },
                                    { "uf", order.company.uf_company },
                                    { "cep", order.company.zip_code_company },
                                    { "fone", null },
                                    { "cel", order.company.fone_company },
                                    { "email", order.company.email_company },
                                    { "contato", order.company.name_company }
                                }
                            },
                            { "tomador", new JObject {
                                    { "nome", order.tomador.reason_company },
                                    { "cnpjCpf", order.tomador.doc_company },
                                    { "ie", order.tomador.state_registration_company },
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
                                    { "contato", order.tomador.name_company }
                                }
                            },
                            { "dfe", new JArray(
                                new JObject {
                                    { "cfop", order.cfop },
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
                        await _jadlogRepository.GenerateResponseLog(order.number, response);
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

        public async Task<bool> SendOrdersJadlog()
        {
            try
            {
                var orders = await _jadlogRepository.GetInvoicedOrders();
                if (orders.Count() > 0)
                {
                    foreach (var order in orders)
                    {
                        var parameter = await _jadlogRepository.GetParameters(order.company.doc_company, order.TIPO_SERVICO);

                        foreach (var item in order.itens)
                        {
                            var jObject = new JObject
                            {
                                { "codCliente", parameter.cod_client },
                                { "conteudo", item.description_product.Replace("Ç","C").Replace("Ú","U").PadRight(25).Substring(0, item.description_product.Length > 24 ? 24 : item.description_product.Length) },
                                { "pedido", new JArray (order.number) },
                                { "totPeso", item.weight_product },
                                { "totValor", item.amount_product },
                                { "obs", null },
                                { "modalidade", parameter.modality },
                                { "contaCorrente", parameter.conta },
                                { "tpColeta", "K" },
                                { "tipoFrete", null },
                                { "cdUnidadeOri", "1099" },
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
                                        { "ie", order.client.state_registration_client },
                                        { "endereco", order.client.address_client },
                                        { "numero", order.client.street_number_client },
                                        { "compl", order.client.complement_address_client },
                                        { "bairro", order.client.neighborhood_client },
                                        { "cidade", order.client.city_client },
                                        { "uf", order.client.uf_client },
                                        { "cep", order.client.zip_code_client },
                                        { "fone", null },
                                        { "cel", order.client.fone_client },
                                        { "email", order.client.email_client },
                                        { "contato", order.client.reason_client }
                                    }
                                },
                                { "rem", new JObject {
                                        { "nome", order.company.reason_company },
                                        { "cnpjCpf", order.company.doc_company },
                                        { "ie", order.company.state_registration_company },
                                        { "endereco", order.company.address_company },
                                        { "numero", order.company.street_number_company },
                                        { "compl", order.company.complement_address_company },
                                        { "bairro", order.company.neighborhood_company },
                                        { "cidade", order.company.city_company },
                                        { "uf", order.company.uf_company },
                                        { "cep", order.company.zip_code_company },
                                        { "fone", null },
                                        { "cel", order.company.fone_company },
                                        { "email", order.company.email_company },
                                        { "contato", order.company.name_company }
                                    }
                                },
                                { "tomador", new JObject {
                                        { "nome", order.tomador.reason_company },
                                        { "cnpjCpf", order.tomador.doc_company },
                                        { "ie", order.tomador.state_registration_company },
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
                                        { "contato", order.tomador.name_company }
                                    }
                                },
                                { "dfe", new JArray(
                                    new JObject {
                                        { "cfop", order.cfop },
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
                            await _jadlogRepository.GenerateResponseLog(order.number, response);
                        }
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

        //public async Task<bool> SendOrderJadlogAsEtur(string order_number)
        //{
        //    try
        //    {
        //        var order = await _jadlogRepository.GetInvoicedOrderETUR(order_number);
        //        if (order is not null)
        //        {
        //            var parameter = await _jadlogRepository.GetParameters(order.company.doc_company);

        //            foreach (var item in order.itens)
        //            {
        //                var jObject = new JObject
        //                {
        //                    { "codCliente", parameter.cod_client },
        //                    { "conteudo", item.description_product.Replace("Ç","C").Replace("Ú","U").PadRight(25).Substring(0, item.description_product.Length > 24 ? 24 : item.description_product.Length) },
        //                    { "pedido", new JArray (order.number) },
        //                    { "totPeso", item.weight_product },
        //                    { "totValor", item.amount_product },
        //                    { "obs", null },
        //                    { "modalidade", parameter.modality },
        //                    { "contaCorrente", null },
        //                    { "tpColeta", "K" },
        //                    { "tipoFrete", null },
        //                    { "cdUnidadeOri", null },
        //                    { "cdUnidadeDes", null },
        //                    { "cdPickupOri", null },
        //                    { "cdPickupDes", null },
        //                    { "nrContrato", null },
        //                    { "servico", null },
        //                    { "shipmentId", null },
        //                    { "vlColeta", null },
        //                    { "des", new JObject {
        //                            { "nome", order.client.reason_client },
        //                            { "cnpjCpf", order.client.doc_client },
        //                            { "ie", order.client.state_registration_client },
        //                            { "endereco", order.client.address_client },
        //                            { "numero", order.client.street_number_client },
        //                            { "compl", order.client.complement_address_client },
        //                            { "bairro", order.client.neighborhood_client },
        //                            { "cidade", order.client.city_client },
        //                            { "uf", order.client.uf_client },
        //                            { "cep", order.client.zip_code_client },
        //                            { "fone", null },
        //                            { "cel", order.client.fone_client },
        //                            { "email", order.client.email_client },
        //                            { "contato", order.client.reason_client }
        //                        }
        //                    },
        //                    { "rem", new JObject {
        //                            { "nome", order.company.reason_company },
        //                            { "cnpjCpf", order.company.doc_company },
        //                            { "ie", order.company.state_registration_company },
        //                            { "endereco", order.company.address_company },
        //                            { "numero", order.company.street_number_company },
        //                            { "compl", order.company.complement_address_company },
        //                            { "bairro", order.company.neighborhood_company },
        //                            { "cidade", order.company.city_company },
        //                            { "uf", order.company.uf_company },
        //                            { "cep", order.company.zip_code_company },
        //                            { "fone", null },
        //                            { "cel", order.company.fone_company },
        //                            { "email", order.company.email_company },
        //                            { "contato", order.company.name_company }
        //                        }
        //                    },
        //                    { "dfe", new JArray(
        //                        new JObject {
        //                            { "cfop", order.cfop },
        //                            { "danfeCte", order.invoice.key_nfe_nf },
        //                            { "nrDoc", order.invoice.number_nf },
        //                            { "serie", order.invoice.serie_nf },
        //                            { "tpDocumento", 2 },
        //                            { "valor", order.invoice.amount_nf },
        //                        })
        //                    },
        //                    { "volume", new JArray(
        //                        new JObject {
        //                            { "altura", null },
        //                            { "comprimento", null },
        //                            { "identificador", null },
        //                            { "largura", null },
        //                            { "peso", null }
        //                        })
        //                    }
        //                };

        //                var response = await _apiCall.PostAsync(order.number, "pedido/incluir", parameter.token, jObject);
        //                await _jadlogRepository.GenerateResponseLog(order.number, response);
        //            }
        //            return true;
        //        }
        //        else
        //            return false;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
    }
}
