using Microsoft.AspNetCore.Mvc;

namespace MVP.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var path = System.IO.Path.Combine("/index.php");

            return File(path, "text/html");
        }
    }
}
