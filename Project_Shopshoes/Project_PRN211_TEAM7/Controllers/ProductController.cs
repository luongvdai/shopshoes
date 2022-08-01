using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Project_PRN211_TEAM7.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;

namespace Project_PRN211_TEAM7.Controllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration configuration;

        public ProductController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        PROJECT_PRN211_SHOES_APPContext db = new PROJECT_PRN211_SHOES_APPContext();


        public IActionResult Shop(int Id, int page, string SearchText, int filterPrice) // id của brand
        {
            
            // lay danh sach category      
            ViewBag.Brand = GetAllBrand();
            // lay cac san pham trong brand yeu cau
            if (page <= 0)
            {
                page = 1;
            }
            int PageSize = 3;


            // lay cac san pham trong brand yeu cau, tại Page yêu cầu
            if (page <= 0) 
                page = 1;

            int pageSize = Convert.ToInt32(configuration.GetValue<string>("AppSettings:PageSize"));
            ViewBag.products = getproductsFilterPrice(Id, (page - 1) * PageSize + 1, PageSize, SearchText, filterPrice);
            int TotalProduct = GetNumberOfProductsFilter(Id, SearchText, filterPrice);
            int TotalPage = TotalProduct / PageSize;
            if (TotalProduct % PageSize != 0) TotalPage++;
            ViewBag.searchkey = SearchText;
            ViewData["TotalPage"] = TotalPage;
            return View();
        }

        public List<Brand> GetAllBrand()
        {
            return db.Brands.ToList();
        }


        public List<Product> getproductsFilterPrice(int brandId, int offset, int count, string searchtext, int fitlerPrice)
        {
            List<Product> list = new List<Product>();
            if (searchtext != null)
            {
                list = db.Products.Where(p => p.ProductName.Contains(searchtext)).ToList();
            }
            else
            {

                list = db.Products.ToList();
            }

            if (brandId != 0)
            {
                list = (from b in list
                        where b.BrandId == brandId
                        select b).ToList();

            }

            if (fitlerPrice == 1)
            {
                list = db.Products.OrderByDescending(p => p.Price).ToList();
            }
            else if (fitlerPrice == -1)
            {
                list = db.Products.OrderBy(p => p.Price).ToList();
            }

            return list.Skip(offset - 1).Take(count).ToList();

        }
        
             public int GetNumberOfProductsFilter(int BrandId, string SearchText, int fitlerPrice)
        {
            List<Product> list = new List<Product>();

            if (SearchText != null)
            {
                list = db.Products.Where(p => p.ProductName.Contains(SearchText)).ToList();
            }
            else
            {
                list = db.Products.ToList();
            }
            if (BrandId != 0)
            {
                list = (from b in list
                        where b.BrandId == BrandId
                        select b).ToList();
            }


            if (fitlerPrice == 1)
            {
                list = db.Products.OrderByDescending(p => p.Price).ToList();
            }
            else if (fitlerPrice == -1)
            {
                list = db.Products.OrderBy(p => p.Price).ToList();
            }


            return list.Count();

        }

        public IActionResult ProductDetail(int id)
        {

            var product = db.Products.Find(id);
            
            int b = (int)product.BrandId;
            var sizes = (from s in db.Sizes
                         where s.ProductId == id 
                         select s).ToList();
            var quantity = db.Sizes.Where(item => item.ProductId == id).Sum(i => i.Quantity);
            ViewBag.Sizes = sizes;
            ViewBag.Quantity = quantity;
            ViewBag.Product = product;
            var list = (from pro in db.Products
                       where pro.BrandId == b
                       select pro).ToList();
            foreach (var item in list)
            {
                if(item.ProductId == id)
                {
                    list.Remove(item);
                    break;
                }
            }
            ViewBag.Brand = db.Brands.ToList();
            return View(list);
        }
    }
}
