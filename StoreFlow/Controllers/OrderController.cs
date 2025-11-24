using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Context;
using StoreFlow.Entities;
using StoreFlow.Models;

namespace StoreFlow.Controllers
{
    public class OrderController : Controller
    {
        private readonly StoreContext _context;

        public OrderController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult AllStockSmallerThen5()
        {
            bool orderStockCount = _context.Orders.All(o => o.OrderCount <= 5);
            if (orderStockCount)
            {
                ViewBag.Message = "Tüm Sipraişler 5 ten Küçüktür.";
            }
            else
            {
                ViewBag.Message = "Tüm Sipraişler 5 ten Küçük Değildir.";
            }
            return View();
        }

        public IActionResult OrderListByStatus(string status)
        {
            var values = _context.Orders.Where(x => x.Status.Contains(status)).ToList();
            if (!values.Any())
            {
                ViewBag.Message = "Bu Statü İle İlgili Veri Bulunamadı";
            }
            return View(values);
        }

        public IActionResult OrderListSearch(string name, string filterType)
        {
            var valuesAll = _context.Orders.ToList();

            if (filterType == "start")
            {
                var values = _context.Orders.Where(o => o.Status.StartsWith(name)).ToList();
                return View(values);
            }
            else if (filterType == "end")
            {
                var values = _context.Orders.Where(o => o.Status.EndsWith(name)).ToList();
                return View(values);
            }
            return View(valuesAll);

        }

        public async Task<IActionResult> OrderList()
        {
            var values = await _context.Orders.Include(x => x.Product).Include(y => y.Customer).ToListAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            var products = await _context.Products
                .Select(c => new SelectListItem
                {
                    Text = c.ProductName,
                    Value = c.ProductId.ToString()
                }).ToListAsync();
            ViewBag.product = products;

            var customers = await _context.Customers
                .Select(p => new SelectListItem
                {
                    Text = p.CustomerName + " " + p.CustomerSurname,
                    Value = p.CustomerId.ToString()
                }).ToListAsync();
            ViewBag.customers = customers;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            order.Status = "Sipariş Alındı";
            order.OrderDate = DateTime.Now;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderList");
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            var value = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(value);
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            var products = await _context.Products
                .Select(c => new SelectListItem
                {
                    Text = c.ProductName,
                    Value = c.ProductId.ToString()
                }).ToListAsync();
            ViewBag.product = products;
            var customers = await _context.Customers
                .Select(p => new SelectListItem
                {
                    Text = p.CustomerName + " " + p.CustomerSurname,
                    Value = p.CustomerId.ToString()
                }).ToListAsync();
            ViewBag.customers = customers;
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderList");
        }

        public IActionResult OrderListWithCustomerGroup()
        {
            var result=from customer in _context.Customers
                       join order in _context.Orders
                       on customer.CustomerId equals order.CustomerId into orderGroup
                       select new CustomerOrderViewModel
                       {
                           CostumerName= customer.CustomerName,
                            Orders= orderGroup.ToList()
                       };

            return View(result.ToList());
        }
    }
}
