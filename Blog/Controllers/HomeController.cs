using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

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

        public IActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 :page.Value;

            var sp = db.Posts.AsNoTracking().OrderBy(x=>x.PostName);
            PagedList<Post> lst = new PagedList<Post>(sp,pageNumber,pageSize);
            var spFood = db.Posts.Where(x => x.CatId == 1);
            var spTravel = db.Posts.Where(x => x.CatId == 2);
            ViewBag.spFood = spFood;
            ViewBag.spTravel = spTravel;
            return View(lst);
        }
        
        public IActionResult ChiTietSanPham(int masp)
        {
            var sp = db.Posts.SingleOrDefault(x => x.PostId == masp);
            return View(sp);
        }

        [Route("TatCaSP")]
        [HttpGet]
        public IActionResult TatcaSP(int? page)
        {
            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            var sp = db.Posts.AsNoTracking().OrderBy(x => x.PostName);
            PagedList<Post> lst = new PagedList<Post>(sp, pageNumber, pageSize);       
            return View(lst);
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