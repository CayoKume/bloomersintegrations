using System.ComponentModel;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Enums
{
    public class LinxAPIAttributes
    {
        public enum TypeEnum
        {
            [Description("BLOOMERS_LINX")]
            Producao,

            [Description("HOMOLOG_BLOOMERS_LINX")]
            Homologacao,

            [Description(@"linx_b2c")]
            authenticationB2C,

            [Description("34238C99-6C9A-48CC-8BAE-DEDB1F6CC3A0")]
            chaveB2C,

            [Description(@"linx_export")]
            authenticationExport,

            [Description(@"36e92c0e-7fe7-40a7-a0c7-a9bde3b69324")]
            chaveExport,
        }
    }
}
