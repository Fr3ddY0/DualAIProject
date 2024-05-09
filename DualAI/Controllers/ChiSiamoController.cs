using Microsoft.AspNetCore.Mvc;

namespace ChiSiamo.Controllers
{
    public class ChiSiamoController : Controller
    {
        // GET: /<controller>/
        public IActionResult ChiSiamoPage()
        {
            return View();
        }
    }
}
