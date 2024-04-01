namespace BloomersWorkers.LabelsPrinter.Domain.Entities
{
    public class DocumentoFiscal
    {
        public string numero { get; set; }
        public string serie { get; set; }
    }

    public class Encomenda
    {
        public string clienteCodigo { get; set; }
        public string data { get; set; }
        public List<DocumentoFiscal> documentoFiscal { get; set; }
        public string hora { get; set; }
        public string pedido { get; set; }
        public string tipoServico { get; set; }
        public List<Volume> volumes { get; set; }
    }

    public class Retorno
    {
        public List<Encomenda> encomendas { get; set; }
    }

    public class Root
    {
        public Retorno retorno { get; set; }
    }

    public class Volume
    {
        public string awb { get; set; }
        public string codigoBarras { get; set; }
        public string rota { get; set; }
    }
}
