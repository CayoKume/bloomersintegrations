using BloomersMicrovixIntegrations.Saida.Core.Interfaces;
using Microvix.Models;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces
{
    public interface ILinxMovimentoRepository<T1> : IMicrovixSaidaCoreRepository<T1> where T1 : class, new()
    {
        public Task InsereRegistroIndividual(T1 registro, string? tableName, string? db);
        public void InsereRegistroIndividualSync(T1 registro, string? tableName, string? db);
        public IEnumerable<Empresa> GetEmpresasSync();
        public Task<IEnumerable<Empresa>> GetEmpresas();
    }
}
