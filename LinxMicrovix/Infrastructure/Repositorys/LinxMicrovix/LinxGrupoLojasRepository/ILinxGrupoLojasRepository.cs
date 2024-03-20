using BloomersMicrovixIntegrations.Saida.Core.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Repositorys.Interfaces
{
    public interface ILinxGrupoLojasRepository<T1> : IMicrovixSaidaCoreRepository<T1> where T1 : class, new()
    {
    }
}
