using AzureWebApplicationTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AzureWebApplicationTest.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly IConfidentialClientApplication _tokenAcquisition;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfidentialClientApplication tokenAcquisition)
        {
            this._tokenAcquisition = tokenAcquisition;
        }

        public async Task<IActionResult> Index()
        {
            IConfidentialClientApplication app;

            app = ConfidentialClientApplicationBuilder.Create("clientId")
                    .WithTenantId("{tenantID}")
                    .WithClientSecret("ClientSecret")
                    .Build();

            string[] scopes = new string[] { "user.read" };
            var accessToken = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            ViewBag.token = accessToken.AccessToken;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
