using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMechanicShop.Data
{
    public abstract class ProductRepository : IProductRepository
    {
        protected List<Product> _products; // This will store all products

        public ProductRepository()
        {
            _products = new List<Product>(); // Initialize the list
        }

        public abstract Task<List<Product>> GetAllProductsAsync();
        public abstract Task<List<Product>> FindProductsAsync(string searchQuery);
    }
}
