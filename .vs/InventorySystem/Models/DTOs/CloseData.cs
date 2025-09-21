namespace InventorySystem.Models.DTOs
{
    public class CloseData
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CloseType { get; set; } = "FINAL";
        public int CloseSequence { get; set; } = 1;
        public decimal TotalSales { get; set; }
        public int TransactionCount { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<ShiftSummary> Shifts { get; set; } = new List<ShiftSummary>();
        public List<ProductSale> TopProducts { get; set; } = new List<ProductSale>();
    }
}