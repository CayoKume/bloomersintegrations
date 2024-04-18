using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public interface ILinxPedidosCompraService<TEntity> : ILinxMicrovixServiceBase<TEntity> where TEntity : class, new()
    {
    }
}
