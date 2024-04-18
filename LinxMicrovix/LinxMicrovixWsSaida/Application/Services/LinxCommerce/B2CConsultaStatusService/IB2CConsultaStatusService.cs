using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxCommerce
{
    public interface IB2CConsultaStatusService<TEntity> : ILinxMicrovixServiceBase<TEntity> where TEntity : class, new()
    {
    }
}
