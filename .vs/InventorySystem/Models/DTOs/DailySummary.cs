namespace InventorySystem.Models.DTOs
{
    public class DailySummary
    {
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalTransactions { get; set; }
        public List<ShiftSummary> Shifts { get; set; } = new List<ShiftSummary>();
        public List<ProductSale> ProductSales { get; set; } = new List<ProductSale>();
    }
    
    public class ProductSale
    {
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}