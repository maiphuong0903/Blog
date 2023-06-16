using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Controllers
{
    public class AccessController : Controller
    {
        BlogContext db = new BlogContext();
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("FullName")==null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Login(Account user)
        {
            if (HttpContext.Session.GetString("FullName")==null)
            {
                string hashedPassword = GetMD5Hash(user.Password);

                var u = db.Accounts.FirstOrDefault(x => x.FullName.Equals(user.FullName) && x.Password.Equals(hashedPassword));
                if (u != null && u.RoleId == 1) {
                    HttpContext.Session.SetString("FullName", u.FullName.ToString());
                    return RedirectToAction("Index", "Admin");
                }
                else if(u != null && u.RoleId == 2)
                {
                    HttpContext.Session.SetString("FullName", u.FullName.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ThongBao = "Tài khoản hoặc mật khẩu sai";
                }
            }
            return View();
        }
        

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Account user)
        {
            //Kiểm tra xem fullname đã tồn tại trong cơ sở dữ liệu hay chưa
            bool isFullNameExists = db.Accounts.Any(a => a.FullName == user.FullName);

            // Kiểm tra xem email đã tồn tại trong cơ sở dữ liệu hay chưa
            bool isEmailExists = db.Accounts.Any(a => a.Email == user.Email);

            if (isFullNameExists)
            {
                ViewBag.ErrorFullName = "Tên tài khoản đã tồn tại.";
                return View("Register");
            }
            else if (isEmailExists)
            {
                ViewBag.ErrorEmail = "Email đã tồn tại.";
                return View("Register");
            }          
            else
            {
                // Tìm giá trị AccountId lớn nhất hiện tại
                int maxAccountId = db.Accounts.Max(a => a.AccountId);

                // Tăng giá trị AccountId lên 1
                user.AccountId = maxAccountId + 1;

                user.RoleId = 2;
                user.Description = "Người Dùng";
                user.Password = GetMD5Hash(user.Password);
                db.Accounts.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login", "Access");
            }          
        }



        private string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("FullName");
            return RedirectToAction("Login", "Access");
        }
    }
}
