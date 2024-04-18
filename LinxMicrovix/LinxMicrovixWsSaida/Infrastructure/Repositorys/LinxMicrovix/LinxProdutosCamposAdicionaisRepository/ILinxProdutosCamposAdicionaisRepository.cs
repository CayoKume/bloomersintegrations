using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxProdutosCamposAdicionaisRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxProdutosCamposAdicionais> registros, string tableName, string database);
        public Task<List<LinxProdutosCamposAdicionais>> GetRegistersExistsAsync(List<LinxProdutosCamposAdicionais> registros, string tableName, string database);
        public List<LinxProdutosCamposAdicionais> GetRegistersExistsNotAsync(List<LinxProdutosCamposAdicionais> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(LinxProdutosCamposAdicionais registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxProdutosCamposAdicionais registro, string tableName, string database);

    }
}
