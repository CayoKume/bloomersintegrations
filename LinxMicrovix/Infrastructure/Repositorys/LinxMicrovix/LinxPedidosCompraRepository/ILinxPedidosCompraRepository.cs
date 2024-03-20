using BloomersMicrovixIntegrations.Saida.Core.Interfaces;
using Microvix.Models;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces
{
    public interface ILinxPedidosCompraRepository<T1> : IMicrovixSaidaCoreRepository<T1> where T1 : class, new()
    {
        public Task<IEnumerable<Empresa>> GetEmpresas();
        public IEnumerable<Empresa> GetEmpresasSync();
    }
}
