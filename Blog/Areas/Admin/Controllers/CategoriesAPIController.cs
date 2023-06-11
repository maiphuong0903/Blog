using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesAPIController : ControllerBase
    {
        BlogContext db = new BlogContext();
        [HttpGet]
        public IEnumerable<Category> GetAllCat()
        {
            var cat = db.Categories.ToList();
            return cat;
        }
        [HttpDelete]
        public bool DeteleDanhMuc(int catID)
        {
            try
            {
                var cat = db.Categories.Find(catID);
                if (cat == null) {return false;}
                db.Categories.Remove(cat);
                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        [HttpPost]
        public bool AddDanhMuc(int catID,string catName,string description)
        {
            try
            {
                Category cat = new Category();
                cat.CatId = catID;
                cat.CatName = catName;
                cat.Description = description;
                db.Categories.Add(cat);
                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        [HttpPut]
        public bool SuaDanhMuc(int catID, string catName, string description)
        {
            try
            {
                Category cat = db.Categories.FirstOrDefault(x=> x.CatId == catID);
                if (cat == null) return false;
                cat.CatId=catID;
                cat.CatName=catName;
                cat.Description=description;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
    }
}
