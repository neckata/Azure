using Microsoft.AspNetCore.Mvc;

namespace AzureWebApplicationTest.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SignOut(string page)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
