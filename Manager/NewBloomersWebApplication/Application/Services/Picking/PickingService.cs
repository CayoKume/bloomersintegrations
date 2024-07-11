using BloomersIntegrationsCore.Domain.Entities;
using NewBloomersWebApplication.Domain.Entities.Picking;
using NewBloomersWebApplication.Infrastructure.Apis;
using System.Text.Json;
using Order = NewBloomersWebApplication.Domain.Entities.Picking.Order;

namespace NewBloomersWebApplication.Application.Services
{
    public class PickingService : IPickingService
    {
        private readonly IAPICall _apiCall;

        public PickingService(IAPICall apiCall) =>
            (_apiCall) = (apiCall);

        public async Task<List<ShippingCompany>?> GetShippingCompanys()
        {
            try
            {
                var result = await _apiCall.GetAsync("GetShippingCompanys");

                return JsonSerializer.Deserialize<List<ShippingCompany>>(result);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Order?> GetUnpickedOrder(string cnpj_emp, string serie, string nr_pedido)
        {
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    { "cnpj_emp", cnpj_emp },
                    { "serie", serie },
                    { "nr_pedido", nr_pedido }
                };
                var encodedParameters = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();
                var result = await _apiCall.GetAsync("GetUnpickedOrder", encodedParameters);

                return JsonSerializer.Deserialize<Order>(result);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Order>?> GetUnpickedOrders(string cnpj_emp, string serie_pedido, string data_inicial, string data_final)
        {
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    { "cnpj_emp", cnpj_emp },
                    { "serie", serie_pedido },
                    { "data_inicial", data_inicial },
                    { "data_final", data_final }
                };
                var encodedParameters = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();
                var result = await _apiCall.GetAsync("GetUnpickedOrders", encodedParameters);

                return JsonSerializer.Deserialize<List<Order>>(result);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateRetorno(Order pedido)
        {
            try
            {
                var result = await _apiCall.PutAsync($"UpdateRetorno", JsonSerializer.Serialize(new { nr_pedido = pedido.number, volumes = pedido.volumes, itens = pedido.itens }));

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateShippingCompany(string nr_pedido, int cod_transportador)
        {
            try
            {
                var result = await _apiCall.PutAsync($"UpdateShippingCompany", JsonSerializer.Serialize(new { orderNumber = nr_pedido, cod_shippingCompany = cod_transportador } ));

                return true;
            }
            catch
            {
                throw;
            }
        }

        public Task PrintCoupons(Order pedido)
        {
            throw new NotImplementedException();
        }

        public async Task<string> PrintCoupon(string cnpj_emp, string serie, string nr_pedido)
        {
            var parameters = new Dictionary<string, string>
            {
                { "cnpj_emp", cnpj_emp },
                { "serie", serie },
                { "nr_pedido", nr_pedido }
            };
            var encodedParameters = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();
            var result = await _apiCall.GetAsync("GetUnpickedOrderToPrint", encodedParameters);
            var pedido = JsonSerializer.Deserialize<OrderToPrint>(result);

            return await _apiCall.PostAsync($"PrintOrderToCupoun", JsonSerializer.Serialize(new { serializePedido = pedido }));
        }
    }
}
