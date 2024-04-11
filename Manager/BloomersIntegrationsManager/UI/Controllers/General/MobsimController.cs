using Microsoft.AspNetCore.Mvc;

namespace BloomersIntegrationsManager.UI.Controllers.General
{
    public class MobsimController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
