using InventorySystem.DAL;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;

namespace InventorySystem.BLL
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly AuthService _authService;
        
        public ProductService(AuthService authService)
        {
            _productRepository = new ProductRepository();
            _authService = authService;
        }
        
        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }
        
        public Product? GetProductById(int id)
        {
            return _productRepository.GetProductById(id);
        }
        
        public bool CreateProduct(Product product)
        {
            // Solo administradores pueden crear productos
            if (_authService.CurrentUser?.Role != UserRole.Admin)
                return false;
                
            return _productRepository.CreateProduct(product);
        }
        
        public bool UpdateProduct(Product product)
        {
            // Solo administradores pueden actualizar productos
            if (_authService.CurrentUser?.Role != UserRole.Admin)
                return false;
                
            return _productRepository.UpdateProduct(product);
        }
        
        public bool UpdateStock(int productId, int quantity, MovementType movementType, string? notes = null)
        {
            // Solo administradores pueden ajustar stock directamente
            if (_authService.CurrentUser?.Role != UserRole.Admin && movementType != MovementType.Venta)
                return false;
                
            var product = _productRepository.GetProductById(productId);
            if (product == null)
                return false;
            
            int newStock = product.StockActual;
            
            switch (movementType)
            {
                case MovementType.Entrada:
                    newStock += quantity;
                    break;
                case MovementType.Venta:
                    newStock -= quantity;
                    break;
                case MovementType.Ajuste:
                    newStock = quantity; // En ajuste, quantity es el nuevo stock total
                    break;
            }
            
            if (newStock < 0)
                return false; // No permitir stock negativo
                
            return _productRepository.UpdateStock(productId, newStock);
        }
        
        public List<Product> GetLowStockProducts()
        {
            return _productRepository.GetLowStockProducts();
        }
        
        public bool ValidateStockForSale(int productId, int quantity)
        {
            var product = _productRepository.GetProductById(productId);
            return product != null && product.StockActual >= quantity;
        }
    }
}