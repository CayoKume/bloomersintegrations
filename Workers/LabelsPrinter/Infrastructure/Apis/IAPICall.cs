namespace BloomersWorkers.LabelsPrinter.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<bool> SendRequest(byte[] zpl, string path, string nr_pedido);
    }
}
