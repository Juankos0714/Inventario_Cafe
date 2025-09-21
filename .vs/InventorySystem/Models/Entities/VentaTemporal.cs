using InventorySystem.BLL;

namespace InventorySystem.Models.Entities
{
    public class VentaTemporal
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Nombre { get; set; } = string.Empty;
        public List<SaleItem> Items { get; set; } = new List<SaleItem>();
        public decimal MontoRecibido { get; set; } = 0;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        
        public decimal Total => Items.Sum(i => i.Quantity * i.UnitPrice);
        public decimal Cambio => MontoRecibido >= Total ? MontoRecibido - Total : 0;
        public bool EsValida => Items.Any() && MontoRecibido >= Total;
        public int CantidadItems => Items.Sum(i => i.Quantity);
        
        public string DisplayText => $"{Nombre} - {Total:C} ({CantidadItems} items)";
        
        public override string ToString() => DisplayText;
    }
}