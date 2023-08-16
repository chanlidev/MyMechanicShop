using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMechanicShop.Data
{
    public abstract class OrderRepository : IOrderRepository
    {
        protected List<Order> _orders; // This will store all orders

        public OrderRepository()
        {
            _orders = new List<Order>(); // Initialize the list
        }

        public abstract Task<List<Order>> GetAllOrdersAsync();
        public abstract Task<List<Order>> FindOrdersAsync(string searchQuery);
    }
}
