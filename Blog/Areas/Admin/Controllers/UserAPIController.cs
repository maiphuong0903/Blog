using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

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
                account.Password = GetMD5Hash(password);
                account.RoleId= roleID;
                account.CreateDate = createDate;
                account.Description = description;
                db.Accounts.Add(account);
                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }
        private string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // Mã hóa sang chuỗi hex
                }

                return sb.ToString();
            }
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
                account.Password = GetMD5Hash(password);
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
