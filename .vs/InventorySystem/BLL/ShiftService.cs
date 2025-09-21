using InventorySystem.DAL;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;
using InventorySystem.Models.DTOs;

namespace InventorySystem.BLL
{
    public class ShiftService
    {
        private readonly ShiftRepository _shiftRepository;
        private readonly MovementRepository _movementRepository;
        private readonly AuthService _authService;
        
        public ShiftService(AuthService authService)
        {
            _shiftRepository = new ShiftRepository();
            _movementRepository = new MovementRepository();
            _authService = authService;
        }
        
        public bool StartShift()
        {
            if (_authService.CurrentUser == null || 
                (_authService.CurrentUser.Role != UserRole.Admin && _authService.CurrentUser.Role != UserRole.Vendedor))
                return false;
            
            // Verificar que no hay un turno activo
            var activeShift = _shiftRepository.GetActiveShiftByUser(_authService.CurrentUser.Id);
            if (activeShift != null)
                return false;
            
            _shiftRepository.StartShift(_authService.CurrentUser.Id);
            return true;
        }
        
        public bool CloseShift()
        {
            if (_authService.CurrentUser == null)
                return false;
            
            var activeShift = _shiftRepository.GetActiveShiftByUser(_authService.CurrentUser.Id);
            if (activeShift == null)
                return false;
            
            // Calcular totales del turno
            var movements = _movementRepository.GetMovementsByUser(_authService.CurrentUser.Id, DateTime.Today)
                .Where(m => m.Type == MovementType.Venta && m.Date >= activeShift.StartTime)
                .ToList();
            
            var totalSold = movements.Sum(m => m.TotalAmount);
            var totalTransactions = movements.Count;
            
            return _shiftRepository.CloseShift(activeShift.Id, totalSold, totalTransactions);
        }
        
        public Shift? GetActiveShift()
        {
            if (_authService.CurrentUser == null)
                return null;
                
            return _shiftRepository.GetActiveShiftByUser(_authService.CurrentUser.Id);
        }
        
        public ShiftSummary? GetShiftSummary(int shiftId)
        {
            var shift = _shiftRepository.GetShiftById(shiftId);
            if (shift == null || !shift.IsClosed)
                return null;
            
            // Solo el dueño del turno o un administrador puede ver el resumen
            if (_authService.CurrentUser == null || 
                (shift.UserId != _authService.CurrentUser.Id && _authService.CurrentUser.Role != UserRole.Admin))
                return null;
            
            var movements = _movementRepository.GetMovementsByUser(shift.UserId, shift.StartTime.Date)
                .Where(m => m.Type == MovementType.Venta && 
                           m.Date >= shift.StartTime && 
                           (shift.EndTime == null || m.Date <= shift.EndTime))
                .ToList();
            
            var summary = new ShiftSummary
            {
                ShiftId = shift.Id,
                UserName = shift.User?.Name ?? "",
                StartTime = shift.StartTime,
                EndTime = shift.EndTime ?? DateTime.Now,
                TotalSales = movements.Sum(m => m.TotalAmount),
                TransactionCount = movements.Count,
                SaleDetails = movements.Select(m => new SaleDetail
                {
                    ProductName = m.Product?.Name ?? "",
                    Quantity = m.Quantity,
                    UnitPrice = m.UnitPrice,
                    Total = m.TotalAmount
                }).ToList()
            };
            
            return summary;
        }
        
        public List<Shift> GetShiftsByDate(DateTime date)
        {
            // Solo administradores pueden ver todos los turnos
            if (_authService.CurrentUser?.Role != UserRole.Admin)
                return new List<Shift>();
                
            return _shiftRepository.GetShiftsByDate(date);
        }
    }
}