namespace BloomersCarriersIntegrations.FlashCourier.Domain.Entities
{
    public class FlashCourierRegisterLog
    {
        public string Pedido { get; set; }
        public DateTime DataEnvio { get; set; }
        public string Retorno { get; set; }
        public string StatusEcom { get; set; }
        public string Remetente { get; set; }
        public string StatusFlash { get; set; }
        public string ChaveNFe { get; set; }

    }
}
