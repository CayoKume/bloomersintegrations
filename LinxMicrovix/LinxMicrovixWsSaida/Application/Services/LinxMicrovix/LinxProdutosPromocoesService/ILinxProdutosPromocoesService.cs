using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Application.Services.LinxMicrovix
{
    public interface ILinxProdutosPromocoesService<TEntity> : ILinxMicrovixServiceBase<TEntity> where TEntity : class, new()
    {
    }
}
