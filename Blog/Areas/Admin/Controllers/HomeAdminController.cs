using Azure;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Blog.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        BlogContext db = new BlogContext();
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("DanhMucCategories")]   
        public IActionResult DanhMucCategories()
        {
            var lstDMCat = db.Categories.ToList();
            return View(lstDMCat);
        }

        [Route("Split")]
        public int splitId(int id)
        {
            //2 
            int res = id +1;
            return res;
        }

        [Route("ThemDanhMuc")]
        public IActionResult ThemDanhMuc()
        {
            var lastDanhMuc = db.Categories.ToList();
            int lastId = lastDanhMuc.OrderByDescending(x => x.CatId).FirstOrDefault().CatId;
            int newId = splitId(lastId);
            ViewBag.lastId = newId;
            return View();
        }


        [Route("XoaDanhMuc")]
        [HttpGet]
        public IActionResult XoaDanhMuc(int catID)
        {
            db.Remove(db.Categories.Find(catID));
            db.SaveChanges();
            return RedirectToAction("DanhMucCategories");
        }

        [Route("SuaDanhMuc")]
        public IActionResult SuaDanhMuc(int catID)
        {
            var sanPham = db.Categories.Find(catID);
            return View(sanPham);
        }

        //User
        [Route("ListUser")]
        public IActionResult ListUser()
        {
            var lstUser = db.Accounts.ToList();
            return View(lstUser);
        }

        [Route("ThemUser")]
        [HttpGet]
        public IActionResult ThemUser()
        {
            var lastUser = db.Accounts.ToList();
            int lastId = lastUser.OrderByDescending(x => x.AccountId).FirstOrDefault().AccountId;
            int newId = splitId(lastId);
            ViewBag.lastId = newId;
            ViewBag.RoleId = new SelectList(db.Roles.ToList(), "RoleId", "RoleName");
            return View();
        }

        [Route("SuaUser")]
        [HttpGet]
        public IActionResult SuaUser(int accountID)
        {
            var user = db.Accounts.Find(accountID);
            ViewBag.RoleId = new SelectList(db.Roles.ToList(), "RoleId", "RoleName");
            return View(user);
        }

        [Route("XoaUser")]
        [HttpGet]
        public IActionResult XoaUser(int accountID)
        {
            db.Remove(db.Accounts.Find(accountID));
            db.SaveChanges();
            return RedirectToAction("ListUser");
        }

        //Bài viết
        [Route("ListPosts")]
        public IActionResult ListPosts(int? page)
        {
            int pageSize = 3;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var posts = db.Posts.AsNoTracking().OrderBy(x => x.PostId);
            PagedList<Post> lstPosts = new PagedList<Post>(posts, pageNumber, pageSize);
            return View(lstPosts);
        }

        [Route("ThemBaiViet")]
        [HttpGet]
        public IActionResult ThemBaiViet()
        {                    
            ViewBag.AccountId = new SelectList(db.Accounts.ToList(), "AccountId", "FullName");
            ViewBag.CatId = new SelectList(db.Categories.ToList(), "CatId", "CatName");
            return View();
        }

        [Route("ThemBaiViet")]
        [HttpPost]
        public IActionResult ThemBaiViet(IFormFile userfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string filename = userfile.FileName;
                    filename = Path.GetFileName(filename);
                    string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", filename);
                    var stream = new FileStream(uploadfilepath, FileMode.Create);
                    userfile.CopyToAsync(stream);
                    ViewBag.message = "Upload thành công";
                }
                catch (Exception ex)
                {
                    ViewBag.message = "Error:" + ex.Message.ToString();
                }              
                return RedirectToAction("ListPosts");
            }
            
            
            return View();
        }

        [Route("XoaBaiViet")]
        [HttpGet]
        public IActionResult XoaBaiViet(int postsID)
        {
            db.Remove(db.Posts.Find(postsID));
            db.SaveChanges();
            return RedirectToAction("ListPosts");
        }

        [Route("SuaBaiViet")]
        [HttpGet]
        public IActionResult SuaBaiViet(int postsID)
        {
            var posts = db.Posts.Find(postsID);
            ViewBag.AccountId = new SelectList(db.Accounts.ToList(), "AccountId", "FullName");
            ViewBag.CatId = new SelectList(db.Categories.ToList(), "CatId", "CatName");
            return View(posts);
        }

        [Route("SuaBaiViet")]
        [HttpPut]
        public IActionResult SuaBaiViet(IFormFile userfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string filename = userfile.FileName;
                    filename = Path.GetFileName(filename);
                    string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", filename);
                    var stream = new FileStream(uploadfilepath, FileMode.Create);
                    userfile.CopyToAsync(stream);
                    ViewBag.message = "Upload thành công";
                }
                catch (Exception ex)
                {
                    ViewBag.message = "Error:" + ex.Message.ToString();
                }
                return RedirectToAction("ListPosts");
            }
            return View();
        }

        [Route("ChiTietBaiViet")]
        public IActionResult ChiTietBaiViet(int postsID)
        {
            var baiviet = db.Posts.Include(p => p.Cat).SingleOrDefault(x => x.PostId == postsID);
            return View(baiviet);
        }
    }
}
