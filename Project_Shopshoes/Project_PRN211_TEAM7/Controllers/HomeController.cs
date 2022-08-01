using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Project_PRN211_TEAM7.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace Project_PRN211_TEAM7.Controllers
{
    public class HomeController : Controller
    {

        PROJECT_PRN211_SHOES_APPContext db = new PROJECT_PRN211_SHOES_APPContext();

        public IActionResult Index()
        {
            ViewBag.Message = HttpContext.Session.GetString("username");
            ViewBag.Brand = db.Brands.ToList();
            return View(db.Products.ToList()) ;
        }

        

       
        

    }
}
