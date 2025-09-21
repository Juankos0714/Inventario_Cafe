namespace InventorySystem.Models.Entities
{
    public class DailyClose
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalGeneral { get; set; }
        public int TotalTransactions { get; set; }
        public int GeneratedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Propiedad de navegación
        public User? GeneratedByUser { get; set; }
    }
}