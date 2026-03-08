using Microsoft.AspNetCore.Mvc;

namespace SmartVideoCallApp.Controllers
{
    public class CallController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TrainModel()
        {
            return View();
        }
    }
}
