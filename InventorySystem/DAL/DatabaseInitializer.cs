using System.Data.SQLite;
using InventorySystem.Models.Enums;
using BCrypt.Net;

namespace InventorySystem.DAL
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            DatabaseConfig.EnsureDatabaseDirectoryExists();
            
            using var connection = new SQLiteConnection(DatabaseConfig.ConnectionString);
            connection.Open();
            
            CreateTables(connection);
            CreateDefaultAdmin(connection);
        }
        
        private static void CreateTables(SQLiteConnection connection)
        {
            var commands = new[]
            {
                @"CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    Role INTEGER NOT NULL,
                    IsActive INTEGER NOT NULL DEFAULT 1,
                    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                )",
                
                @"CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Category TEXT NOT NULL,
                    Price DECIMAL(10,2) NOT NULL,
                    StockActual INTEGER NOT NULL DEFAULT 0,
                    StockMinimo INTEGER NOT NULL DEFAULT 0,
                    IsActive INTEGER NOT NULL DEFAULT 1,
                    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                )",
                
                @"CREATE TABLE IF NOT EXISTS Movements (
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
                
                @"CREATE TABLE IF NOT EXISTS Shifts (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    StartTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    EndTime DATETIME,
                    TotalSold DECIMAL(10,2) NOT NULL DEFAULT 0,
                    TotalTransactions INTEGER NOT NULL DEFAULT 0,
                    IsClosed INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                )",
                
                @"CREATE TABLE IF NOT EXISTS DailyCloses (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date DATE NOT NULL,
                    TotalGeneral DECIMAL(10,2) NOT NULL,
                    TotalTransactions INTEGER NOT NULL,
                    GeneratedBy INTEGER NOT NULL,
                    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (GeneratedBy) REFERENCES Users(Id)
                )",
                
                @"CREATE TABLE IF NOT EXISTS TempSales (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Alias TEXT NOT NULL,
                    UserId INTEGER NOT NULL,
                    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    IsActive INTEGER NOT NULL DEFAULT 1,
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                )",
                
                @"CREATE TABLE IF NOT EXISTS TempSaleItems (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TempSaleId INTEGER NOT NULL,
                    ProductId INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL,
                    UnitPrice DECIMAL(10,2) NOT NULL,
                    FOREIGN KEY (TempSaleId) REFERENCES TempSales(Id),
                    FOREIGN KEY (ProductId) REFERENCES Products(Id)
                )"
            };
            
            foreach (var commandText in commands)
            {
                using var command = new SQLiteCommand(commandText, connection);
                command.ExecuteNonQuery();
            }
        }
        
        private static void CreateDefaultAdmin(SQLiteConnection connection)
        {
            // Verificar si ya existe un administrador
            var checkCommand = new SQLiteCommand(
                "SELECT COUNT(*) FROM Users WHERE Role = @role", 
                connection);
            checkCommand.Parameters.AddWithValue("@role", (int)UserRole.Admin);
            
            var adminCount = Convert.ToInt32(checkCommand.ExecuteScalar());
            
            if (adminCount == 0)
            {
                // Crear administrador por defecto
                var insertCommand = new SQLiteCommand(
                    "INSERT INTO Users (Name, PasswordHash, Role) VALUES (@name, @password, @role)",
                    connection);
                
                insertCommand.Parameters.AddWithValue("@name", "admin");
                insertCommand.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword("admin123"));
                insertCommand.Parameters.AddWithValue("@role", (int)UserRole.Admin);
                
                insertCommand.ExecuteNonQuery();
            }
        }
    }
}