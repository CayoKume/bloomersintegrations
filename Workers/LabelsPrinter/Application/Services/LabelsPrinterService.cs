using BloomersWorkers.LabelsPrinter.Domain.Entities;
using BloomersWorkers.LabelsPrinter.Infrastructure.Apis;
using BloomersWorkers.LabelsPrinter.Infrastructure.Repositorys;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace BloomersWorkers.LabelsPrinter.Application.Services
{
    public class LabelsPrinterService : ILabelsPrinterService
    {
        private readonly IAPICall _apiCall;
        private readonly IConfiguration _configuration;
        private readonly IGenerateZPLsService _generateZPLsService;
        private readonly ILabelsPrinterRepository _labelsPrinterRepository;
        
        public LabelsPrinterService(ILabelsPrinterRepository labelsPrinterRepository, IGenerateZPLsService generateZPLsService, IConfiguration configuration, IAPICall apiCall)
            => (_labelsPrinterRepository, _generateZPLsService, _configuration,_apiCall) = (labelsPrinterRepository, generateZPLsService, configuration, apiCall);

        public async Task PrintLabels()
        {
            try
            {
                string? path = _configuration.GetSection("ConfigureService").GetSection("LabelsPrinter").GetSection("path").Value;
                string? pathLabels = _configuration.GetSection("ConfigureService").GetSection("LabelsPrinter").GetSection("pathLabels").Value;

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (!Directory.Exists(pathLabels))
                    Directory.CreateDirectory(pathLabels);

                var listOrders = await _labelsPrinterRepository.GetOrders();

                if (listOrders.Count() > 0)
                {
                    foreach (var order in listOrders)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(order._return) && order.shippingCompany.cod_shippingCompany == "7601" || String.IsNullOrEmpty(order._return) && order.shippingCompany.cod_shippingCompany != "7601")
                            {
                                //tratamento ASCII em caracteres especiais
                                order.client.reason_client = RemoveInvalidCharactersForZebra(order.client.reason_client);
                                order.client.address_client = RemoveInvalidCharactersForZebra(order.client.address_client);
                                order.client.neighborhood_client = RemoveInvalidCharactersForZebra(order.client.neighborhood_client);
                                order.client.city_client = RemoveInvalidCharactersForZebra(order.client.city_client);
                                order.company.reason_company = RemoveInvalidCharactersForZebra(order.company.reason_company);
                                order.company.address_company = RemoveInvalidCharactersForZebra(order.company.address_company);
                                order.company.neighborhood_company = RemoveInvalidCharactersForZebra(order.company.neighborhood_company);
                                order.company.city_company = RemoveInvalidCharactersForZebra(order.company.city_company);


                                if (order.shippingCompany.cod_shippingCompany == "7601" && order.shippingCompany.metodo_shippingCompany == "ESTD" || order.shippingCompany.cod_shippingCompany == "7601" && order.shippingCompany.metodo_shippingCompany == "NORM")
                                    order.shippingCompany.metodo_shippingCompany = "STD";
                                    
                                else if (order.shippingCompany.cod_shippingCompany == "7601" && order.shippingCompany.metodo_shippingCompany == "ETUR")
                                    order.shippingCompany.metodo_shippingCompany = "EXP";

                                if (order.client.doc_client.Length == 14)
                                    order.client.doc_client = Convert.ToInt64(order.client.doc_client).ToString(@"00\.000\.000\/0000\-00");

                                if (order.client.doc_client.Length == 11)
                                    order.client.doc_client = Convert.ToInt64(order.client.doc_client).ToString(@"000\.000\.000\-00");

                                if (order.client.state_registration_client != "" && order.client.state_registration_client != "ISENTO")
                                    order.client.state_registration_client = Convert.ToUInt64(order.client.state_registration_client).ToString(@"00\.000\.00\-0");

                                List<byte[]> requests = new List<byte[]>();

                                if (!String.IsNullOrEmpty(order._return) && order.shippingCompany.cod_shippingCompany == "7601")
                                {
                                    var total_infos = JsonConvert.DeserializeObject<Root>(order._return);
                                    for (int i = 0; i < total_infos.retorno.encomendas.First().volumes.Count(); i++)
                                    {
                                        order.awb.Add(total_infos.retorno.encomendas.First().volumes[i].awb);
                                    }
                                    order.rote = total_infos.retorno.encomendas.First().volumes.First().rota;

                                    var awbBodyRequest = _generateZPLsService.GenerateAWBBodyRequest(order);
                                    requests.AddRange(awbBodyRequest);
                                }

                                else if (order.shippingCompany.cod_shippingCompany == "3535")
                                {
                                    var awbAWRBodyRequest = _generateZPLsService.GenerateAWBAWRBodyRequest(order);
                                    requests.AddRange(awbAWRBodyRequest);
                                }
                                var danfeBodyRequest = _generateZPLsService.GenerateDanfeBodyRequest(order);
                                requests.AddRange(danfeBodyRequest);

                                for (int i = 0; i < requests.Count(); i++)
                                {
                                   await _apiCall.SendRequest(requests[i], pathLabels, $"{order.number} - {i + 1}");
                                }

                                for (int i = 0; i < order.zpl.Count(); i++)
                                {
                                    //RawPrinterHelper.SendStringToPrinter("EtiquetasMicrovix", order.zpl[i]);
                                }

                                //await _labelsPrinterRepository.UpdateStatus(order.number);
                            }
                        }
                        catch (Exception ex) when (ex.Message.Contains(" - "))
                        {
                            throw;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(" - ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Errors => {@ex}", ex.Message);
            }
        }

        private string RemoveInvalidCharactersForZebra(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                    return String.Empty;

                return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(text));
            }
            catch (Exception ex)
            {
                throw new Exception($@"RemoveInvalidCharactersForZebra - Erro ao remover caracteres especiais do zpl - {ex.Message}");
            }
        }
    }
}
