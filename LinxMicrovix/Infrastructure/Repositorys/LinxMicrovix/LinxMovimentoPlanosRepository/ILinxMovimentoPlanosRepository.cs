using BloomersMicrovixIntegrations.Domain.Entities.Ecommerce;

namespace BloomersMicrovixIntegrations.Infrastructure.Repositorys.LinxMicrovix
{
    public interface ILinxMovimentoPlanosRepository
    {
        public void BulkInsertIntoTableRaw(List<LinxMovimentoPlanos> registros, string tableName, string database);
        public Task<List<LinxMovimentoPlanos>> GetRegistersExistsAsync(List<LinxMovimentoPlanos> registros, string tableName, string database);
        public List<LinxMovimentoPlanos> GetRegistersExistsNotAsync(List<LinxMovimentoPlanos> registros, string tableName, string database);
        public Task<string> GetParametersAsync(string tableName, string database, string parameterCol);
        public string GetParametersNotAsync(string tableName, string database, string parameterCol);
        public Task InsereRegistroIndividualAsync(LinxMovimentoPlanos registro, string tableName, string database);
        public void InsereRegistroIndividualNotAsync(LinxMovimentoPlanos registro, string tableName, string database);
    }
}
