using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxCommerce
{
    public interface IB2CConsultaNFeRepository
    {
        public void BulkInsertIntoTableRaw(List<B2CConsultaNFe> registros, string tableName, string database);
        public Task<List<B2CConsultaNFe>> GetRegistersExistsAsync(List<B2CConsultaNFe> registros, string tableName, string database);
        public List<B2CConsultaNFe> GetRegistersExistsNotAsync(List<B2CConsultaNFe> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(B2CConsultaNFe registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(B2CConsultaNFe registro, string tableName, string database);
    }
}
