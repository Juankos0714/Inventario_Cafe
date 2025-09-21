using System.Data.SQLite;
using InventorySystem.Models.Entities;

namespace InventorySystem.DAL
{
    public class PartialCloseRepository
    {
        public bool CreatePartialClose(PartialClose partialClose)
        {
            try
            {
                using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
                connection.Open();
                
                var command = new SQLiteCommand(
                    "INSERT INTO PartialCloses (CloseDate, CloseTime, CloseType, UserId, TotalSales, TransactionCount) VALUES (@date, @time, @type, @userId, @totalSales, @transactionCount)",
                    connection);
                
                command.Parameters.AddWithValue("@date", partialClose.CloseDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@time", partialClose.CloseTime.ToString(@"hh\:mm\:ss"));
                command.Parameters.AddWithValue("@type", partialClose.CloseType);
                command.Parameters.AddWithValue("@userId", partialClose.UserId);
                command.Parameters.AddWithValue("@totalSales", partialClose.TotalSales);
                command.Parameters.AddWithValue("@transactionCount", partialClose.TransactionCount);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public List<PartialClose> GetPartialClosesByDate(DateTime date)
        {
            var partialCloses = new List<PartialClose>();
            
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                @"SELECT pc.Id, pc.CloseDate, pc.CloseTime, pc.CloseType, pc.UserId, pc.TotalSales, pc.TransactionCount, pc.CreatedAt,
                         u.Name as UserName
                  FROM PartialCloses pc
                  INNER JOIN Users u ON pc.UserId = u.Id
                  WHERE DATE(pc.CloseDate) = DATE(@date)
                  ORDER BY pc.CreatedAt",
                connection);
            
            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                partialCloses.Add(new PartialClose
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    CloseDate = reader.GetDateTime(reader.GetOrdinal("CloseDate")),
                    CloseTime = TimeSpan.Parse(reader.GetString(reader.GetOrdinal("CloseTime"))),
                    CloseType = reader.GetString(reader.GetOrdinal("CloseType")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    TotalSales = reader.GetDecimal(reader.GetOrdinal("TotalSales")),
                    TransactionCount = reader.GetInt32(reader.GetOrdinal("TransactionCount")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    User = new User { Name = reader.GetString(reader.GetOrdinal("UserName")) }

                });
            }
            
            return partialCloses;
        }
        
        public int GetCloseCountForDate(DateTime date)
        {
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                "SELECT COUNT(*) FROM PartialCloses WHERE DATE(CloseDate) = DATE(@date)",
                connection);
            
            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
            
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
}