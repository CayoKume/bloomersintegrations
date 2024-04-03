using BloomersWorkers.AuthorizeNFe.Domain.Entities;
using BloomersWorkers.AuthorizeNFe.Infrastructure.Repositorys;
using BloomersWorkers.AuthorizeNFe.Infrastructure.Source.Pages;
using BloomersWorkersCore.Infrastructure.Source.Drivers;
using BloomersWorkersCore.Infrastructure.Source.Pages;
using Microsoft.Extensions.Configuration;

namespace BloomersWorkers.AuthorizeNFe.Application.Services
{
    public class AuthorizeNFeService : IAuthorizeNFeService
    {
        private readonly ILoginPage _loginPage;
        private readonly IHomePage _homePage;
        private readonly IAuthorizeNFePage _authorizeNFePage;
        private readonly IChromeDriver _chromeDriver;
        private readonly IConfiguration _configuration;
        private readonly IAuthorizeNFeRepository _authorizeNFeRepository;

        public AuthorizeNFeService(ILoginPage loginPage, IHomePage homePage, IAuthorizeNFePage authorizeNFePage, IChromeDriver chromeDriver, IConfiguration configuration, IAuthorizeNFeRepository authorizeNFeRepository) =>
            (_loginPage, _homePage, _authorizeNFePage, _chromeDriver, _configuration, _authorizeNFeRepository) = (loginPage, homePage, authorizeNFePage, chromeDriver, configuration, authorizeNFeRepository);

        public async Task AuthorizeNFes()
        {
            try
            {
                string? workerName = _configuration.GetSection("ConfigureService").GetSection("WorkerName").Value;
                var ordersB2C = await _authorizeNFeRepository.GetPendingNFesFromB2CConsultaNFe();
                var ordersVD = await _authorizeNFeRepository.GetPendingNFesFromLinxXMLDocumentos();
                var orders = new List<Order>();
                orders.AddRange(ordersB2C);
                orders.AddRange(ordersVD);

                if (orders.Count() > 0)
                {
                    using (var driver = _chromeDriver.GetChromeDriverInstance())
                    {
                        var wait = _chromeDriver.GetWebDriverWaitInstance(driver);

                        foreach (var order in orders)
                        {
                            var user = await _authorizeNFeRepository.GetMicrovixUser(workerName);
                            _loginPage.InsertLoginAndPassword(user, wait);
                            _loginPage.SelectCompany(order.company.doc_company, user, driver, wait);
                            _homePage.NavigateToNFeScreen(driver, wait);
                            var parentWindowHandle = _authorizeNFePage.NavigateToNFeTab(driver, wait);
                            _authorizeNFePage.SetFilters(order, driver, wait);
                            _authorizeNFePage.GetResults(parentWindowHandle, driver, wait);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string[] subs = ex.Message.Split(" - ");
            }
        }
    }
}
