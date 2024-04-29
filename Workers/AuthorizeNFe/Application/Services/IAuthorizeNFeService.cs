namespace BloomersWorkers.AuthorizeNFe.Application.Services
{
    public interface IAuthorizeNFeService
    {
        public Task AuthorizeNFes();
        public Task AuthorizeNFes(string workerName);
    }
}
