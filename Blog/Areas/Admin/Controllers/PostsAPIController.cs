using Blog.Models;
using Microsoft.AspNetCore.Http;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Blog.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsAPIController : ControllerBase
    {
        BlogContext db = new BlogContext();
        [HttpGet]
        public IEnumerable<Post> GetAllPosts()
        {
            var posts = db.Posts.ToList();
            return posts;
        }

        [HttpDelete]
        public bool DeteleBaiViet(int postsID)
        {
            try
            {
                var posts = db.Posts.Find(postsID);
                if (posts == null) { return false; }
                db.Posts.Remove(posts);
                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        [HttpPost]
        public bool ThemBaiViet(string postName, string shortContent, string contents, string picture, DateTime createDate, string author, int catId, int accountId)
        {
            try
            {
                int lastPostId = db.Posts.OrderByDescending(x => x.PostId).FirstOrDefault()?.PostId ?? 0;
                int newPostId = lastPostId + 1;
                Post posts = new Post();
                posts.PostId = newPostId;
                posts.PostName = postName;
                posts.ShortContent = shortContent;
                // Remove HTML tags and store only the text content
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(contents);
                string plainTextContent = doc.DocumentNode.InnerText;
                posts.Contents = plainTextContent;
                posts.Picture = picture;
                posts.CreateDate = createDate;
                posts.Author = author;
                posts.CatId = catId;
                posts.AccountId = accountId;
                db.Posts.Add(posts);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        [HttpPut]
        public bool SuaBaiViet(int postsID,string postName, string shortContent, string contents, string picture, DateTime createDate, string author, int catId, int accountId)
        {
            try
            {              
                Post posts = db.Posts.FirstOrDefault(x => x.PostId == postsID);
                if (posts == null) return false;
                posts.PostId = postsID;
                posts.PostName = postName;
                posts.ShortContent = shortContent;
                // Remove HTML tags and store only the text content
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(contents);
                string plainTextContent = doc.DocumentNode.InnerText;
                posts.Contents = plainTextContent;
                posts.Picture = picture;
                posts.CreateDate = createDate;
                posts.Author = author;
                posts.CatId = catId;
                posts.AccountId = accountId;
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
