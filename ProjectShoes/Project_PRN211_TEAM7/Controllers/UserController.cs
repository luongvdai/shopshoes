using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Project_PRN211_TEAM7.Models;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization; 

namespace Project_PRN211_TEAM7.Controllers
{
    public class UserController : Controller
    {
        PROJECT_PRN211_SHOES_APPContext db = new PROJECT_PRN211_SHOES_APPContext();

        public IActionResult Register()
        {
            ViewBag.Brand = db.Brands.ToList();
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(User acc, string re_password)
        {
            ViewBag.Brand = db.Brands.ToList();
            if (acc.Password == null || re_password == null || acc.UserName == null || acc.Address == null || acc.Phone == null)
            {
                return View();
            }
            if (!acc.Password.Equals(re_password))
            {
                ViewBag.password = "Confirmation password does not match";
                return View();
            }
            Regex re = new Regex("[0-9]");
            if (acc.Phone.Length > 8)
            {
                for (int i = 0; i < acc.Phone.Length; i++)
                {
                    if (!re.IsMatch(acc.Phone[i] + ""))
                    {
                        ViewBag.phone = "Please enter valid phone number";
                        return View();
                    }
                }
            }
            else
            {
                ViewBag.phone = "Please enter valid phone number > 8";
                return View();
            }
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.UserName.Equals(acc.UserName));
                if (user != null)
                {
                    ViewBag.user = "This UserName was exist!";
                    return View();
                }
                else
                {
                    acc.Role = "customer";
                    db.Users.Add(acc);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                return RedirectToAction("Register");
            }
        }


        public ActionResult Login()
        {
            ViewBag.Message = HttpContext.Session.GetString("username");

            ViewBag.Error = "";

            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            try
            {
                ViewBag.Error = "";

                // kiem tra trong co so du lieu co username va password chua
                User user = db.Users.Where(m => m.UserName == username).FirstOrDefault();
                if (user != null)
                {
                    if (user.Password.Equals(password))
                    {
                        HttpContext.Session.SetString("username", username);
                        HttpContext.Session.SetString("role", user.Role);
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        ViewBag.Error = "Password not true!";
                    }
                }
                else
                {
                    ViewBag.Error = "Account " + username + " Not exist!";
                }

            }
            catch (System.Exception e)
            {

                System.Console.WriteLine(e);
            }
            return View("Login");
        }

    
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("role");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePass(string oldpass, string newpass, string repass)
        {
            string username = HttpContext.Session.GetString("username");
            User user = (from u in db.Users
                         where u.UserName.Equals(username)
                         select u).ToList().First();

            if (user.Password.Equals(oldpass) == false)
            {
                ViewBag.Er = "oldpass is not correct";

                return View();
            }
            else
            {
                if (repass.Equals(newpass) == false)
                {
                    ViewBag.Er = "Repassword is not correct";
                    return View();
                }
                else
                {
                    user.Password = newpass;
                    db.Users.Update(user);
                    db.SaveChanges();
                    ViewBag.Mess = "Changepass successfully";
                    return View();
                }
            }

        }

        public IActionResult Profile()
        {
            ViewBag.Message = HttpContext.Session.GetString("username");
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            User user = db.Users.FirstOrDefault(u => u.UserName.Equals(username));
            return View(user);
        }

        public IActionResult EditProfile()
        {
            ViewBag.Message = HttpContext.Session.GetString("username");
            string username = HttpContext.Session.GetString("username");
            ViewBag.PhoneEmpty = TempData["PhoneEmpty"];
            ViewBag.AddressEmpty = TempData["AddressEmpty"];
            ViewBag.PhoneValid = TempData["PhoneValid"];

            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            User user = db.Users.FirstOrDefault(u => u.UserName.Equals(username));
            return View(user);
        }
        [HttpPost]
        public IActionResult EditProfile(string phone, string address)
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            User u = db.Users.FirstOrDefault(p => p.UserName == username);
            if (phone == null)
            {
                TempData["PhoneEmpty"] = "Input your phone";
                return RedirectToAction("EditProfile", "User");
            }
            if (address == null)
            {
                TempData["AddressEmpty"] = "Input your address";
                return RedirectToAction("EditProfile", "User");
            }
            Regex re = new Regex("[0-9]");
            if (phone.Length > 8)
            {
                for (int i = 0; i < phone.Length; i++)
                {
                    if (!re.IsMatch(phone[i] + ""))
                    {
                        TempData["PhoneValid"] = "Please enter valid phone number";
                        return RedirectToAction("EditProfile", "User");

                    }
                }
            }
            else
            {
                TempData["PhoneValid"] = "Please enter valid phone number > 8";
                return RedirectToAction("EditProfile", "User");

            }
            u.Phone = phone;
            u.Address = address;
            db.Users.Update(u);
            db.SaveChanges();
            return RedirectToAction("Profile", "User");

        }
    }
}
