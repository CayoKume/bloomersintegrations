using BloomersGeneralIntegrations.Dootax.Infrastructure.Apis;
using BloomersGeneralIntegrations.Dootax.Infrastructure.Repositorys;

namespace BloomersGeneralIntegrations.Dootax.Application.Services
{
    public class DootaxService : IDootaxService
    {
        private readonly IAPICall _apiCall;
        private readonly IDootaxRepository _dootaxRepository;

        public DootaxService(IDootaxRepository dootaxRepository, IAPICall apiCall) =>
            (_dootaxRepository, _apiCall) = (dootaxRepository, apiCall);

        public async Task EnviaXML()
        {
            try
            {
                var xmls = await _dootaxRepository.GetXMLs();

                if (xmls.Count() > 0)
                {
                    foreach (var xml in xmls)
                    {
                        var result = await _apiCall.PostXmlAsync(xml);

                        if (result != null)
                            await _dootaxRepository.InsertSendXMLOk_Log(xml.CNPJCPF, xml.Documento, xml.Serie, xml.ChaveNfe);
                        else
                            continue;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
