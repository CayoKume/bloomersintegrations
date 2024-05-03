using NewBloomersWebApplication.Domain.Entities.Picking;
using NewBloomersWebApplication.Infrastructure.Apis;
using System.Text.Json;

namespace NewBloomersWebApplication.Application.Services
{
    public class PickingService : IPickingService
    {
        private readonly IAPICall _apiCall;

        public PickingService(IAPICall apiCall) =>
            (_apiCall) = (apiCall);

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
                var result = await _apiCall.PostAsync($"UpdateRetorno", JsonSerializer.Serialize(pedido));

                return true;
            }
            catch
            {
                throw;
            }
        }

        public Task<bool> UpdateShippingCompany(string nr_pedido, int cod_transportador)
        {
            throw new NotImplementedException();
        }
    }
}
