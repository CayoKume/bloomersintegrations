using BloomersMicrovixIntegrations.LinxMicrovix.Application.Services.Base;

namespace BloomersMicrovixIntegrations.Application.Services.LinxMicrovix
{
    public interface ILinxMovimentoCartoesService<TEntity> : ILinxMicrovixServiceBase<TEntity> where TEntity : class, new()
    {
    }
}
