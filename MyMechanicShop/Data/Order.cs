using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMechanicShop.Data
{
    public class Order
    {
        public string orderId { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime? shipDate { get; set; }
        public decimal orderTotal { get; set; }
        public string itemName { get; set; }
        public int quantity { get; set; }
        public string orderStatus { get; set; }
    }
}
