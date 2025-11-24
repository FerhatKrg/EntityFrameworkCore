using Microsoft.AspNetCore.Mvc;
using StoreFlow.Context;

namespace StoreFlow.ViewComponents.StatisticViewComponents
{
    public class _StatisticsWigdetCompnentPartial : ViewComponent
    {
        private readonly StoreContext _context;

        public _StatisticsWigdetCompnentPartial(StoreContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.CategoryCount=_context.Categories.Count();
            ViewBag.ProductMaxPrice=_context.Products.Max(p=>p.ProductPrice);
            ViewBag.ProductMinPrice=_context.Products.Min(p=>p.ProductPrice);
            ViewBag.ProductMaxPriceName=_context.Products.Where(x=>x.ProductPrice==(_context.Products.Max(y=>y.ProductPrice))).Select(z=>z.ProductName).FirstOrDefault();
            ViewBag.ProductMinPriceName=_context.Products.Where(x=>x.ProductPrice==(_context.Products.Min(y=>y.ProductPrice))).Select(z=>z.ProductName).FirstOrDefault();
            ViewBag.TotalProductSumStock = _context.Products.Sum(x => x.ProductStock);
            ViewBag.AvgProductStock= _context.Products.Average(x => x.ProductStock);
            ViewBag.AvgProductPrice= _context.Products.Average(x => x.ProductPrice);

            ViewBag.biggerPriceThen1000Count=_context.Products.Where(x=>x.ProductPrice>1000).Count();
            ViewBag.GetIdIs4ForProductName=_context.Products.Where(x=>x.ProductId==4).Select(y=>y.ProductName).FirstOrDefault();
            ViewBag.StockCountBigger50andSmaller100ProductCount = _context.Products.Where(x => x.ProductStock > 50 && x.ProductStock < 100).Count();

            return View();
        }
    }
}
