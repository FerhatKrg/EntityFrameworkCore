using Microsoft.AspNetCore.Mvc;
using StoreFlow.Context;
using StoreFlow.Entities;
using StoreFlow.Models;

namespace StoreFlow.Controllers
{
    public class CustomerController : Controller
    {
        private readonly StoreContext _context;

        public CustomerController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult CustomerListOrderByCustomerName()
        {
            var Customers = _context.Customers.OrderBy(x=>x.CustomerName).ToList();
            return View(Customers);
        }

        public IActionResult CustomerGetByCity(string city)
        {
            var exist=_context.Customers.Any(x=>x.CustomerCity==city);
            if (exist)
            {
                ViewBag.mesaj = $"{city} şehrinde en az 1 müşteri var";
            }
            else
            {
                ViewBag.mesaj = $"{city} şehrinde Hiç müşteri Yok";
            }
            return View( );
        }

        public IActionResult CustomerListOrderByDescBalance()
        {
            var Customers = _context.Customers.OrderByDescending(x => x.CustomerBalance).ToList();
            return View(Customers);
        }

        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return RedirectToAction("CustomerList");
        }

        public IActionResult DeleteCustomer(int id)
        {
            var value = _context.Customers.Find(id);
            _context.Customers.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("CustomerList");
        }

        [HttpGet]
        public IActionResult UpdateCustomer(int id)
        {
            var value = _context.Customers.Find(id);
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
            return RedirectToAction("CustomerList");
        }

        public IActionResult CustomerListByCity()
        {
            var groupedCustomers=_context.Customers.ToList().GroupBy(c=>c.CustomerCity).ToList();
            return View(groupedCustomers);
        }

        public IActionResult CustomersByCityCount()
        {
            var query=from c in _context.Customers
                      group c by c.CustomerCity into cityGroup
                      select new  CustomerCityGroup
                      {
                          City=cityGroup.Key,
                          CustomerCount=cityGroup.Count()
                      };
            var model=query.ToList();
            return View(model);
        }

        public IActionResult CustomerCityList()
        {
            var Customers = _context.Customers.Select(c => c.CustomerCity).Distinct().ToList();
            return View(Customers);
        }

        public IActionResult ParallelCustomer()
        {
            var Customers = _context.Customers.ToList();
            var resultparallel = Customers.AsParallel().Where(c => c.CustomerCity.StartsWith("A",StringComparison.OrdinalIgnoreCase)).ToList();
            return View(resultparallel);
        }

        public IActionResult CustomerListExceptCityIstanbul()
        {
            var allCustomers = _context.Customers.ToList();
            var istanbulCustomers = _context.Customers.Where(c => c.CustomerCity == "İstanbul").Select(y=>y.CustomerCity).ToList();
            var resultCustomers = allCustomers.ExceptBy(istanbulCustomers,c=>c.CustomerCity).ToList();

             
            return View(resultCustomers);
        }

        public IActionResult CustomerListDefaultIfEmpty()
        {
            var Customers = _context.Customers.Where(c => c.CustomerCity == "Ankara").ToList().DefaultIfEmpty(
                new Customer
                {
                    CustomerId=0,
                    CustomerName = "Böyle bir müşteri yok",
                    CustomerSurname = "-----",
                    CustomerCity = "Ankara",
                    CustomerBalance = 0
                }
                ).ToList();
            return View(Customers);
        }

        public IActionResult CustomerIntersectByCity()
        {
            var vaalues1 = _context.Customers.Where(c => c.CustomerCity == "İstanbul").Select(x => x.CustomerName + " " + x.CustomerSurname).ToList();
           var vaalues2 = _context.Customers.Where(c => c.CustomerCity == "Ankara").Select(x => x.CustomerName + " " + x.CustomerSurname).ToList();
            var result= vaalues1.Intersect(vaalues2).ToList();
            return View(result);
        }

        public IActionResult CustomerCast()
        {
            var Customers = _context.Customers.ToList();
            ViewBag.v = Customers;
            return View();
        }

        public IActionResult CustomerListWithIndex()
        {
            var Customers = _context.Customers.ToList().Select((c, index) => new
            {
                SiraNo = index + 1,
                c.CustomerName,
                c.CustomerSurname,
                c.CustomerCity
            }).ToList();
            return View(Customers);
        }
    }
}
