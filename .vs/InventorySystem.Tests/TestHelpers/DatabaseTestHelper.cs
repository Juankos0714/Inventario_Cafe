using System.Data.SQLite;
using InventorySystem.DAL;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;
using BCrypt.Net;

namespace InventorySystem.Tests.TestHelpers
{
    public class DatabaseTestHelper : IDisposable
    {
        private readonly string _connectionString;
        private readonly SQLiteConnection _connection;

        public DatabaseTestHelper()
        {
            _connectionString = "Data Source=:memory:;Version=3;";
            _connection = new SQLiteConnection(_connectionString);
            _connection.Open();
            InitializeTestDatabase();
        }

        public string ConnectionString => _connectionString;

        private void InitializeTestDatabase()
        {
            var commands = new[]
            {
                @"CREATE TABLE Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    Role INTEGER NOT NULL,
                    IsActive INTEGER NOT NULL DEFAULT 1,
                    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                )",
                
                @"CREATE TABLE Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Category TEXT NOT NULL,
                    Price DECIMAL(10,2) NOT NULL,
                    StockActual INTEGER NOT NULL DEFAULT 0,
                    StockMinimo INTEGER NOT NULL DEFAULT 0,
                    IsActive INTEGER NOT NULL DEFAULT 1,
                    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                )",
                
                @"CREATE TABLE Movements (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    Type INTEGER NOT NULL,
                    ProductId INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL,
                    UnitPrice DECIMAL(10,2) NOT NULL,
                    TotalAmount DECIMAL(10,2) NOT NULL,
                    UserId INTEGER NOT NULL,
                    Notes TEXT,
                    FOREIGN KEY (ProductId) REFERENCES Products(Id),
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                )",
                
                @"CREATE TABLE Shifts (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    StartTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    EndTime DATETIME,
                    TotalSold DECIMAL(10,2) NOT NULL DEFAULT 0,
                    TotalTransactions INTEGER NOT NULL DEFAULT 0,
                    IsClosed INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                )",
                
                @"CREATE TABLE DailyCloses (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date DATE NOT NULL,
                    TotalGeneral DECIMAL(10,2) NOT NULL,
                    TotalTransactions INTEGER NOT NULL,
                    GeneratedBy INTEGER NOT NULL,
                    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (GeneratedBy) REFERENCES Users(Id)
                )"
            };

            foreach (var commandText in commands)
            {
                using var command = new SQLiteCommand(commandText, _connection);
                command.ExecuteNonQuery();
            }
        }

        public void SeedTestData()
        {
            // Crear usuarios de prueba
            CreateTestUser("admin", "admin123", UserRole.Admin);
            CreateTestUser("vendedor1", "pass123", UserRole.Vendedor);
            CreateTestUser("contador1", "pass123", UserRole.Contador);

            // Crear productos de prueba
            CreateTestProduct("Café Expreso", "Bebidas", 3000, 50, 10);
            CreateTestProduct("Capuchino", "Bebidas", 5000, 30, 5);
            CreateTestProduct("Sandwich", "Comida", 8500, 20, 3);
            CreateTestProduct("Producto Sin Stock", "Test", 1000, 0, 5);
        }

        private void CreateTestUser(string name, string password, UserRole role)
        {
            var command = new SQLiteCommand(
                "INSERT INTO Users (Name, PasswordHash, Role) VALUES (@name, @password, @role)",
                _connection);
            
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(password));
            command.Parameters.AddWithValue("@role", (int)role);
            
            command.ExecuteNonQuery();
        }

        private void CreateTestProduct(string name, string category, decimal price, int stock, int minStock)
        {
            var command = new SQLiteCommand(
                "INSERT INTO Products (Name, Category, Price, StockActual, StockMinimo) VALUES (@name, @category, @price, @stock, @minStock)",
                _connection);
            
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@category", category);
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@stock", stock);
            command.Parameters.AddWithValue("@minStock", minStock);
            
            command.ExecuteNonQuery();
        }

        public void ClearAllData()
        {
            var tables = new[] { "DailyCloses", "Movements", "Shifts", "Products", "Users" };
            
            foreach (var table in tables)
            {
                using var command = new SQLiteCommand($"DELETE FROM {table}", _connection);
                command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}