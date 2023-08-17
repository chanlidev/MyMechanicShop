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

        // Constructor to initialize the repository with a database connection
        public MySqlOrderRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        // Override method to fetch all orders from the database
        public override async Task<List<Order>> GetAllOrdersAsync()
        {
            await _connection.OpenAsync();

            var orders = new List<Order>();

            // SQL query to retrieve all orders
            var query = "SELECT * FROM orders";
            var command = new MySqlCommand(query, _connection);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    // Create Order objects from database records
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

        // Override method to search for orders in the database
        public override async Task<List<Order>> FindOrdersAsync(string searchQuery)
        {
            await _connection.OpenAsync();

            var orders = new List<Order>();

            // SQL query to search for orders based on ID or item name
            var query = "SELECT * FROM orders WHERE orderId LIKE @query OR itemName LIKE @query";
            var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@query", "%" + searchQuery + "%");

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    // Create Order objects from search results
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

        // Method to update order status
        public async Task UpdateOrderStatusAsync(string orderId, string newOrderStatus)
        {
            await _connection.OpenAsync();

            using var transaction = _connection.BeginTransaction();

            try
            {
                // Update order status in the database
                var query = "UPDATE orders SET orderStatus = @orderStatus WHERE orderId = @orderId";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@orderStatus", newOrderStatus);
                command.Parameters.AddWithValue("@orderId", orderId);
                command.Transaction = transaction;

                await command.ExecuteNonQueryAsync();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Error updating order status.", ex);
            }
            finally
            {
                _connection.Close();
            }
        }

    }
}
