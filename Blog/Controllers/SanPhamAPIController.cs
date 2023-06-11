using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamAPIController : ControllerBase
    {
        BlogContext db = new BlogContext();
        [HttpGet("{Catid}")]
        public List<Post> GetAllProducts(int Catid)
        {
            var sanPham = db.Posts.Where(x => x.CatId == Catid).ToList();
            return sanPham;
        }
    }
}
