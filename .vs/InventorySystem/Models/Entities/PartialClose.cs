namespace InventorySystem.Models.Entities
{
    public class PartialClose
    {
        public int Id { get; set; }
        public DateTime CloseDate { get; set; }
        public TimeSpan CloseTime { get; set; }
        public string CloseType { get; set; } = "PARCIAL"; // PARCIAL, FINAL
        public int UserId { get; set; }
        public decimal TotalSales { get; set; }
        public int TransactionCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Propiedad de navegación
        public User? User { get; set; }
    }
}