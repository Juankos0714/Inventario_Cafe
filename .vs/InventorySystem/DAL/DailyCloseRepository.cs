using System.Data.SQLite;
using InventorySystem.Models.Entities;

namespace InventorySystem.DAL
{
    public class DailyCloseRepository
    {
        public bool CreateDailyClose(DailyClose dailyClose)
        {
            try
            {
                using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
                connection.Open();
                
                var command = new SQLiteCommand(
                    "INSERT INTO DailyCloses (Date, TotalGeneral, TotalTransactions, GeneratedBy) VALUES (@date, @total, @transactions, @generatedBy)",
                    connection);
                
                command.Parameters.AddWithValue("@date", dailyClose.Date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@total", dailyClose.TotalGeneral);
                command.Parameters.AddWithValue("@transactions", dailyClose.TotalTransactions);
                command.Parameters.AddWithValue("@generatedBy", dailyClose.GeneratedBy);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public DailyClose? GetDailyCloseByDate(DateTime date)
        {
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                @"SELECT dc.Id, dc.Date, dc.TotalGeneral, dc.TotalTransactions, dc.GeneratedBy, dc.CreatedAt,
                         u.Name as GeneratedByName
                  FROM DailyCloses dc
                  INNER JOIN Users u ON dc.GeneratedBy = u.Id
                  WHERE DATE(dc.Date) = DATE(@date)",
                connection);
            
            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
            
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new DailyClose
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                    TotalGeneral = reader.GetDecimal(reader.GetOrdinal("TotalGeneral")),
                    TotalTransactions = reader.GetInt32(reader.GetOrdinal("TotalTransactions")),
                    GeneratedBy = reader.GetInt32(reader.GetOrdinal("GeneratedBy")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    GeneratedByUser = new User { Name = reader.GetString(reader.GetOrdinal("GeneratedByName")) }

                };
            }
            
            return null;
        }
        
        public List<DailyClose> GetDailyClosesByDateRange(DateTime startDate, DateTime endDate)
        {
            var dailyCloses = new List<DailyClose>();
            
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                @"SELECT dc.Id, dc.Date, dc.TotalGeneral, dc.TotalTransactions, dc.GeneratedBy, dc.CreatedAt,
                         u.Name as GeneratedByName
                  FROM DailyCloses dc
                  INNER JOIN Users u ON dc.GeneratedBy = u.Id
                  WHERE DATE(dc.Date) BETWEEN DATE(@startDate) AND DATE(@endDate)
                  ORDER BY dc.Date DESC",
                connection);
            
            command.Parameters.AddWithValue("@startDate", startDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@endDate", endDate.ToString("yyyy-MM-dd"));
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                dailyCloses.Add(new DailyClose
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                    TotalGeneral = reader.GetDecimal(reader.GetOrdinal("TotalGeneral")),
                    TotalTransactions = reader.GetInt32(reader.GetOrdinal("TotalTransactions")),
                    GeneratedBy = reader.GetInt32(reader.GetOrdinal("GeneratedBy")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    GeneratedByUser = new User { Name = reader.GetString(reader.GetOrdinal("GeneratedByName")) }

                });
            }
            
            return dailyCloses;
        }
    }
}