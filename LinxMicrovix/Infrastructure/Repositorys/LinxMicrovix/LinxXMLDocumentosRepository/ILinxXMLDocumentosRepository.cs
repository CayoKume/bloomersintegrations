using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxXMLDocumentosRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxXMLDocumentos> registros, string tableName, string database);
        public Task<List<LinxXMLDocumentos>> GetRegistersExistsAsync(List<LinxXMLDocumentos> registros, string tableName, string database);
        public List<LinxXMLDocumentos> GetRegistersExistsNotAsync(List<LinxXMLDocumentos> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(LinxXMLDocumentos registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxXMLDocumentos registro, string tableName, string database);
        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database);
        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database);
    }
}
