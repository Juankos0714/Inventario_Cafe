using InventorySystem.DAL;
using InventorySystem.Models.Entities;

namespace InventorySystem.BLL
{
    public class TempSaleService
    {
        private readonly TempSaleRepository _tempSaleRepository;
        private readonly ProductRepository _productRepository;
        private readonly AuthService _authService;
        
        public TempSaleService(AuthService authService)
        {
            _tempSaleRepository = new TempSaleRepository();
            _productRepository = new ProductRepository();
            _authService = authService;
        }
        
        public List<TempSale> GetUserTempSales()
        {
            if (_authService.CurrentUser == null)
                return new List<TempSale>();
                
            return _tempSaleRepository.GetActiveTempSalesByUser(_authService.CurrentUser.Id);
        }
        
        public int CreateTempSale(string alias)
        {
            if (_authService.CurrentUser == null)
                return 0;
                
            return _tempSaleRepository.CreateTempSale(alias, _authService.CurrentUser.Id);
        }
        
        public bool AddItemToTempSale(int tempSaleId, int productId, int quantity)
        {
            var product = _productRepository.GetProductById(productId);
            if (product == null)
                return false;
                
            return _tempSaleRepository.AddItemToTempSale(tempSaleId, productId, quantity, product.Price);
        }
        
        public bool RemoveItemFromTempSale(int tempSaleId, int productId)
        {
            return _tempSaleRepository.RemoveItemFromTempSale(tempSaleId, productId);
        }
        
        public bool DeleteTempSale(int tempSaleId)
        {
            return _tempSaleRepository.DeleteTempSale(tempSaleId);
        }
        
        public List<SaleItem> ConvertTempSaleToSaleItems(TempSale tempSale)
        {
            return tempSale.Items.Select(item => new SaleItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Product = item.Product
            }).ToList();
        }
    }
}