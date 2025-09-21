using AutoFixture;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;
using InventorySystem.BLL;

namespace InventorySystem.Tests.TestHelpers
{
    public class TestDataBuilder
    {
        private readonly Fixture _fixture;

        public TestDataBuilder()
        {
            _fixture = new Fixture();
            ConfigureFixture();
        }

        private void ConfigureFixture()
        {
            // Configurar generación de datos específicos
            _fixture.Customize<Product>(composer =>
                composer
                    .With(p => p.Price, () => _fixture.Create<decimal>() % 100000 + 1) // Precio positivo
                    .With(p => p.StockActual, () => _fixture.Create<int>() % 1000) // Stock no negativo
                    .With(p => p.StockMinimo, () => _fixture.Create<int>() % 100) // Stock mínimo no negativo
                    .With(p => p.IsActive, true)
                    .Without(p => p.Id)); // ID se asigna por BD

            _fixture.Customize<User>(composer =>
                composer
                    .With(u => u.IsActive, true)
                    .With(u => u.Role, UserRole.Vendedor)
                    .Without(u => u.Id));

            _fixture.Customize<SaleItem>(composer =>
                composer
                    .With(s => s.Quantity, () => _fixture.Create<int>() % 10 + 1) // Cantidad positiva
                    .With(s => s.UnitPrice, () => _fixture.Create<decimal>() % 10000 + 1)); // Precio positivo
        }

        public Product CreateProduct(
            string? name = null,
            string? category = null,
            decimal? price = null,
            int? stock = null,
            int? minStock = null,
            bool? isActive = null)
        {
            var product = _fixture.Create<Product>();
            
            if (name != null) product.Name = name;
            if (category != null) product.Category = category;
            if (price.HasValue) product.Price = price.Value;
            if (stock.HasValue) product.StockActual = stock.Value;
            if (minStock.HasValue) product.StockMinimo = minStock.Value;
            if (isActive.HasValue) product.IsActive = isActive.Value;
            
            return product;
        }

        public User CreateUser(
            string? name = null,
            string? passwordHash = null,
            UserRole? role = null,
            bool? isActive = null)
        {
            var user = _fixture.Create<User>();
            
            if (name != null) user.Name = name;
            if (passwordHash != null) user.PasswordHash = passwordHash;
            if (role.HasValue) user.Role = role.Value;
            if (isActive.HasValue) user.IsActive = isActive.Value;
            
            return user;
        }

        public List<SaleItem> CreateSaleItems(int count = 3, int? productId = null)
        {
            var items = new List<SaleItem>();
            
            for (int i = 0; i < count; i++)
            {
                var item = _fixture.Create<SaleItem>();
                if (productId.HasValue) item.ProductId = productId.Value;
                items.Add(item);
            }
            
            return items;
        }

        public Movement CreateMovement(
            MovementType? type = null,
            int? productId = null,
            int? userId = null,
            int? quantity = null,
            decimal? unitPrice = null)
        {
            var movement = _fixture.Create<Movement>();
            
            if (type.HasValue) movement.Type = type.Value;
            if (productId.HasValue) movement.ProductId = productId.Value;
            if (userId.HasValue) movement.UserId = userId.Value;
            if (quantity.HasValue) movement.Quantity = quantity.Value;
            if (unitPrice.HasValue) movement.UnitPrice = unitPrice.Value;
            
            movement.TotalAmount = movement.Quantity * movement.UnitPrice;
            
            return movement;
        }

        public Shift CreateShift(
            int? userId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            bool? isClosed = null)
        {
            var shift = _fixture.Create<Shift>();
            
            if (userId.HasValue) shift.UserId = userId.Value;
            if (startTime.HasValue) shift.StartTime = startTime.Value;
            if (endTime.HasValue) shift.EndTime = endTime.Value;
            if (isClosed.HasValue) shift.IsClosed = isClosed.Value;
            
            return shift;
        }
    }
}