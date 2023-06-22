using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Security.Claims;
using System.Xml;
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

        public IActionResult Index()
        {
            var lstbaiviet = db.Posts.ToList();
            var spFood = db.Posts.Where(x => x.CatId == 1);
            var spTravel = db.Posts.Where(x => x.CatId == 2);
            ViewBag.spFood = spFood;
            ViewBag.spTravel = spTravel;
            return View(lstbaiviet);
        }

        [Route("ChiTietBaiViet")]
        [HttpGet]
        public IActionResult ChiTietBaiViet(int masp)
        {
           
            string nameUser = "";

            if (HttpContext.Session.GetString("FullName") != null)
            {
                nameUser = HttpContext.Session.GetString("FullName");
            }
            ViewBag.NameUser = nameUser;
            var baiviet = db.Posts.SingleOrDefault(x => x.PostId == masp);
            return View(baiviet);

        }


        [Route("TatCaSP")]
        [HttpGet]
        public IActionResult TatcaSP(int? page)
        {
            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var sp = db.Posts.AsNoTracking().OrderBy(x => x.PostName);
            PagedList<Post> lst = new PagedList<Post>(sp, pageNumber, pageSize);
            var pageNumbers = new List<int>(lst.PageCount);
            for (int i = 1; i <= lst.PageCount; i++)
            {
                pageNumbers.Add(i);
            }
            ViewBag.PageNumbers = pageNumbers;                                     
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