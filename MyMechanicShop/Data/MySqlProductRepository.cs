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

        // Override method to fetch all products from the database
        public override async Task<List<Product>> GetAllProductsAsync()
        {
            await _connection.OpenAsync();

            var products = new List<Product>();

            // SQL query to retrieve all products
            var query = "SELECT * FROM products";
            var command = new MySqlCommand(query, _connection);

            using (var reader = await command.ExecuteReaderAsync())
            {
                // Loop through the result and create Product objects
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

        // Override method to search for products in the database
        public override async Task<List<Product>> FindProductsAsync(string searchQuery)
        {
            await _connection.OpenAsync();

            var products = new List<Product>();

            // SQL query to search for products based on ID or name
            var query = "SELECT * FROM products WHERE id LIKE @query OR itemName LIKE @query";
            var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@query", "%" + searchQuery + "%");

            using (var reader = await command.ExecuteReaderAsync())
            {
                // Loop through the result and create Product objects
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

        // Method to update product inventory
        public async Task UpdateProductInventoryAsync(int id, int newInventory)
        {
            await _connection.OpenAsync();

            using var transaction = _connection.BeginTransaction();

            try
            {
                var query = "UPDATE products SET inventory = @inventory WHERE id = @id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@inventory", newInventory);
                command.Parameters.AddWithValue("@id", id);
                command.Transaction = transaction;

                // Execute the update command and commit the transaction
                await command.ExecuteNonQueryAsync();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Rollback the transaction if an error occurs
                transaction.Rollback();
                throw new InvalidOperationException("Error updating product inventory.", ex);
            }
            finally
            {
                _connection.Close();
            }
        }

        // Method to update product status
        public async Task UpdateProductStatusAsync(int id, string newStatus)
        {
            await _connection.OpenAsync();

            using var transaction = _connection.BeginTransaction();

            try
            {
                var query = "UPDATE products SET status = @status WHERE id = @id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@status", newStatus);
                command.Parameters.AddWithValue("@id", id);
                command.Transaction = transaction;

                // Execute the update command and commit the transaction
                await command.ExecuteNonQueryAsync();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Rollback the transaction if an error occurs
                transaction.Rollback();
                throw new InvalidOperationException("Error updating product status.", ex);
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
