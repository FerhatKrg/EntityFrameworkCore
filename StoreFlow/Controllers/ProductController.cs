using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Context;
using StoreFlow.Entities;
using StoreFlow.Models;

namespace StoreFlow.Controllers
{
    public class ProductController : Controller
    {
        private readonly StoreContext _context;

        public ProductController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult ProductList()
        {
            var values = _context.Products.Include(x=>x.Category).ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            var categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.CategoryId.ToString()
                }).ToList();
            ViewBag.categories = categories;
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(Product p)
        {
            _context.Products.Add(p);
            _context.SaveChanges();
            return RedirectToAction("ProductList");
        }


        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
             
                _context.Products.Remove(product);
                _context.SaveChanges();
            
            return RedirectToAction("ProductList");
        }

        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {

            var categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.CategoryId.ToString()
                }).ToList();
            ViewBag.categories = categories;
            var product = _context.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product p)
        {
           _context.Products.Update(p);
            _context.SaveChanges();
            return RedirectToAction("ProductList");
        }

        public IActionResult First5ProductList(int id)
        {
            var values=_context.Products.Include(x=>x.Category).Take(5).ToList();
            return View(values);
        }

        public IActionResult Skip4ProductList()
        {
            var values=_context.Products.Include(x=>x.Category).Skip(4).Take(10).ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateProductWithAttach()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateProductWithAttach(Product p)
        {
            var category = new Category()
            {
                CategoryId = 1
            };
           _context.Categories.Attach(category);
            var productValue = new Product()
            {
                ProductName = p.ProductName,
                ProductPrice = p.ProductPrice,
                ProductStock = p.ProductStock,
                Category = category
            };
            _context.Products.Add(productValue);
            _context.SaveChanges();
            return RedirectToAction("ProductList");
        }

        public IActionResult ProductCount(int id)
        {
            var value = _context.Products.LongCount();
            var lastProduct = _context.Products.OrderBy(x=>x.ProductId).Last();
            ViewBag.count = value;
            ViewBag.lastProduct = lastProduct.ProductName;
            return View();
        }

        public IActionResult ProductListWithCategory(int id)
        {

            var query = from c in _context.Categories
                        join p in _context.Products
                        on c.CategoryId equals p.CategoryId
                        select new ProductWithcategoryViewModel
                        {
                            CategoryName = c.CategoryName,
                            ProductName = p.ProductName,
                            ProductStock = p.ProductStock

                        };
             
            return View(query.ToList());
        }


    }
}
