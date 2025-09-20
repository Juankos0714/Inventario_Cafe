namespace InventorySystem.Models.Entities
{
    public class TempSale
    {
        public int Id { get; set; }
        public string Alias { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        
        // Propiedad de navegación
        public User? User { get; set; }
        public List<TempSaleItem> Items { get; set; } = new List<TempSaleItem>();
    }
    
    public class TempSaleItem
    {
        public int Id { get; set; }
        public int TempSaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        
        // Propiedades de navegación
        public TempSale? TempSale { get; set; }
        public Product? Product { get; set; }
        
        public decimal Total => Quantity * UnitPrice;
    }
}