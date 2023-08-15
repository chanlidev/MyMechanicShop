using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMechanicShop.Data
{
    public class MySqlProductRepository : ProductRepository
    {
        private readonly MySqlConnection _connection;

        public MySqlProductRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public override async Task<List<Product>> GetAllProductsAsync()
        {
            // Implement the logic to fetch all products from the database
            await _connection.OpenAsync();

            var products = new List<Product>();

            var query = "SELECT * FROM products";
            var command = new MySqlCommand(query, _connection);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    products.Add(new Product
                    {
                        id = reader.GetInt32(0),
                        imageURL = reader.GetString(1),
                        itemName = reader.GetString(2),
                        price = reader.GetDecimal(3),
                        inventory = reader.GetInt32(4),
                        status = reader.GetString(5)
                    });
                }
            }

            return products;
        }

        public override async Task<List<Product>> FindProductsAsync(string searchQuery)
        {
            // Implement the logic to search for products in the database
            await _connection.OpenAsync();

            var products = new List<Product>();

            var query = "SELECT * FROM products WHERE id LIKE @query OR itemName LIKE @query";
            var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@query", "%" + searchQuery + "%");

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    products.Add(new Product
                    {
                        id = reader.GetInt32(0),
                        imageURL = reader.GetString(1),
                        itemName = reader.GetString(2),
                        price = reader.GetDecimal(3),
                        inventory = reader.GetInt32(4),
                        status = reader.GetString(5)
                    });
                }
            }

            return products;
        }
    }
}
