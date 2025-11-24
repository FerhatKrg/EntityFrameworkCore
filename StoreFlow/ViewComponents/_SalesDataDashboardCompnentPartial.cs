using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreFlow.Context;

namespace StoreFlow.ViewComponents
{
    public class _SalesDataDashboardCompnentPartial:ViewComponent
    {
        private readonly StoreContext _context;

        public _SalesDataDashboardCompnentPartial(StoreContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values=_context.Orders.OrderByDescending(z=>z.OrderId).Include(x=>x.Customer).Include(y=>y.Product).Take(5).ToList();
            return View(values);
        }
    }
}
