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

            List<OderDetail> lst = db.OderDetails.OrderByDescending(s => s.Quantity).ToList();
            List<OderDetail> sum = new List<OderDetail>();
            for (int i = 0; i < 3; i++)
            {
                sum.Add(lst[i]);
            }
            for (int i = 3; i < lst.Count; i++)
            {
                for (int j = 0; j < sum.Count; j++)
                {
                    if (sum[j].ProductId == lst[i].ProductId)
                    {
                        sum[j].Quantity += lst[i].Quantity;
                    }
                }
            }
            for (int i = 0; i < sum.Count; i++)
            {
                for (int j = 0; j < sum.Count; j++)
                {
                    if (sum[i].Quantity > sum[j].Quantity)
                    {
                        OderDetail temp = sum[i];
                        sum[i] = sum[j];
                        sum[j] = temp;
                    }
                }
            }
            ViewBag.quantity = sum;
            List<Product> lst2 = new List<Product>();
            for (int i = 0; i < 3; i++)
            {
                Product p = db.Products.SingleOrDefault(o => o.ProductId == sum[i].ProductId);
                lst2.Add(p);
            }
            return View(lst2);
        }

    }
}
