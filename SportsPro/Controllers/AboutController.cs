using Microsoft.AspNetCore.Mvc;

namespace SportsPro.Controllers
{
    [Route("about")]

    public class AboutController : Controller
    {
        [HttpGet("")]

        public IActionResult Index()
        {
            return View();
        }
    }
}
