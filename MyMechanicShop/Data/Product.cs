using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMechanicShop.Data
{
    public class Product
    {
        public int id { get; set; }
        public string imageURL { get; set; }
        public string itemName { get; set; }
        public decimal price { get; set; }
        public int inventory { get; set; }
        public string status { get; set; }
        public int OriginalInventory { get; set; }
    }
}
