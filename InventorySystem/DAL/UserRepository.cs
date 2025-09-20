using System.Data.SQLite;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;

namespace InventorySystem.DAL
{
    public class UserRepository
    {
        public User? GetUserByName(string name)
        {
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                "SELECT Id, Name, PasswordHash, Role, IsActive, CreatedAt FROM Users WHERE Name = @name AND IsActive = 1",
                connection);
            command.Parameters.AddWithValue("@name", name);
            
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Role = (UserRole)reader.GetInt32(reader.GetOrdinal("Role")),
                    IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) == 1,
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))

                };
            }
            
            return null;
        }
        
        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                "SELECT Id, Name, PasswordHash, Role, IsActive, CreatedAt FROM Users WHERE IsActive = 1",
                connection);
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Role = (UserRole)reader.GetInt32(reader.GetOrdinal("Role")),
                    IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) == 1,
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))

                });
            }
            
            return users;
        }
        
        public bool CreateUser(User user)
        {
            try
            {
                using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
                connection.Open();
                
                var command = new SQLiteCommand(
                    "INSERT INTO Users (Name, PasswordHash, Role, IsActive) VALUES (@name, @password, @role, @active)",
                    connection);
                
                command.Parameters.AddWithValue("@name", user.Name);
                command.Parameters.AddWithValue("@password", user.PasswordHash);
                command.Parameters.AddWithValue("@role", (int)user.Role);
                command.Parameters.AddWithValue("@active", user.IsActive ? 1 : 0);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public bool UpdateUser(User user)
        {
            try
            {
                using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
                connection.Open();
                
                var command = new SQLiteCommand(
                    "UPDATE Users SET Name = @name, Role = @role, IsActive = @active WHERE Id = @id",
                    connection);
                
                command.Parameters.AddWithValue("@id", user.Id);
                command.Parameters.AddWithValue("@name", user.Name);
                command.Parameters.AddWithValue("@role", (int)user.Role);
                command.Parameters.AddWithValue("@active", user.IsActive ? 1 : 0);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public bool UpdatePassword(int userId, string newPasswordHash)
        {
            try
            {
                using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
                connection.Open();
                
                var command = new SQLiteCommand(
                    "UPDATE Users SET PasswordHash = @password WHERE Id = @id",
                    connection);
                
                command.Parameters.AddWithValue("@id", userId);
                command.Parameters.AddWithValue("@password", newPasswordHash);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public User? GetUserById(int id)
        {
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            var command = new SQLiteCommand(
                "SELECT Id, Name, PasswordHash, Role, IsActive, CreatedAt FROM Users WHERE Id = @id",
                connection);
            command.Parameters.AddWithValue("@id", id);
            
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Role = (UserRole)reader.GetInt32(reader.GetOrdinal("Role")),
                    IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) == 1,
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))

                };
            }
            
            return null;
        }
    }
}