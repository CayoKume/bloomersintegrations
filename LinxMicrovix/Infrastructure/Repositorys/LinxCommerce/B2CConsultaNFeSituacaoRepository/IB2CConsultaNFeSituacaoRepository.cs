using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxCommerce
{
    public interface IB2CConsultaNFeSituacaoRepository
    {
        public void BulkInsertIntoTableRaw(List<B2CConsultaNFeSituacao> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task<List<B2CConsultaNFeSituacao>> GetRegistersExistsAsync(List<B2CConsultaNFeSituacao> registros, string tableName, string database);
        public List<B2CConsultaNFeSituacao> GetRegistersExistsNotAsync(List<B2CConsultaNFeSituacao> registros, string tableName, string database);
    }
}
