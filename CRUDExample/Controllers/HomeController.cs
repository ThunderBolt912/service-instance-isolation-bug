using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
