using InventorySystem.DAL;
using InventorySystem.Models.Entities;
using BCrypt.Net;

namespace InventorySystem.BLL
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private User? _currentUser;
        
        public AuthService()
        {
            _userRepository = new UserRepository();
        }
        
        public User? CurrentUser => _currentUser;
        
        public bool Login(string username, string password)
        {
            try
            {
                var user = _userRepository.GetUserByName(username);
                
                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    _currentUser = user;
                    return true;
                }
                
                return false;
            }
            catch
            {
                return false;
            }
        }
        
        public void Logout()
        {
            _currentUser = null;
        }
        
        public bool ChangePassword(string currentPassword, string newPassword)
        {
            if (_currentUser == null)
                return false;
                
            try
            {
                if (BCrypt.Net.BCrypt.Verify(currentPassword, _currentUser.PasswordHash))
                {
                    var newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    var result = _userRepository.UpdatePassword(_currentUser.Id, newHash);
                    
                    if (result)
                    {
                        _currentUser.PasswordHash = newHash;
                    }
                    
                    return result;
                }
                
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}