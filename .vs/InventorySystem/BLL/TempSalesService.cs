using System.Text.Json;
using InventorySystem.DAL;
using InventorySystem.Models.Entities;

namespace InventorySystem.BLL
{
    public class TempSalesService
    {
        private readonly TempSaleRepository _tempSaleRepository;
        private readonly AuthService _authService;
        
        public TempSalesService(AuthService authService)
        {
            _tempSaleRepository = new TempSaleRepository();
            _authService = authService;
        }
        
        public void SaveTempSale(VentaTemporal venta)
        {
            if (_authService.CurrentUser == null) return;
            
            try
            {
                var itemsJson = JsonSerializer.Serialize(venta.Items);
                
                var tempSale = new TempSale
                {
                    Alias = venta.Id,
                    UserId = _authService.CurrentUser.Id,
                    Items = venta.Items.Select(item => new TempSaleItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Product = item.Product
                    }).ToList()
                };
                
                // Verificar si ya existe
                var existing = _tempSaleRepository.GetActiveTempSalesByUser(_authService.CurrentUser.Id)
                    .FirstOrDefault(ts => ts.Alias == venta.Id);
                
                if (existing != null)
                {
                    // Actualizar existente
                    _tempSaleRepository.DeleteTempSale(existing.Id);
                }
                
                // Crear nuevo
                _tempSaleRepository.CreateTempSale(venta.Nombre, _authService.CurrentUser.Id);
            }
            catch (Exception ex)
            {
                // Log error but don't throw - persistence is not critical
                System.Diagnostics.Debug.WriteLine($"Error saving temp sale: {ex.Message}");
            }
        }
        
        public List<VentaTemporal> LoadTempSales()
        {
            if (_authService.CurrentUser == null) 
                return new List<VentaTemporal>();
            
            try
            {
                var tempSales = _tempSaleRepository.GetActiveTempSalesByUser(_authService.CurrentUser.Id);
                
                return tempSales.Select(ts => new VentaTemporal
                {
                    Id = ts.Alias,
                    Nombre = ts.Alias,
                    Items = ts.Items.Select(item => new SaleItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Product = item.Product
                    }).ToList(),
                    FechaCreacion = ts.CreatedAt,
                    MontoRecibido = 0 // Se calculará en la UI
                }).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading temp sales: {ex.Message}");
                return new List<VentaTemporal>();
            }
        }
        
        public void DeleteTempSale(string ventaId)
        {
            if (_authService.CurrentUser == null) return;
            
            try
            {
                var tempSales = _tempSaleRepository.GetActiveTempSalesByUser(_authService.CurrentUser.Id);
                var tempSale = tempSales.FirstOrDefault(ts => ts.Alias == ventaId);
                
                if (tempSale != null)
                {
                    _tempSaleRepository.DeleteTempSale(tempSale.Id);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting temp sale: {ex.Message}");
            }
        }
        
        public void ClearOldTempSales()
        {
            try
            {
                // Esta funcionalidad se puede implementar más tarde
                // Por ahora, las ventas temporales se mantienen hasta ser procesadas
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing old temp sales: {ex.Message}");
            }
        }
        
        public void SaveAllTempSales(List<VentaTemporal> ventas)
        {
            foreach (var venta in ventas.Where(v => v.Items.Any()))
            {
                SaveTempSale(venta);
            }
        }
    }
}