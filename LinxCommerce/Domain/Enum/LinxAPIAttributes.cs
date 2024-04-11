using System.ComponentModel;

namespace BloomersCommerceIntegrations.LinxCommerce.Domain.Enums
{
    public class LinxAPIAttributes
    {
        public enum TypeEnum
        {
            [Description("BLOOMERS_LINX")]
            Producao,

            [Description("HOMOLOG_BLOOMERS_LINX")]
            Homologacao,

            [Description(@"layer.misha")]
            authentication,

            [Description(@"y5RuoAA")]
            chave,
        }
    }
}
