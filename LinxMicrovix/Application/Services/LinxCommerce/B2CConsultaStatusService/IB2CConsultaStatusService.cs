using BloomersMicrovixIntegrations.LinxMicrovix.Application.Services.Base;

namespace BloomersMicrovixIntegrations.Application.Services.LinxCommerce
{
    public interface IB2CConsultaStatusService<TEntity> : ILinxMicrovixServiceBase<TEntity> where TEntity : class, new()
    {
    }
}
