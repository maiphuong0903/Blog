using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        BlogContext db = new BlogContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            var sp = db.Posts.ToList();
            var spFood = db.Posts.Where(x => x.CatId == 1);
            var spTravel = db.Posts.Where(x => x.CatId == 2);
            ViewBag.spFood = spFood;
            ViewBag.spTravel = spTravel;
            return View(sp);
        }
        
        public IActionResult ChiTietSanPham(int masp)
        {
            var sp = db.Posts.SingleOrDefault(x => x.PostId == masp);
            return View(sp);
        }

        [Route("TatCaSP")]
        [HttpGet]
        public IActionResult TatcaSP()
        {
            var sp = db.Posts.ToList();
            return View(sp);
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