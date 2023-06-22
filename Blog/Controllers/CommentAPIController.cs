using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NuGet.Protocol;
using System.Data;
using System.Globalization;
using System.Xml.Linq;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentAPIController : ControllerBase
    {
        BlogContext db = new BlogContext();
        [HttpGet]
        public IEnumerable<Comment> GetAllComment([FromQuery] int postId)
        {
            var commentList = (from cmt in db.Comments
                               join acc in db.Accounts on cmt.AccountId equals acc.AccountId
                               where cmt.PostId == postId
                               select new Comment
                               {
                                   CommentText = cmt.CommentText,
                                   FullName= acc.FullName,
                                   CommentDate = cmt.CommentDate
                               }).ToList();

            return commentList;
        }


        [HttpPost]
        public bool ThemComment(string cmtText,string fullName, int accountId,int postId)
        {
            try
            {
                int lastCmtId = db.Comments.OrderByDescending(x => x.CommentId).FirstOrDefault()?.CommentId ?? 0;
                int newCmtId = lastCmtId + 1;

                Comment cmt = new Comment();
                cmt.CommentId = newCmtId;
                cmt.CommentText = cmtText;
                cmt.FullName = fullName;
                cmt.CommentDate = DateTime.Now;
                cmt.AccountId = accountId;
                cmt.PostId = postId;
                cmt.ParentId = 0;
                db.Comments.Add(cmt);
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
