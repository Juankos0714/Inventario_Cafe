using System.Data.SQLite;
using InventorySystem.Models.Entities;

namespace InventorySystem.DAL
{
    public class ProductRepository
    {
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                "SELECT Id, Name, Category, Price, StockActual, StockMinimo, IsActive, CreatedAt FROM Products WHERE IsActive = 1",
                connection);
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Category = reader.GetString(reader.GetOrdinal("Category")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    StockActual = reader.GetInt32(reader.GetOrdinal("StockActual")),
                    StockMinimo = reader.GetInt32(reader.GetOrdinal("StockMinimo")),
                    IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) == 1,
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))

                });
            }
            
            return products;
        }
        
        public Product? GetProductById(int id)
        {
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                "SELECT Id, Name, Category, Price, StockActual, StockMinimo, IsActive, CreatedAt FROM Products WHERE Id = @id",
                connection);
            command.Parameters.AddWithValue("@id", id);
            
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Product
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Category = reader.GetString(reader.GetOrdinal("Category")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    StockActual = reader.GetInt32(reader.GetOrdinal("StockActual")),
                    StockMinimo = reader.GetInt32(reader.GetOrdinal("StockMinimo")),
                    IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) == 1,
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))

                };
            }
            
            return null;
        }
        
        public bool CreateProduct(Product product)
        {
            try
            {
                using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
                connection.Open();
                
                var command = new SQLiteCommand(
                    "INSERT INTO Products (Name, Category, Price, StockActual, StockMinimo, IsActive) VALUES (@name, @category, @price, @stock, @stockMin, @active)",
                    connection);
                
                command.Parameters.AddWithValue("@name", product.Name);
                command.Parameters.AddWithValue("@category", product.Category);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@stock", product.StockActual);
                command.Parameters.AddWithValue("@stockMin", product.StockMinimo);
                command.Parameters.AddWithValue("@active", product.IsActive ? 1 : 0);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public bool UpdateProduct(Product product)
        {
            try
            {
                using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
                connection.Open();
                
                var command = new SQLiteCommand(
                    "UPDATE Products SET Name = @name, Category = @category, Price = @price, StockActual = @stock, StockMinimo = @stockMin, IsActive = @active WHERE Id = @id",
                    connection);
                
                command.Parameters.AddWithValue("@id", product.Id);
                command.Parameters.AddWithValue("@name", product.Name);
                command.Parameters.AddWithValue("@category", product.Category);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@stock", product.StockActual);
                command.Parameters.AddWithValue("@stockMin", product.StockMinimo);
                command.Parameters.AddWithValue("@active", product.IsActive ? 1 : 0);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public bool UpdateStock(int productId, int newStock)
        {
            try
            {
                using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
                connection.Open();
                
                var command = new SQLiteCommand(
                    "UPDATE Products SET StockActual = @stock WHERE Id = @id",
                    connection);
                
                command.Parameters.AddWithValue("@id", productId);
                command.Parameters.AddWithValue("@stock", newStock);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public List<Product> GetLowStockProducts()
        {
            var products = new List<Product>();
            
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                "SELECT Id, Name, Category, Price, StockActual, StockMinimo, IsActive, CreatedAt FROM Products WHERE IsActive = 1 AND StockActual <= StockMinimo",
                connection);
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Category = reader.GetString(reader.GetOrdinal("Category")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    StockActual = reader.GetInt32(reader.GetOrdinal("StockActual")),
                    StockMinimo = reader.GetInt32(reader.GetOrdinal("StockMinimo")),
                    IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) == 1,
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))

                });
            }
            
            return products;
        }
    }
}