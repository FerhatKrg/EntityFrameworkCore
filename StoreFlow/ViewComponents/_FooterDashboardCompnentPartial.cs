using Microsoft.AspNetCore.Mvc;

namespace StoreFlow.ViewComponents
{
    public class _FooterDashboardCompnentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
