using BloomersWorkers.LabelsPrinter.Domain.Entities;

namespace BloomersWorkers.LabelsPrinter.Application.Services
{
    public interface IGenerateZPLsService
    {
        public List<byte[]> GenerateDanfeBodyRequest(Order invoice);
        public List<byte[]> GenerateAWBBodyRequest(Order invoice);
        public List<byte[]> GenerateAWBAWRBodyRequest(Order pedido);
    }
}
