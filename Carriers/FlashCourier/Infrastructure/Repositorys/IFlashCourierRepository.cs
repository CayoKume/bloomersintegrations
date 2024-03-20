namespace BloomersCarriersIntegrations.FlashCourier.Infrastructure.Repositorys
{
    public interface IFlashCourierRepository
    {
        public Pedido GetPedidoFaturado(string orderNumber);
        public Task<IEnumerable<Pedido>> GetPedidosFaturados();
        public Task<IEnumerable<FlashCourierRegistroLog>> GetPedidosEnviados();
        public Task GeraLogSucesso(string pedido, string remetenteID, string retorno, string statusFlash, string chaveNFe);
        public Task Update_NB_DATA_COLETA(string dtSla, string codigoCartao);
        public Task Update_NB_PREVISAO_REAL_ENTREGA(string dtSla, string codigoCartao);
        public Task Update_NB_DATA_ENTREGA_REALIZADA(string ocorrencia, string codigoCartao);
        public Task Update_NB_DATA_ULTIMO_STATUS(string ocorrencia, string eventoId, string evento, string codigoCartao);
    }
}
