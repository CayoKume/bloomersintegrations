namespace NewBloomersWebApplication.Infrastructure.Domain.Entities.ExecuteCancellation
{
    public class Order : BloomersIntegrationsCore.Domain.Entities.Order
    {
        public string? buttonText { get; set; }
        public string? buttonClass { get; set; }
        public string? canceled { get; set; }
        public string? id_motivo { get; set; }
        public string? descricao_motivo { get; set; }
    }
}
