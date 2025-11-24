using Microsoft.AspNetCore.Mvc;
using StoreFlow.Context;

namespace StoreFlow.ViewComponents
{
    public class _CardStatisticDashboardComponentPartial:ViewComponent
    {
        private readonly StoreContext _context;

        public _CardStatisticDashboardComponentPartial(StoreContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.TotalCustomerCount = _context.Customers.Count();
            ViewBag.TotalCategoryCount=_context.Categories.Count();
            ViewBag.TotalProductCount=_context.Products.Count();
            ViewBag.AvgCustomerBalance =_context.Customers.Average(c => c.CustomerBalance).ToString("0.00");
            ViewBag.TotalOrderCount = _context.Orders.Count();
            ViewBag.SumOrderProductCount = _context.Orders.Sum(x => x.OrderCount);
            return View();
        }
    }
}
