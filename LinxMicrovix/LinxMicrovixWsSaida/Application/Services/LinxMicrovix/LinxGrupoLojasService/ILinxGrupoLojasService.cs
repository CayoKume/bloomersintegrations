using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public interface ILinxGrupoLojasService<TEntity> : ILinxMicrovixServiceBase<TEntity> where TEntity : class, new()
    {
    }
}
