using BloomersMicrovixIntegrations.LinxMicrovix.Application.Services.Base;

namespace BloomersMicrovixIntegrations.Application.Services.LinxMicrovix
{
    public interface ILinxGrupoLojasService<TEntity> : ILinxMicrovixServiceBase<TEntity> where TEntity : class, new()
    {
    }
}
