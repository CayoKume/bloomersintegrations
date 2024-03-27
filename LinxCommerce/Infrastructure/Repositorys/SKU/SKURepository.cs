using BloomersCommerceIntegrations.LinxCommerce.Domain.Entities;
using BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys.Base;
using System.Data;

namespace BloomersCommerceIntegrations.LinxCommerce.Infrastructure.Repositorys
{
    public class SKURepository : ISKURepository
    {
        private readonly ILinxCommerceRepositoryBase<SKUs> _linxCommerceRepositoryBase;

        public SKURepository(ILinxCommerceRepositoryBase<SKUs> linxCommerceRepositoryBase) =>
            _linxCommerceRepositoryBase = linxCommerceRepositoryBase;

        public void BulkInsertIntoTableRaw(List<SKUs> registros, string? database)
        {
            try
            {
                var sku = _linxCommerceRepositoryBase.CreateDataTable("Sku", new List<string> { "UPC", "DisplayCondition", "DefinitionID", "ConditionID", "UnitOfMeasureID", "ManageStock", "MinimumQtyAllowed", "MaximumQtyAllowed", "Preorderable", "PreorderDate", "PreorderLimit", "Backorderable", "BackorderLimit", "PurchasingPolicyID", "WrappingQty", "Weight", "Height", "Width", "Depth", "Cost", "Tax", "IsVisible", "VisibleFrom", "VisibleTo", "IsDeleted", "ProductID", "Name", "SKU", "CreatedDate", "CreatedBy", "ModifiedDate", "ModifiedBy", "IntegrationID", "ParentID", "lastupdateon" });
                FillDataTable(sku, registros, new List<string> { "UPC","DisplayCondition","DefinitionID","ConditionID","UnitOfMeasureID","ManageStock","MinimumQtyAllowed","MaximumQtyAllowed","Preorderable","PreorderDate","PreorderLimit","Backorderable","BackorderLimit","PurchasingPolicyID","WrappingQty","Weight","Height","Width","Depth","Cost","Tax","IsVisible","VisibleFrom","VisibleTo","IsDeleted","ProductID","Name","SKU","CreatedDate","CreatedBy","ModifiedDate","ModifiedBy","IntegrationID","ParentID","lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(sku, database, "Sku", sku.Rows.Count);

                var skuMetaDataValue = _linxCommerceRepositoryBase.CreateDataTable("SkuMetaDataValue", new List<string> { "DisplayName","FormattedValue","InputType","IntegrationID","PropertyGroup","PropertyMetadataID","PropertyName","SerializedBlobValue","SerializedValue","Value","ProductID","lastupdateon" });
                FillDataTable(skuMetaDataValue, registros, new List<string> { "DisplayName", "FormattedValue", "InputType", "IntegrationID", "PropertyGroup", "PropertyMetadataID", "PropertyName", "SerializedBlobValue", "SerializedValue", "Value", "ProductID", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(skuMetaDataValue, database, "SkuMetaDataValue", skuMetaDataValue.Rows.Count);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void BulkInsertIntoTableRaw(SearchSKUResponse.Root registros, string? database)
        {
            try
            {
                var skuBase = _linxCommerceRepositoryBase.CreateDataTable("SkuBase", new List<string> { "CreatedDate","IntegrationID","ModifiedDate","Name","ProductID","SKU","lastupdateon" });
                FillDataTable(skuBase, registros, new List<string> { "CreatedDate", "IntegrationID", "ModifiedDate", "Name", "ProductID", "SKU", "lastupdateon" });
                _linxCommerceRepositoryBase.BulkInsertIntoTableRaw(skuBase, database, "SkuBase", skuBase.Rows.Count);
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

        public async Task<List<SKUs>> GetRegistersExists(List<string> productsIds, string? database)
        {
            var productIds = String.Empty;
            for (int i = 0; i < productsIds.Count(); i++)
            {
                if (i == productsIds.Count() - 1)
                    productIds += $"'{productsIds[i]}'";
                else
                    productIds += $"'{productsIds[i]}', ";
            }

            string query = @$"SELECT A.PRODUCTID FROM [{database}].[dbo].[SKUBASE_TRUSTED] A (NOLOCK)
                              LEFT JOIN [{database}].[dbo].[SKU_TRUSTED] B (NOLOCK) ON A.PRODUCTID = B.PRODUCTID
                              WHERE B.PRODUCTID IS NULL";

            try
            {
                var retorno = await _linxCommerceRepositoryBase.GetRegistersExists("SKUBASE_TRUSTED", query);
                return retorno.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        private static void FillDataTable(DataTable dataTable, List<SKUs> registros, List<string> properties)
        {
            for (int i = 0; i < registros.Count(); i++)
            {
                if (dataTable.TableName == "Sku")
                {
                    DataRow row = dataTable.NewRow();

                    for (int j = 0; j < properties.Count(); j++)
                    {
                        if (properties[j] == "lastupdateon")
                            row[properties[j]] = DateTime.Now;

                        else if (properties[j] == "ParentID")
                            row[properties[j]] = registros[i].ParentsID.Count() > 0 ? registros[i].ParentsID.First() : null;

                        else
                            row[properties[j]] = registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) is not null ?
                            registros[i].GetType().GetProperty(properties[j]).GetValue(registros[i]) : null;
                    }

                    dataTable.Rows.Add(row);
                }
                else if (dataTable.TableName == "SkuMetaDataValue")
                {
                    DataRow row = dataTable.NewRow();

                    for (int k = 0; k < registros[i].MetadataValues.Count(); k++)
                    {
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
                    }

                    dataTable.Rows.Add(row);
                }
            }
        }

        private static void FillDataTable(DataTable dataTable, SearchSKUResponse.Root registros, List<string> properties)
        {
            for (int i = 0; i < registros.Result.Count(); i++)
            {
                if (dataTable.TableName == "SkuBase")
                {
                    DataRow row = dataTable.NewRow();

                    for (int j = 0; j < properties.Count(); j++)
                    {
                        if (properties[j] == "lastupdateon")
                            row[properties[j]] = DateTime.Now;

                        else
                            row[properties[j]] = registros.Result[i].GetType().GetProperty(properties[j]).GetValue(registros.Result[i]) is not null ?
                            registros.Result[i].GetType().GetProperty(properties[j]).GetValue(registros.Result[i]) : null;
                    }

                    dataTable.Rows.Add(row);
                }
            }
        }
    }
}
