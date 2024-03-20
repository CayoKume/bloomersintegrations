using BloomersMicrovixIntegrations.Saida.Core.Interfaces;
using Microvix.Models;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces
{
    public interface ILinxProdutosDepositosRepository<T1> : IMicrovixSaidaCoreRepository<T1> where T1 : class, new()
    {
        public Task InsereRegistroIndividual(T1 registro, string? tableName, string? db);
        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db);
        public Task<IEnumerable<Empresa>> GetEmpresas();
        public IEnumerable<Empresa> GetEmpresasSync();
    }
}
