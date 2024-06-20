namespace BloomersMiniWmsIntegrations.Application.Services
{
    public interface ILabelsService
    {
        public Task<bool> SendZPLToAPI(string zpl, string nr_pedido, string volume);
        public Task<string> GetLabelToPrint(string fileName);
        public Task<string> GetOrdersToPrint(string cnpj_emp, string serie, string data_inicial, string data_final);
        public Task<string> GetOrderToPrint(string cnpj_emp, string serie, string nr_pedido);
        public Task<string> GetOrdersToPresent(string cnpj_emp, string serie, string nr_pedido);
        public Task<bool> UpdateFlagPrinted(string nr_pedido);
        public Task<string> PrintExchangeCupoun(string serializedOrder);
    }
}
