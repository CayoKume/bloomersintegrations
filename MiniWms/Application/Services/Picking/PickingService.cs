using BloomersMiniWmsIntegrations.Domain.Entities.Picking;
using BloomersMiniWmsIntegrations.Infrastructure.Repositorys;
using Newtonsoft.Json;

namespace BloomersMiniWmsIntegrations.Application.Services
{
    public class PickingService : IPickingService
    {
        private readonly IPickingRepository _pickingRepository;

        public PickingService(IPickingRepository pickingRepository) =>
            (_pickingRepository) = (pickingRepository);

        public async Task<string> GetShippingCompanys()
        {
            var list = await _pickingRepository.GetShippingCompanys();
            return JsonConvert.SerializeObject(list);
        }

        public async Task<string> GetUnpickedOrder(string cnpj_emp, string serie, string nr_pedido)
        {
            var list = await _pickingRepository.GetUnpickedOrder(cnpj_emp, serie, nr_pedido);
            return JsonConvert.SerializeObject(list);
        }

        public async Task<string> GetUnpickedOrders(string cnpj_emp, string serie_pedido, string data_inicial, string data_final)
        {
            var list = await _pickingRepository.GetUnpickedOrders(cnpj_emp, serie_pedido, data_inicial, data_final);
            return JsonConvert.SerializeObject(list);
        }

        public async Task<bool> UpdateRetorno(string nr_pedido, int volumes, string listProdutos)
        {
            var result = await _pickingRepository.UpdateRetorno(nr_pedido, volumes, listProdutos);

            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> UpdateShippingCompany(string nr_pedido, int cod_transportador)
        {
            var result = await _pickingRepository.UpdateShippingCompany(nr_pedido, cod_transportador);

            if (result > 0)
                return true;
            else
                return false;
        }
    }
}
