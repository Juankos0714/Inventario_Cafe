using InventorySystem.UI.Forms;
using InventorySystem.DAL;

namespace InventorySystem
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
            // Inicializar la base de datos
            DatabaseInitializer.Initialize();
            
            Application.Run(new LoginForm());
        }
    }
}