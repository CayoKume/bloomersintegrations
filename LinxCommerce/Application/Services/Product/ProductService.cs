﻿using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Enums;
using BloomersCommerceIntegrations.LinxCommerce.Domain.Extensions;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Apis;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys;
using Microsoft.Win32;

namespace BloomersCommerceIntegrations.LinxCommerce.Application.Services
{
    public class ProductService<TEntity> : IProductService<TEntity> where TEntity : SearchProductResponse.Root, new()
    {
        private string CHAVE = LinxAPIAttributes.TypeEnum.chave.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authentication.ToName();
        private readonly IAPICall _apiCall;
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository, IAPICall apiCall) =>
            (_productRepository, _apiCall) = (productRepository, apiCall);

        public async Task IntegraRegistros(string database)
        {
            try
            {
                var days = await _productRepository.GetParameters("LinxEcomProduct");
                var objectRequest = new
                {
                    Page = new { PageIndex = 0, PageSize = 0 },
                    Where = $"(ModifiedDate>=\"{DateTime.Now.AddDays(-days).Date:yyyy-MM-dd}T00:00:00\" && ModifiedDate<=\"{DateTime.Now.Date:yyyy-MM-dd}T12:59:59\")",
                    WhereMetadata = "",
                    OrderBy = "",
                };

                var searchProductsResponse = await _apiCall.PostRequest(objectRequest, "/v1/Catalog/API.svc/web/SearchProduct", AUTENTIFICACAO, CHAVE);
                var searchProducts = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchProductResponse.Root>(searchProductsResponse);
                var productsInSql = await _productRepository.GetRegistersExists(searchProducts.Result.Select(r => r.ProductID).ToList(), database);

                for (int i = 0; i < productsInSql.Count(); i++)
                {
                    var product = searchProducts.Result.Where(r => r.ProductID == productsInSql[i].ProductID).First();
                    if (product.Name == productsInSql[i].Name && product.LongDescription == productsInSql[i].LongDescription &&
                        product.MetaDescription == productsInSql[i].MetaDescription && product.MetaKeywords == productsInSql[i].MetaKeywords &&
                        product.PageTitle == productsInSql[i].PageTitle && product.SearchKeywords == productsInSql[i].SearchKeywords &&
                        product.ShortDescription == productsInSql[i].ShortDescription)
                    {
                        searchProducts.Result.Remove(searchProducts.Result.Where(r => r.ProductID == productsInSql[i].ProductID).First());
                    }
                    else
                        continue;
                }

                if (searchProducts.Result.Count() > 0)
                {
                    var listProducts = new List<Product>();

                    for (int i = 0; i < searchProducts.Result.Count(); i++)
                    {
                        var productResponse = await _apiCall.PostRequest(searchProducts.Result[i].ProductID, "/v1/Catalog/API.svc/web/GetProduct", AUTENTIFICACAO, CHAVE);
                        var product = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(productResponse);
                        listProducts.Add(product);
                    }

                    _productRepository.BulkInsertIntoTableRaw(listProducts, database);
                }
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
