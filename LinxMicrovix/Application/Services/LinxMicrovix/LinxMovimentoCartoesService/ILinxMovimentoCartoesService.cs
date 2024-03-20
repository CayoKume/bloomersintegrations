using BloomersMicrovixIntegrations.Saida.Core.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Microvix.Services.Interfaces
{
    public interface ILinxMovimentoCartoesService<T1> : IMicrovixSaidaCoreService<T1> where T1 : class, new()
    {
    }
}
