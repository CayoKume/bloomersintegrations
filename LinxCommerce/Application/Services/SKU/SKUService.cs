using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Enums;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Extensions;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Apis;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys;

namespace BloomersCommerceIntegrations.LinxCommerce.Application.Services
{
    public class SKUService<TEntity> : ISKUService<TEntity> where TEntity : SearchSKUResponse.Root, new()
    {
        private string CHAVE = LinxAPIAttributes.TypeEnum.chave.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authentication.ToName();
        private readonly IAPICall _apiCall;
        private readonly ISKURepository _skuRepository;

        public SKUService(IAPICall apiCall, ISKURepository skuRepository) =>
            (_apiCall, _skuRepository) = (apiCall, skuRepository);

        public async Task IntegraRegistros(string database)
        {
            try
            {
                var days = await _skuRepository.GetParameters("SkuBase");
                var objectRequest = new
                {
                    Page = new { PageIndex = 0, PageSize = 0 },
                    Where = $"(ModifiedDate>=\"{DateTime.Now.AddDays(-days).Date:yyyy-MM-dd}T00:00:00\" && ModifiedDate<=\"{DateTime.Now.Date:yyyy-MM-dd}T23:59:59\")",
                    WhereMetadata = "",
                    OrderBy = ""
                };

                var searchSKUResponse = await _apiCall.PostRequest(objectRequest, "/v1/Catalog/API.svc/web/GetSKU", AUTENTIFICACAO, CHAVE);
                var searchSKUs = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchSKUResponse.Root>(searchSKUResponse);
                var skuInSql = await _skuRepository.GetRegistersExists(searchSKUs.Result.Select(r => r.ProductID).ToList(), database);
                var listSku = new List<SKUs>();

                for (int i = 0; i < skuInSql.Count(); i++)
                {
                    var skuResponse = await _apiCall.PostRequest(skuInSql[i].ProductID, "/v1/Catalog/API.svc/web/GetSKU", AUTENTIFICACAO, CHAVE);
                    var sku = Newtonsoft.Json.JsonConvert.DeserializeObject<SKUs>(skuResponse);
                    listSku.Add(sku);
                }

                _skuRepository.BulkInsertIntoTableRaw(listSku, database);
                _skuRepository.BulkInsertIntoTableRaw(searchSKUs, database);
            }
            catch (Exception ex) when (
                ex.Message.Contains("BulkInsertIntoTableRaw") ||
                ex.Message.Contains("GetParameters") ||
                ex.Message.Contains("GetRegisterExists") ||
                ex.Message.Contains("GetRegistersExists") ||
                ex.Message.Contains("InsereRegistroIndividual") ||
                ex.Message.Contains("CreateDataTable") ||
                ex.Message.Contains("PostRequest") ||
                ex.Message.Contains("CreateClient") ||
                ex.Message.Contains("FillDataTable"))
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($" LinxEcomProduct - IntegraRegistros - Erro ao integrar registros - {ex.Message}");
            }
        }
    }
}
