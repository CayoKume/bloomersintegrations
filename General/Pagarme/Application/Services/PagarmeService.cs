using BloomersGeneralIntegrations.Pagarme.Infrastructure.Apis;
using BloomersGeneralIntegrations.Pagarme.Infrastructure.Repositorys;

namespace BloomersGeneralIntegrations.Pagarme.Application.Services
{
    public class PagarmeService : IPagarmeService
    {
        private readonly IAPICall _apiCall;
        private readonly IPagarmeRepository _pagarmeRepository;

        public PagarmeService(IPagarmeRepository pagarmeRepository, IAPICall apiCall) =>
            (_pagarmeRepository, _apiCall) = (pagarmeRepository, apiCall);

        public async Task GetReceivables()
        {
            try
            {
                var dataInicio = new DateTime(2024, 01, 01);

                for (var dt = dataInicio; dt <= new DateTime(2024, 01, 31); dt.AddDays(1))
                {
                    var recebiveis = await _apiCall.GetAsync(dataInicio.Date.ToString("yyyy-MM-dd"), dt.Date.ToString("yyyy-MM-dd"));
                    await _pagarmeRepository.InsereReceivableInDatabase(recebiveis);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
