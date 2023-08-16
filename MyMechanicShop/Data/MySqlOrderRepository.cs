using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMechanicShop.Data
{
    public class MySqlOrderRepository : OrderRepository
    {
        private readonly MySqlConnection _connection;

        public MySqlOrderRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public override async Task<List<Order>> GetAllOrdersAsync()
        {
            // Implement the logic to fetch all orders from the database
            await _connection.OpenAsync();

            var orders = new List<Order>();

            var query = "SELECT * FROM orders";
            var command = new MySqlCommand(query, _connection);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    orders.Add(new Order
                    {
                        orderId = reader.GetString(0),
                        orderDate = reader.GetDateTime(1),
                        shipDate = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                        orderTotal = reader.GetDecimal(3),
                        itemName = reader.GetString(4),
                        quantity = reader.GetInt32(5),
                        orderStatus = reader.GetString(6)
                    });
                }
            }

            return orders;
        }

        public override async Task<List<Order>> FindOrdersAsync(string searchQuery)
        {
            // Implement the logic to search for orders in the database
            await _connection.OpenAsync();

            var orders = new List<Order>();

            var query = "SELECT * FROM orders WHERE orderId LIKE @query OR itemName LIKE @query";
            var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@query", "%" + searchQuery + "%");

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    orders.Add(new Order
                    {
                        orderId = reader.GetString(0),
                        orderDate = reader.GetDateTime(1),
                        shipDate = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                        orderTotal = reader.GetDecimal(3),
                        itemName = reader.GetString(4),
                        quantity = reader.GetInt32(5),
                        orderStatus = reader.GetString(6)
                    });
                }
            }

            return orders;
        }
    }
}
