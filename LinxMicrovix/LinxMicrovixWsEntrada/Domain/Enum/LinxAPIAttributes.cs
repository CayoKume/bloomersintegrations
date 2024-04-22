using System.ComponentModel;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsEntrada.Domain.Enums
{
    public class LinxAPIAttributes
    {
        public enum TypeEnum
        {
            [Description(@"linx_import")]
            authentication,

            [Description("9A357A08-9C36-43D2-A47B-A987D4967580")]
            key
        }
    }
}
