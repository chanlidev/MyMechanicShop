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
