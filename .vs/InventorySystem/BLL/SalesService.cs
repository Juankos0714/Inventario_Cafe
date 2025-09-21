using InventorySystem.DAL;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;

namespace InventorySystem.BLL
{
    public class SalesService
    {
        private readonly MovementRepository _movementRepository;
        private readonly ProductRepository _productRepository;
        private readonly ShiftRepository _shiftRepository;
        private readonly AuthService _authService;
        
        public SalesService(AuthService authService)
        {
            _movementRepository = new MovementRepository();
            _productRepository = new ProductRepository();
            _shiftRepository = new ShiftRepository();
            _authService = authService;
        }
        
        public bool ProcessSale(List<SaleItem> saleItems)
        {
            if (_authService.CurrentUser == null || 
                (_authService.CurrentUser.Role != UserRole.Admin && _authService.CurrentUser.Role != UserRole.Vendedor))
                return false;
            
            // Verificar que hay un turno activo
            var activeShift = _shiftRepository.GetActiveShiftByUser(_authService.CurrentUser.Id);
            if (activeShift == null)
                return false;
            
            try
            {
                // Validar stock para todos los productos antes de procesar
                foreach (var item in saleItems)
                {
                    var product = _productRepository.GetProductById(item.ProductId);
                    if (product == null || product.StockActual < item.Quantity)
                        return false;
                }
                
                // Procesar la venta
                foreach (var item in saleItems)
                {
                    var product = _productRepository.GetProductById(item.ProductId);
                    if (product == null) continue;
                    
                    // Crear movimiento de venta
                    var movement = new Movement
                    {
                        Date = DateTime.Now,
                        Type = MovementType.Venta,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalAmount = item.Quantity * item.UnitPrice,
                        UserId = _authService.CurrentUser.Id
                    };
                    
                    _movementRepository.CreateMovement(movement);
                    
                    // Actualizar stock
                    var newStock = product.StockActual - item.Quantity;
                    _productRepository.UpdateStock(item.ProductId, newStock);
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public List<Movement> GetSalesByUser(int userId, DateTime? date = null)
        {
            return _movementRepository.GetMovementsByUser(userId, date)
                .Where(m => m.Type == MovementType.Venta)
                .ToList();
        }
        
        public List<Movement> GetSalesByDateRange(DateTime startDate, DateTime endDate)
        {
            return _movementRepository.GetMovementsByDateRange(startDate, endDate)
                .Where(m => m.Type == MovementType.Venta)
                .ToList();
        }
    }
    
    public class SaleItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        
        // Propiedad de navegación
        public Product? Product { get; set; }
    }
}