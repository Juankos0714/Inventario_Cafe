using System.Data.SQLite;
using InventorySystem.Models.Entities;

namespace InventorySystem.DAL
{
    public class ShiftRepository
    {
        public int StartShift(int userId)
        {
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();

            var command = new SQLiteCommand(
                "INSERT INTO Shifts (UserId, StartTime) VALUES (@userId, @startTime)",
                connection);

            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@startTime", DateTime.Now);

            command.ExecuteNonQuery();

            return (int)connection.LastInsertRowId;
        }

        public bool CloseShift(int shiftId, decimal totalSold, int totalTransactions)
        {
            try
            {
                using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
                connection.Open();

                var command = new SQLiteCommand(
                    "UPDATE Shifts SET EndTime = @endTime, TotalSold = @totalSold, TotalTransactions = @totalTransactions, IsClosed = 1 WHERE Id = @id",
                    connection);

                command.Parameters.AddWithValue("@id", shiftId);
                command.Parameters.AddWithValue("@endTime", DateTime.Now);
                command.Parameters.AddWithValue("@totalSold", totalSold);
                command.Parameters.AddWithValue("@totalTransactions", totalTransactions);

                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }

        public Shift? GetActiveShiftByUser(int userId)
        {
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();

            var command = new SQLiteCommand(
                @"SELECT s.Id, s.UserId, s.StartTime, s.EndTime, s.TotalSold, s.TotalTransactions, s.IsClosed,
                          u.Name as UserName
                  FROM Shifts s
                  INNER JOIN Users u ON s.UserId = u.Id
                  WHERE s.UserId = @userId AND s.IsClosed = 0
                  ORDER BY s.StartTime DESC
                  LIMIT 1",
                connection);

            command.Parameters.AddWithValue("@userId", userId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Shift
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    StartTime = reader.GetDateTime(reader.GetOrdinal("StartTime")),
                    EndTime = reader.IsDBNull(reader.GetOrdinal("EndTime"))
                        ? (DateTime?)null
                        : reader.GetDateTime(reader.GetOrdinal("EndTime")),
                    TotalSold = reader.GetDecimal(reader.GetOrdinal("TotalSold")),
                    TotalTransactions = reader.GetInt32(reader.GetOrdinal("TotalTransactions")),
                    IsClosed = reader.GetInt32(reader.GetOrdinal("IsClosed")) == 1,
                    User = new User { Name = reader["UserName"] as string }
                };
            }

            return null;
        }

        public List<Shift> GetShiftsByDate(DateTime date)
        {
            var shifts = new List<Shift>();

            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();

            var command = new SQLiteCommand(
                @"SELECT s.Id, s.UserId, s.StartTime, s.EndTime, s.TotalSold, s.TotalTransactions, s.IsClosed,
                          u.Name as UserName
                  FROM Shifts s
                  INNER JOIN Users u ON s.UserId = u.Id
                  WHERE DATE(s.StartTime) = DATE(@date)
                  ORDER BY s.StartTime",
                connection);

            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                shifts.Add(new Shift
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    UserId = Convert.ToInt32(reader["UserId"]),
                    StartTime = Convert.ToDateTime(reader["StartTime"]),
                    EndTime = reader["EndTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["EndTime"]),
                    TotalSold = Convert.ToDecimal(reader["TotalSold"]),
                    TotalTransactions = Convert.ToInt32(reader["TotalTransactions"]),
                    IsClosed = Convert.ToInt32(reader["IsClosed"]) == 1,
                    User = new User { Name = reader["UserName"] as string }
                });
            }

            return shifts;
        }

        public Shift? GetShiftById(int shiftId)
        {
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();

            var command = new SQLiteCommand(
                @"SELECT s.Id, s.UserId, s.StartTime, s.EndTime, s.TotalSold, s.TotalTransactions, s.IsClosed,
                          u.Name as UserName
                  FROM Shifts s
                  INNER JOIN Users u ON s.UserId = u.Id
                  WHERE s.Id = @id",
                connection);

            command.Parameters.AddWithValue("@id", shiftId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Shift
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    UserId = Convert.ToInt32(reader["UserId"]),
                    StartTime = Convert.ToDateTime(reader["StartTime"]),
                    EndTime = reader["EndTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["EndTime"]),
                    TotalSold = Convert.ToDecimal(reader["TotalSold"]),
                    TotalTransactions = Convert.ToInt32(reader["TotalTransactions"]),
                    IsClosed = Convert.ToInt32(reader["IsClosed"]) == 1,
                    User = new User { Name = reader["UserName"] as string }
                };
            }

            return null;
        }
    }
}