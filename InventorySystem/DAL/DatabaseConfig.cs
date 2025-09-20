using System.Data.SQLite;

namespace InventorySystem.DAL
{
    public static class DatabaseConfig
    {
        private static readonly string DatabasePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "InventorySystem",
            "database.db"
        );
        
        public static string ConnectionString => $"Data Source={DatabasePath};Version=3;";
        
        public static void EnsureDatabaseDirectoryExists()
        {
            var directory = Path.GetDirectoryName(DatabasePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }
        }
    }
}