using BloomersMicrovixIntegrations.Saida.Core.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces
{
    public interface ILinxProdutosService<T1> : IMicrovixSaidaCoreService<T1> where T1 : class, new()
    {
        public Task<bool> IntegraRegistrosIndividual(string tableName, string procName, string database, string identificador, string cnpj_emp);
        public bool IntegraRegistrosIndividualSync(string tableName, string procName, string database, string identificador, string cnpj_emp);
    }
}
