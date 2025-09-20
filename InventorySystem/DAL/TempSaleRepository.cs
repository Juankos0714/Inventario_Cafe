using System.Data.SQLite;
using InventorySystem.Models.Entities;

namespace InventorySystem.DAL
{
    public class TempSaleRepository
    {
        public List<TempSale> GetActiveTempSalesByUser(int userId)
        {
            var tempSales = new List<TempSale>();
            
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                @"SELECT ts.Id, ts.Alias, ts.UserId, ts.CreatedAt, ts.IsActive,
                         u.Name as UserName
                  FROM TempSales ts
                  INNER JOIN Users u ON ts.UserId = u.Id
                  WHERE ts.UserId = @userId AND ts.IsActive = 1
                  ORDER BY ts.CreatedAt DESC",
                connection);
            
            command.Parameters.AddWithValue("@userId", userId);
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var tempSale = new TempSale
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Alias = reader.GetString(reader.GetOrdinal("Alias")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) == 1,
                    User = new User { Name = reader.GetString(reader.GetOrdinal("UserName")) }

                };
                
                tempSale.Items = GetTempSaleItems(tempSale.Id);
                tempSales.Add(tempSale);
            }
            
            return tempSales;
        }
        
        public int CreateTempSale(string alias, int userId)
        {
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                "INSERT INTO TempSales (Alias, UserId) VALUES (@alias, @userId)",
                connection);
            
            command.Parameters.AddWithValue("@alias", alias);
            command.Parameters.AddWithValue("@userId", userId);
            
            command.ExecuteNonQuery();
            return (int)connection.LastInsertRowId;
        }
        
        public bool AddItemToTempSale(int tempSaleId, int productId, int quantity, decimal unitPrice)
        {
            try
            {
                using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
                connection.Open();
                
                // Verificar si el producto ya existe en la venta temporal
                var checkCommand = new SQLiteCommand(
                    "SELECT Id, Quantity FROM TempSaleItems WHERE TempSaleId = @tempSaleId AND ProductId = @productId",
                    connection);
                checkCommand.Parameters.AddWithValue("@tempSaleId", tempSaleId);
                checkCommand.Parameters.AddWithValue("@productId", productId);
                
                using var reader = checkCommand.ExecuteReader();
                if (reader.Read())
                {
                    // Actualizar cantidad existente
                    var existingId = reader.GetInt32(reader.GetOrdinal("Id"));
                    var existingQuantity = reader.GetInt32(reader.GetOrdinal("Quantity"));

                    reader.Close();
                    
                    var updateCommand = new SQLiteCommand(
                        "UPDATE TempSaleItems SET Quantity = @quantity WHERE Id = @id",
                        connection);
                    updateCommand.Parameters.AddWithValue("@quantity", existingQuantity + quantity);
                    updateCommand.Parameters.AddWithValue("@id", existingId);
                    
                    return updateCommand.ExecuteNonQuery() > 0;
                }
                else
                {
                    reader.Close();
                    
                    // Insertar nuevo item
                    var insertCommand = new SQLiteCommand(
                        "INSERT INTO TempSaleItems (TempSaleId, ProductId, Quantity, UnitPrice) VALUES (@tempSaleId, @productId, @quantity, @unitPrice)",
                        connection);
                    insertCommand.Parameters.AddWithValue("@tempSaleId", tempSaleId);
                    insertCommand.Parameters.AddWithValue("@productId", productId);
                    insertCommand.Parameters.AddWithValue("@quantity", quantity);
                    insertCommand.Parameters.AddWithValue("@unitPrice", unitPrice);
                    
                    return insertCommand.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }
        
        public bool RemoveItemFromTempSale(int tempSaleId, int productId)
        {
            try
            {
                using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
                connection.Open();
                
                var command = new SQLiteCommand(
                    "DELETE FROM TempSaleItems WHERE TempSaleId = @tempSaleId AND ProductId = @productId",
                    connection);
                command.Parameters.AddWithValue("@tempSaleId", tempSaleId);
                command.Parameters.AddWithValue("@productId", productId);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public bool DeleteTempSale(int tempSaleId)
        {
            try
            {
                using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
                connection.Open();
                
                using var transaction = connection.BeginTransaction();
                
                // Eliminar items
                var deleteItemsCommand = new SQLiteCommand(
                    "DELETE FROM TempSaleItems WHERE TempSaleId = @tempSaleId",
                    connection, transaction);
                deleteItemsCommand.Parameters.AddWithValue("@tempSaleId", tempSaleId);
                deleteItemsCommand.ExecuteNonQuery();
                
                // Eliminar venta temporal
                var deleteSaleCommand = new SQLiteCommand(
                    "DELETE FROM TempSales WHERE Id = @tempSaleId",
                    connection, transaction);
                deleteSaleCommand.Parameters.AddWithValue("@tempSaleId", tempSaleId);
                deleteSaleCommand.ExecuteNonQuery();
                
                transaction.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        private List<TempSaleItem> GetTempSaleItems(int tempSaleId)
        {
            var items = new List<TempSaleItem>();
            
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                @"SELECT tsi.Id, tsi.TempSaleId, tsi.ProductId, tsi.Quantity, tsi.UnitPrice,
                         p.Name as ProductName, p.Price as ProductPrice
                  FROM TempSaleItems tsi
                  INNER JOIN Products p ON tsi.ProductId = p.Id
                  WHERE tsi.TempSaleId = @tempSaleId",
                connection);
            
            command.Parameters.AddWithValue("@tempSaleId", tempSaleId);
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new TempSaleItem
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    TempSaleId = reader.GetInt32(reader.GetOrdinal("TempSaleId")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                    UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                    Product = new Product
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                        Name = reader.GetString(reader.GetOrdinal("ProductName")),
                        Price = reader.GetDecimal(reader.GetOrdinal("ProductPrice"))
                    }

                });
            }
            
            return items;
        }
    }
}