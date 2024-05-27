using BloomersGeneralIntegrations.AfterSale.Domain.Entities;
using BloomersGeneralIntegrations.AfterSale.Infrastructure.Apis;
using BloomersGeneralIntegrations.AfterSale.Infrastructure.Repositorys;
using System.Text.Json;

namespace BloomersGeneralIntegrations.AfterSale.Application.Services;

public class AfterSaleService : IAfterSaleService
{
    private readonly IAPICall _apiCall;
    private readonly IAfterSaleRepository _afterSaleRepository;

    public AfterSaleService(IAfterSaleRepository afterSaleRepository, IAPICall apiCall) =>
        (_afterSaleRepository, _apiCall) = (afterSaleRepository, apiCall);

    public async Task GetReverses()
    {
        try
        {
            var empresas = await _afterSaleRepository.GetCompanys();
            foreach (var empresa in empresas)
            {
                var reverses = new List<Reverses>();
                var autorization = await _afterSaleRepository.GetBearerToken(empresa.doc_company);
                var response = await _apiCall.GetReversesAsync(autorization.token);
                var page = JsonSerializer.Deserialize<Page>(response);
                reverses.AddRange(page.data);

                if (page.last_page > 1)
                {
                    for (int i = 1; i <= page.last_page; i++)
                    {
                        var responseByPage = await _apiCall.GetReversesByPageAsync(autorization.token, i + 1);
                        var nextPage = JsonSerializer.Deserialize<Page>(responseByPage);
                        reverses.AddRange(nextPage.data);
                    }
                }

                await _afterSaleRepository.InsertIntoTable(reverses);
                await _afterSaleRepository.CallDbProcMerge();
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
