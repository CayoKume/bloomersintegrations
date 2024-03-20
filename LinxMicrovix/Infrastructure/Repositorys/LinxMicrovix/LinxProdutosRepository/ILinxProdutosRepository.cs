using BloomersMicrovixIntegrations.Saida.Core.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces
{
    public interface ILinxProdutosRepository<T1> : IMicrovixSaidaCoreRepository<T1> where T1 : class, new()
    {
        public Task InsereRegistroIndividual(T1 registro, string? tableName, string? db);
        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db);
    }
}
