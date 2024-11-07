using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace NFT_UI.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
           
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
