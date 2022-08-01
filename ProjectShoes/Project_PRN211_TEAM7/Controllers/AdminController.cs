using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_PRN211_TEAM7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using X.PagedList;

namespace Project_PRN211_TEAM7.Controllers
{
    public class AdminController : Controller
    {
        PROJECT_PRN211_SHOES_APPContext db = new PROJECT_PRN211_SHOES_APPContext();
 
        public IActionResult ProductManage(int? page, int id)
        {
 
            if (page == null) page = 1;
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            List<Product> products = null;
            if (id == 0)
            {
                products = db.Products.ToList();
                ViewBag.Brand = db.Brands.ToList();
                ViewBag.brandId = id;
                ViewBag.type = 1;
                return View(products.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                products = db.Products.Where(item => item.BrandId == id).ToList();
                ViewBag.Brand = db.Brands.ToList();
                ViewBag.brandId = id;
                ViewBag.type = 2;
                return View(products.ToPagedList(pageNumber, pageSize));
            }

           

        }

        public IActionResult AddProduct()
        {
            var listBrand = db.Brands.ToList();
            ViewBag.listbrand = listBrand;
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(string name,int brand ,string image ,string price, string discount, string description,string size,string quantity)
        {
            ViewBag.name = name;    
            ViewBag.image = image;
            ViewBag.price = price;  
            ViewBag.discount = discount;
            ViewBag.description = description;
            ViewBag.size = size;
            ViewBag.brand = brand;
            var listBrand = db.Brands.ToList();
            ViewBag.listbrand = listBrand;
            //if (name == null)
            //{
            //    ViewBag.Mess = "Product name cannot be empty";
            //    return View();
            //}

            Product p = new Product();
            double sellPrice = 0;
            double discountPro = 0;
            int sizePro = 0;
            int quantityPro = 0;
            bool check = false;

            DateTime now = DateTime.Now;
            // Xét xem price có phải là số ko
            try
            {
                sellPrice = Double.Parse(price);
            }
            catch (Exception e)
            {
                ViewBag.Mess = "Price must be number";
                return View();
            }
            //Xét xem discount có phải số ko
            try
            {
                discountPro = Double.Parse(discount);
            }
            catch (Exception e)
            {
                ViewBag.Mess = "Discount must be number";
                return View();
            }
            
           
            //Xét xem size có phải số ko
            try
            {
                sizePro = int.Parse(size);
            }
            catch (Exception e)
            {
                ViewBag.Mess = "Size must be number";
                return View();
            }
            
            //Xét xem quantity có phải số ko
            try
            {
                quantityPro = int.Parse(quantity);
            }
            catch (Exception e)
            {
                ViewBag.Mess = "Quantity must be number";
                return View();
            }



            if (sellPrice < 0)
            {
                ViewBag.Mess = "Price must be >= 0";
                return View();
            }
            else if (discountPro < 0 || discountPro > 1)
            {
                ViewBag.Mess = "Discount must be >= 0 and <=1";
                return View();
            }
            else if (sizePro < 0)
            {
                ViewBag.Mess = "size must be > 0";
                return View();
            }else if(quantityPro <= 0)
            {
                ViewBag.Mess = "quantity must be > 0";
                return View();
            }
            else 
            {
                Product tempProduct = db.Products.FirstOrDefault(item => item.ProductName.ToUpper().Equals(name.ToUpper()));
                if (tempProduct != null)
                {
                    List<Size> sizeList = db.Sizes.Where(item => item.ProductId == tempProduct.ProductId).ToList();
                    foreach (Size s in sizeList)
                    {
                        if (s.Size1 == sizePro)
                        {
                            ViewBag.Mess = "Product is valid";
                            check = true;
                            return View();
                        }
                    }
                    if (check == false)
                    {
                        Size sizeOfPro = new Size();
                        sizeOfPro.ProductId = tempProduct.ProductId;
                        sizeOfPro.Size1 = sizePro;
                        sizeOfPro.Quantity = quantityPro;
                        db.Sizes.Add(sizeOfPro);
                        db.SaveChanges();
                        return View();
                    }
                }
                    p.ProductName = name;
                    p.BrandId = brand;
                    p.Price = sellPrice;
                    p.Image = image;
                    p.Description = description;
                    p.Discount = discountPro;
                    p.CreatedDate = now;
                    Size si = new Size();
                    db.Products.Add(p);
                    db.SaveChanges();
                    Product p1 = db.Products.OrderByDescending(item => item.ProductId).FirstOrDefault();
                    si.ProductId = p1.ProductId;
                    si.Size1 = sizePro;
                    si.Quantity = quantityPro;
                    db.Sizes.Add(si);
                    db.SaveChanges();
                    return View();       
            }


        }

        public IActionResult UpdateProduct(int id)
        {
            var brands = db.Brands.ToList();
            var product = db.Products.Find(id);
            var sizes = (from s in db.Sizes
                         where s.ProductId == id
                         select s).ToList();
            var quantity = db.Sizes.Where(item => item.ProductId == id).Sum(i => i.Quantity);
            ViewBag.Sizes = sizes;
            ViewBag.Quantity = quantity;
            ViewBag.brands = brands;
            ViewBag.product = product;
            return View();
        }
        [HttpPost]
        public IActionResult UpdateProduct(int id, string name,int quantity ,int brand, string image, string price, string discount, string description,string createdate, string modifidate)
        {
            
            ViewBag.id = id;
            ViewBag.name = name;
            if (image == null)
            {
                Product product = db.Products.Find(id);
                ViewBag.image = product.Image;
            }
            else
            {
                ViewBag.image  = image;
            }
            
            ViewBag.price = price;
            ViewBag.discount = discount;
            ViewBag.description = description;
            ViewBag.createdate = createdate;
            ViewBag.modifidate = modifidate;
            ViewBag.quantity = quantity;
            ViewBag.brand = brand;
            var brands = db.Brands.ToList();
            ViewBag.brands = brands;
            //if (name == null)
            //{
            //    ViewBag.Mess = "Product name cannot be empty";
            //    return View();
            //}

            
            double sellPrice = 0;
            double discountPro = 0;
            DateTime now = DateTime.Now;
            // Xét xem price có phải là số ko
            try
            {
                sellPrice = Double.Parse(price);
            }
            catch (Exception e)
            {
                ViewBag.Mess = "Price must be number";
                return View();
            }
            //Xét xem discount có phải số ko
            try
            {
                discountPro = Double.Parse(discount);
            }
            catch (Exception e)
            {
                ViewBag.Mess = "Discount must be number";
                return View();
            }



            if (sellPrice < 0)
            {
                ViewBag.Mess = "Price must be >= 0";
                return View();
            }
            else if (discountPro < 0 || discountPro > 1)
            {
                ViewBag.Mess = "Discount must be >= 0 and <=1";
                return View();

            }
            else
            {
                Product p = db.Products.Find(id);
                p.ProductId = id;
                p.ProductName = name;
                p.BrandId = brand;
                p.Price = sellPrice;
                if (image == null)
                {
                    
                    p.Image = p.Image;
                }
                else
                {
                    p.Image = image;
                }
                p.Description = description;
                p.Discount = discountPro;
                p.ModifiedDate = now;
                db.Products.Update(p);
                db.SaveChanges();
                ViewBag.product = p;
                return View();  
            }

            
        }

        public IActionResult SizeManage(int id)
        {
            var sizes = (from s in db.Sizes
                         where s.ProductId == id
                         select s).ToList();
            ViewBag.id = id;
            return View(sizes);
        }

        public IActionResult QuantityChange(int id, int size,int quantity)
        {
            ViewBag.size = size;
            ViewBag.quantity = quantity;
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public IActionResult QuantityChange(int id, int size, string quantity)
        {
            ViewBag.size = size;
            ViewBag.quantity = quantity;
            ViewBag.id = id;
            int quantityPro = 0;
            try
            {
                quantityPro = int.Parse(quantity);
            }
            catch (Exception e)
            {
                ViewBag.Mess = "Quantity must be number!!";
                return View();
            }

            if (quantityPro < 0)
            {
                ViewBag.Mess = "Quantity must be >= 0";
                return View();
            }
            else
            {
                Size sizePro = new Size();
                sizePro.Size1 = size;
                sizePro.ProductId = id;
                sizePro.Quantity = quantityPro;
                Product p = db.Products.SingleOrDefault(item => item.ProductId == id);
                p.ProductId = id;
                DateTime now = DateTime.Now;
                p.ModifiedDate = now;
                db.Sizes.Update(sizePro);
                db.Products.Update(p);
                db.SaveChanges();
                return View();
            } 
        }
        
        public IActionResult DeleteProduct(int id)
        {
          
            
            List<Size> sizes = db.Sizes.Where(item => item.ProductId == id).ToList();

            if (sizes.Count != 0)
            {
                HttpContext.Session.SetString("messdelete", "Không thể xóa product do có xuất hiện trong bảng size");
                return RedirectToAction("ProductManage");
            }
            else
            {
                if (HttpContext.Session.GetString("messdelete") != null)
                {
                    HttpContext.Session.Remove("messdelete");
                }
                var p = db.Products.Find(id);
                db.Products.Remove(p);
                db.SaveChanges();
                return RedirectToAction("ProductManage");
            }


        }

    }
}
