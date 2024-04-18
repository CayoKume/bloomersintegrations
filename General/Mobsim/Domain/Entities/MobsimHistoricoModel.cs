namespace BloomersGeneralIntegrations.Mobsim.Domain.Entities
{
    public class MobsimHistoricoModel
    {
        public MobsimHistoricoModel()
        {
            
        }

        public Guid IdMobsim { get; set; }
        public string Pedido { get; set; }
        public string CodCliente { get; set; }
        public bool Faturado { get; set; }
        public bool Enviado { get; set; }
        public bool Entregue { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
