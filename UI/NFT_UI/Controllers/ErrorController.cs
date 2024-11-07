using Microsoft.AspNetCore.Mvc;

namespace MultiVerse_UI.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ErrorController(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
