using BloomersMiniWmsIntegrations.Infrastructure.Apis.Labels;
using BloomersMiniWmsIntegrations.Infrastructure.Repositorys;
using Newtonsoft.Json;
using System.Text;

namespace BloomersMiniWmsIntegrations.Application.Services
{
    public class LabelsService : ILabelsService
    {
        private readonly string path = @"C:\printer";
        private readonly string pathLabels = @"C:\printer\labels";
        private readonly string pathOrders = @"C:\printer\orders";
        private readonly IAPICall _apiCall;
        private readonly ILabelsRepository _labelsRepository;

        public LabelsService(ILabelsRepository labelsRepository, IAPICall apiCall) =>
            (_labelsRepository, _apiCall) = (labelsRepository, apiCall);

        public async Task<string> GetLabelToPrint(string fileName)
        {
            try
            {
                string path = @$"C:\printer\labels\{fileName}";

                using (var fileInput = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var memoryStream = new MemoryStream();
                    await fileInput.CopyToAsync(memoryStream);

                    var buffer = memoryStream.ToArray();
                    return Convert.ToBase64String(buffer);
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetOrdersToPrint(string cnpj_emp, string serie, string data_inicial, string data_final)
        {
            var list = await _labelsRepository.GetOrdersToPrint(cnpj_emp, serie, data_inicial, data_final);
            return JsonConvert.SerializeObject(list);
        }

        public async Task<string> GetOrderToPrint(string cnpj_emp, string serie, string nr_pedido)
        {
            var list = await _labelsRepository.GetOrderToPrint(cnpj_emp, serie, nr_pedido);
            return JsonConvert.SerializeObject(list);
        }

        public async Task<bool> SendZPLToAPI(string zpl, string nr_pedido, string volume)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (!Directory.Exists(pathLabels))
                    Directory.CreateDirectory(pathLabels);
                if (!Directory.Exists(pathOrders))
                    Directory.CreateDirectory(pathOrders);

                byte[] encodedZpl = Encoding.UTF8.GetBytes(zpl);

                var request = _apiCall.CallAPI(encodedZpl, pathLabels, nr_pedido, true);

                if (request)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateFlagPrinted(string nr_pedido)
        {
            var result = await _labelsRepository.UpdatePrintedFlag(nr_pedido);
            if (result > 0)
                return true;
            else
                return false;
        }
    }
}
