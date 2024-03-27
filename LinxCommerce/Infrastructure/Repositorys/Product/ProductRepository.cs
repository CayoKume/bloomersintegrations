using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys.Base;
using System.Data;
using System.Reflection;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys
{
    public class ProductRepository : IProductRepository
    {
        private readonly ILinxCommerceRepositoryBase<Product> _linxCommerceRepositoryBase;

        public ProductRepository(ILinxCommerceRepositoryBase<Product> linxCommerceRepositoryBase) =>
            (_linxCommerceRepositoryBase) = (linxCommerceRepositoryBase);

        public void BulkInsertIntoTableRaw(List<Product> registros, string? database)
        {
            try
            {
                var product = _linxCommerceRepositoryBase.CreateDataTable("Product", new List<string> { "CreatedBy","CreatedDate","IntegrationID","DisplayName","ModifiedBy","ModifiedDate","Name","ProductID","SKU","AcceptanceTermID","BrandID","CatalogItemType","DefinitionID","DisplayAvailability","DisplayPrice","DisplayStockQty","FlagsID","IsDeleted","IsFreeShipping","IsNew","IsSearchable","IsUponRequest","IsVisible","LongDescription","MetaDescription","MetaKeywords","NewFrom","NewTo","PageTitle","PurchasingPolicyID","RatingSetID","SearchKeywords","SendToMarketplace","ShippingRegionsID","ShortDescription","UrlFriendly","UseAcceptanceTerm","VisibleFrom","VisibleTo","WarrantyDescription","lastupdateon" });
                FillDataTable(product, registros, new List<string> { "CreatedBy", "CreatedDate", "IntegrationID", "DisplayName", "ModifiedBy", "ModifiedDate", "Name", "ProductID", "SKU", "AcceptanceTermID", "BrandID", "CatalogItemType", "DefinitionID", "DisplayAvailability", "DisplayPrice", "DisplayStockQty", "FlagsID", "IsDeleted", "IsFreeShipping", "IsNew", "IsSearchable", "IsUponRequest", "IsVisible", "LongDescription", "MetaDescription", "MetaKeywords", "NewFrom", "NewTo", "PageTitle", "PurchasingPolicyID", "RatingSetID", "SearchKeywords", "SendToMarketplace", "ShippingRegionsID", "ShortDescription", "UrlFriendly", "UseAcceptanceTerm", "VisibleFrom", "VisibleTo", "WarrantyDescription", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(product, database, "Product", product.Rows.Count);

                var productMedia = _linxCommerceRepositoryBase.CreateDataTable("ProductMedia", new List<string> { "CreatedDate","AbsolutePath","Extension","Height","MaxHeight","MaxWidth","MediaSizeType","RelativePath","Width","Index","IsDeleted","ProductID","Order","ParentMediaID","Type","VideoTitle","VideoUrl","AssociationPath","lastupdateon" });
                FillDataTable(productMedia, registros, new List<string> { "CreatedDate", "AbsolutePath", "Extension", "Height", "MaxHeight", "MaxWidth", "MediaSizeType", "RelativePath", "Width", "Index", "IsDeleted", "ProductID", "Order", "ParentMediaID", "Type", "VideoTitle", "VideoUrl", "AssociationPath", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(productMedia, database, "ProductMedia", productMedia.Rows.Count);

                var productMetaDataValues = _linxCommerceRepositoryBase.CreateDataTable("ProductMetaDataValues", new List<string> { "DisplayName","FormattedValue","InputType","IntegrationID","PropertyGroup","PropertyMetadataID","PropertyName","SerializedBlobValue","SerializedValue","Value","ProductID","lastupdateon" });
                FillDataTable(productMetaDataValues, registros, new List<string> { "DisplayName", "FormattedValue", "InputType", "IntegrationID", "PropertyGroup", "PropertyMetadataID", "PropertyName", "SerializedBlobValue", "SerializedValue", "Value", "ProductID", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(productMetaDataValues, database, "ProductMetaDataValue", productMetaDataValues.Rows.Count);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetParameters(string tableName)
        {
            string query = $@"SELECT NUMBEROFDAYS FROM [BLOOMERS_LINX].[dbo].[LINXAPIPARAM] WHERE METHOD = '{tableName}'";

            try
            {
                return await _linxCommerceRepositoryBase.GetParameters(tableName, query);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Product>> GetRegistersExists(List<string> productsIds, string? database)
        {
            var productIds = String.Empty;
            for (int i = 0; i < productsIds.Count(); i++)
            {
                if (i == productsIds.Count() - 1)
                    productIds += $"'{productsIds[i]}'";
                else
                    productIds += $"'{productsIds[i]}', ";
            }

            string query = @$"SELECT NAME, PRODUCTID, LONGDESCRIPTION, METADESCRIPTION, METAKEYWORDS, PAGETITLE, SEARCHKEYWORDS, SHORTDESCRIPTION 
                              FROM [{database}].[dbo].[PRODUCT_TRUSTED] (NOLOCK)
                              WHERE PRODUCTID IN ({productIds})";

            try
            {
                var retorno = await _linxCommerceRepositoryBase.GetRegistersExists("PRODUCT_TRUSTED", query);
                return retorno.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void FillDataTable(DataTable dataTable, List<Product> registros, List<string> properties)
        {
            try
            {
                for (int i = 0; i < registros.Count(); i++)
                {
                    if (dataTable.TableName == "Product")
                    {
                        DataRow row = dataTable.NewRow();

                        for (int j = 0; j < properties.Count(); j++)
                        {
                            if (properties[j] == "lastupdateon")
                                row[properties[j]] = DateTime.Now;

                            else if (properties[j] == "DisplayName" || properties[j] == "FlagsID" || properties[j] == "ShippingRegionsID")
                                row[properties[j]] = null;

                            else
                                row[properties[j]] = registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) is not null ?
                                registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) : null;
                        }

                        dataTable.Rows.Add(row);
                    }
                    else if (dataTable.TableName == "ProductMedia")
                    {
                        for (int k = 0; k < registros[i].Medias.Count(); k++)
                        {
                            DataRow row = dataTable.NewRow();

                            for (int j = 0; j < properties.Count(); j++)
                            {
                                if (properties[j] == "lastupdateon")
                                    row[properties[j]] = DateTime.Now;

                                else if (properties[j] == "ProductID")
                                    row[properties[j]] = registros[i].ProductID;

                                else if (properties[j] == "Order")
                                    row[properties[j]] = registros[i].Medias[k].MediaAssociations.Count() > 0 && registros[i].Medias[k].MediaAssociations is not null ? registros[i].Medias[k].MediaAssociations.First().Order : null;

                                else if (properties[j] == "AssociationPath")
                                    row[properties[j]] = registros[i].Medias[k].MediaAssociations.Count() > 0 && registros[i].Medias[k].MediaAssociations is not null ? registros[i].Medias[k].MediaAssociations.First().Path : null;

                                else if (properties[j] == "AbsolutePath" || properties[j] == "Extension" || properties[j] == "Height" || properties[j] == "MaxHeight" || properties[j] == "MaxWidth" || properties[j] == "MediaSizeType" || properties[j] == "RelativePath" || properties[j] == "Width")
                                    row[properties[j]] = registros[i].Medias[k].Image.GetType().GetProperty(properties[j]).GetValue(registros[i].Medias[k].Image) is not null ?
                                    registros[i].Medias[k].Image.GetType().GetProperty(properties[j]).GetValue(registros[i].Medias[k].Image) : null;

                                else if (properties[j] == "VideoTitle" || properties[j] == "VideoUrl")
                                {
                                    var propertie = properties[j].Replace("Video", "");
                                    row[properties[j]] = registros[i].Medias[k].Video.GetType().GetProperty(propertie).GetValue(registros[i].Medias[k].Video) is not null ?
                                    registros[i].Medias[k].Video.GetType().GetProperty(propertie).GetValue(registros[i].Medias[k].Video) : null;
                                }

                                else
                                    row[properties[j]] = registros[i].Medias[k].GetType().GetProperty(properties[j]).GetValue(registros[i].Medias[k]) is not null ?
                                    registros[i].Medias[k].GetType().GetProperty(properties[j]).GetValue(registros[i].Medias[k]) : null;
                            }

                            dataTable.Rows.Add(row);
                        }
                    }
                    else if (dataTable.TableName == "ProductMetaDataValues")
                    {
                        for (int k = 0; k < registros[i].MetadataValues.Count(); k++)
                        {
                            DataRow row = dataTable.NewRow();

                            for (int j = 0; j < properties.Count(); j++)
                            {
                                if (properties[j] == "lastupdateon")
                                    row[properties[j]] = DateTime.Now;

                                else if (properties[j] == "ProductID")
                                    row[properties[j]] = registros[i].ProductID;

                                else
                                    row[properties[j]] = registros[i].MetadataValues[k].GetType().GetProperty(properties[j]).GetValue(registros[i].MetadataValues[k]) is not null ?
                                    registros[i].MetadataValues[k].GetType().GetProperty(properties[j]).GetValue(registros[i].MetadataValues[k]) : null;
                            }

                            dataTable.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
