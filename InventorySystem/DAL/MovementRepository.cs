using System.Data.SQLite;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;

namespace InventorySystem.DAL
{
    public class MovementRepository
    {
        public bool CreateMovement(Movement movement)
        {
            try
            {
                using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
                connection.Open();
                
                var command = new SQLiteCommand(
                    @"INSERT INTO Movements (Date, Type, ProductId, Quantity, UnitPrice, TotalAmount, UserId, Notes) 
                      VALUES (@date, @type, @productId, @quantity, @unitPrice, @total, @userId, @notes)",
                    connection);
                
                command.Parameters.AddWithValue("@date", movement.Date);
                command.Parameters.AddWithValue("@type", (int)movement.Type);
                command.Parameters.AddWithValue("@productId", movement.ProductId);
                command.Parameters.AddWithValue("@quantity", movement.Quantity);
                command.Parameters.AddWithValue("@unitPrice", movement.UnitPrice);
                command.Parameters.AddWithValue("@total", movement.TotalAmount);
                command.Parameters.AddWithValue("@userId", movement.UserId);
                command.Parameters.AddWithValue("@notes", movement.Notes ?? (object)DBNull.Value);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public List<Movement> GetMovementsByDateRange(DateTime startDate, DateTime endDate)
        {
            var movements = new List<Movement>();
            
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                @"SELECT m.Id, m.Date, m.Type, m.ProductId, m.Quantity, m.UnitPrice, m.TotalAmount, m.UserId, m.Notes,
                         p.Name as ProductName, u.Name as UserName
                  FROM Movements m
                  INNER JOIN Products p ON m.ProductId = p.Id
                  INNER JOIN Users u ON m.UserId = u.Id
                  WHERE DATE(m.Date) BETWEEN DATE(@startDate) AND DATE(@endDate)
                  ORDER BY m.Date DESC",
                connection);
            
            command.Parameters.AddWithValue("@startDate", startDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@endDate", endDate.ToString("yyyy-MM-dd"));
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var movement = new Movement
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                    Type = (MovementType)reader.GetInt32(reader.GetOrdinal("Type")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                    UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                    Product = new Product { Name = reader.GetString(reader.GetOrdinal("ProductName")) },
                    User = new User { Name = reader.GetString(reader.GetOrdinal("UserName")) }

                };
                
                movements.Add(movement);
            }
            
            return movements;
        }
        
        public List<Movement> GetMovementsByUser(int userId, DateTime? date = null)
        {
            var movements = new List<Movement>();
            
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            string query = @"SELECT m.Id, m.Date, m.Type, m.ProductId, m.Quantity, m.UnitPrice, m.TotalAmount, m.UserId, m.Notes,
                            p.Name as ProductName, u.Name as UserName
                     FROM Movements m
                     INNER JOIN Products p ON m.ProductId = p.Id
                     INNER JOIN Users u ON m.UserId = u.Id
                     WHERE m.UserId = @userId";
            
            if (date.HasValue)
            {
                query += " AND DATE(m.Date) = DATE(@date)";
            }
            
            query += " ORDER BY m.Date DESC";
            
            var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            
            if (date.HasValue)
            {
                command.Parameters.AddWithValue("@date", date.Value.ToString("yyyy-MM-dd"));
            }
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var movement = new Movement
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                    Type = (MovementType)reader.GetInt32(reader.GetOrdinal("Type")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                    UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                    Product = new Product { Name = reader.GetString(reader.GetOrdinal("ProductName")) },
                    User = new User { Name = reader.GetString(reader.GetOrdinal("UserName")) }

                };
                
                movements.Add(movement);
            }
            
            return movements;
        }
    }
}