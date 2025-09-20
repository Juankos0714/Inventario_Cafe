namespace InventorySystem.Models.Entities
{
    public class Shift
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal TotalSold { get; set; }
        public int TotalTransactions { get; set; }
        public bool IsClosed { get; set; } = false;
        
        // Propiedad de navegación
        public User? User { get; set; }
    }
}