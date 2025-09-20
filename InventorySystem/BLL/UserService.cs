using InventorySystem.DAL;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;
using BCrypt.Net;

namespace InventorySystem.BLL
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly AuthService _authService;
        
        public UserService(AuthService authService)
        {
            _userRepository = new UserRepository();
            _authService = authService;
        }
        
        public List<User> GetAllUsers()
        {
            // Solo los administradores pueden ver todos los usuarios
            if (_authService.CurrentUser?.Role != UserRole.Admin)
                return new List<User>();
                
            return _userRepository.GetAllUsers();
        }
        
        public bool CreateUser(string name, string password, UserRole role)
        {
            // Solo los administradores pueden crear usuarios
            if (_authService.CurrentUser?.Role != UserRole.Admin)
                return false;
                
            try
            {
                // Verificar si el usuario ya existe
                var existingUser = _userRepository.GetUserByName(name);
                if (existingUser != null)
                    return false;
                
                var user = new User
                {
                    Name = name,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    Role = role,
                    IsActive = true
                };
                
                return _userRepository.CreateUser(user);
            }
            catch
            {
                return false;
            }
        }
        
        public bool UpdateUser(int userId, string name, UserRole role, bool isActive)
        {
            // Solo los administradores pueden actualizar usuarios
            if (_authService.CurrentUser?.Role != UserRole.Admin)
                return false;
                
            try
            {
                var user = _userRepository.GetUserById(userId);
                if (user == null)
                    return false;
                
                user.Name = name;
                user.Role = role;
                user.IsActive = isActive;
                
                return _userRepository.UpdateUser(user);
            }
            catch
            {
                return false;
            }
        }
        
        public bool ResetPassword(int userId, string newPassword)
        {
            // Solo los administradores pueden resetear contraseñas
            if (_authService.CurrentUser?.Role != UserRole.Admin)
                return false;
                
            try
            {
                var newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                return _userRepository.UpdatePassword(userId, newHash);
            }
            catch
            {
                return false;
            }
        }
    }
}