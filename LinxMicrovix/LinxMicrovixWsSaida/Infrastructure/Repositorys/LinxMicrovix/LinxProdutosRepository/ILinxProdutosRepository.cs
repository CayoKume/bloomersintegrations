using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxProdutosRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxProdutos> registros, string tableName, string database);
        public Task<List<LinxProdutos>> GetRegistersExistsAsync(List<LinxProdutos> registros, string tableName, string database);
        public List<LinxProdutos> GetRegistersExistsNotAsync(List<LinxProdutos> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(LinxProdutos registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxProdutos registro, string tableName, string database);
        public Task CallDbProcMergeAsync(string procName, string tableName, string database);
        public void CallDbProcMergeNotAsync(string procName, string tableName, string database);
    }
}
