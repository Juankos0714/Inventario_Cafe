using InventorySystem.Models.Enums;

namespace InventorySystem.Models.Entities
{
    public class Movement
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public MovementType Type { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserId { get; set; }
        public string? Notes { get; set; }
        
        // Propiedades de navegación (se llenarán desde la DAL)
        public Product? Product { get; set; }
        public User? User { get; set; }
    }
}