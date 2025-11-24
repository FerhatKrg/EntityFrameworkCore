using StoreFlow.Entities;

namespace StoreFlow.Models
{
    public class CustomerOrderViewModel
    {
        public string CostumerName{ get; set; }
        public List<Order> Orders{ get; set; }
    }
}
