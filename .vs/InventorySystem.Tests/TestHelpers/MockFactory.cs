using Moq;
using InventorySystem.BLL;
using InventorySystem.DAL;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;

namespace InventorySystem.Tests.TestHelpers
{
    public class MockFactory
    {
        public static Mock<AuthService> CreateMockAuthService(User? currentUser = null)
        {
            var mock = new Mock<AuthService>();
            
            if (currentUser != null)
            {
                mock.Setup(x => x.CurrentUser).Returns(currentUser);
                mock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            }
            else
            {
                mock.Setup(x => x.CurrentUser).Returns((User?)null);
                mock.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            }
            
            return mock;
        }

        public static Mock<ProductRepository> CreateMockProductRepository(List<Product>? products = null)
        {
            var mock = new Mock<ProductRepository>();
            
            if (products != null)
            {
                mock.Setup(x => x.GetAllProducts()).Returns(products);
                
                foreach (var product in products)
                {
                    mock.Setup(x => x.GetProductById(product.Id)).Returns(product);
                }
            }
            
            mock.Setup(x => x.CreateProduct(It.IsAny<Product>())).Returns(true);
            mock.Setup(x => x.UpdateProduct(It.IsAny<Product>())).Returns(true);
            mock.Setup(x => x.UpdateStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            
            return mock;
        }

        public static Mock<UserRepository> CreateMockUserRepository(List<User>? users = null)
        {
            var mock = new Mock<UserRepository>();
            
            if (users != null)
            {
                mock.Setup(x => x.GetAllUsers()).Returns(users);
                
                foreach (var user in users)
                {
                    mock.Setup(x => x.GetUserByName(user.Name)).Returns(user);
                    mock.Setup(x => x.GetUserById(user.Id)).Returns(user);
                }
            }
            
            mock.Setup(x => x.CreateUser(It.IsAny<User>())).Returns(true);
            mock.Setup(x => x.UpdateUser(It.IsAny<User>())).Returns(true);
            
            return mock;
        }

        public static Mock<MovementRepository> CreateMockMovementRepository()
        {
            var mock = new Mock<MovementRepository>();
            
            mock.Setup(x => x.CreateMovement(It.IsAny<Movement>())).Returns(true);
            mock.Setup(x => x.GetMovementsByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<Movement>());
            
            return mock;
        }

        public static Mock<ShiftRepository> CreateMockShiftRepository()
        {
            var mock = new Mock<ShiftRepository>();
            
            mock.Setup(x => x.StartShift(It.IsAny<int>())).Returns(1);
            mock.Setup(x => x.CloseShift(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<int>())).Returns(true);
            
            return mock;
        }
    }
}