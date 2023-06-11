using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class AccessController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
