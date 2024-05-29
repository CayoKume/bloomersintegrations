using NewBloomersWebApplication.Domain.Entities.DeliveryList;
using NewBloomersWebApplication.Infrastructure.Apis;
using Newtonsoft.Json;
using System.Net.Http;

namespace NewBloomersWebApplication.Application.Services
{
    public class DeliveryListService : IDeliveryListService
    {
        private readonly IAPICall _apiCall;

        public DeliveryListService(IAPICall apiCall) =>
            (_apiCall) = (apiCall);

        public async Task<string> GetDeliveryListToPrint(string fileName)
        {
            var parameters = new Dictionary<string, string>
            {
                { "fileName",  fileName}
            };
            var encodedParameters = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();

            return await _apiCall.GetAsync($"GetDeliveryListToPrint", encodedParameters);
        }

        public async Task<Order?> GetOrderShipped(string nr_pedido, string serie, string cnpj_emp, string transportadora)
        {
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    { "cnpj_emp", cnpj_emp },
                    { "serie", serie },
                    { "nr_pedido", nr_pedido },
                    { "cod_transportadora", transportadora }
                };
                var encodedParameters = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();
                var result = await _apiCall.GetAsync("GetOrderShipped", encodedParameters);

                return System.Text.Json.JsonSerializer.Deserialize<Order>(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Order>?> GetOrdersShipped(string cod_transportadora, string cnpj_emp, string serie_pedido, string data_inicial, string data_final)
        {
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    { "cod_transportadora", cod_transportadora },
                    { "cnpj_emp", cnpj_emp },
                    { "serie", serie_pedido },
                    { "data_inicial", data_inicial },
                    { "data_final", data_final }
                };
                var encodedParameters = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();
                var result = await _apiCall.GetAsync("GetOrdersShipped", encodedParameters);

                return System.Text.Json.JsonSerializer.Deserialize<List<Order>>(result);
            }
            catch
            {
                throw;
            }
        }

        public async Task PrintOrder(List<Order> pedidos)
        {
            try
            {
                var response = await _apiCall.PostAsync($"PrintDeliveryList", JsonConvert.SerializeObject(new { serializePedidosList = pedidos }));
            }
            catch
            {
                throw;
            }
        }
    }
}
