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
