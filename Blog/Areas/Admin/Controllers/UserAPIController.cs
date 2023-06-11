using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        BlogContext db = new BlogContext();
        [HttpGet]
        public IEnumerable<Account> GetAllUser()
        {
            var user = db.Accounts.ToList();
            return user;
        }
        [HttpDelete]
        public bool DeteleUser(int accountID)
        {
            try
            {
                var account = db.Accounts.Find(accountID);
                if (account == null) { return false; }
                db.Accounts.Remove(account);
                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        [HttpPost]
        public bool AddUser(int accountId, string fullName, string email,string password,int roleID,DateTime createDate,string description)
        {
            try
            {
                Account account = new Account();
                account.AccountId = accountId;
                account.FullName = fullName;
                account.Email = email;
                account.Password = password;
                account.RoleId= roleID;
                account.CreateDate = createDate;
                account.Description = description;
                db.Accounts.Add(account);
                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        [HttpPut]
        public bool EditUser(int accountId, string fullName, string email, string password, int roleID, DateTime createDate, string description)
        {
            try
            {
                Account account = db.Accounts.FirstOrDefault(x => x.AccountId == accountId);
                if (account == null) return false;
                account.AccountId = accountId;
                account.FullName = fullName;
                account.Email = email;
                account.Password = password;
                account.RoleId= roleID;
                account.CreateDate = createDate;
                account.Description = description;
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
