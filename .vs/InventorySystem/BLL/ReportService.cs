using InventorySystem.DAL;
using InventorySystem.Models.DTOs;
using InventorySystem.Models.Entities;
using InventorySystem.Models.Enums;

namespace InventorySystem.BLL
{
    public class ReportService
    {
        private readonly MovementRepository _movementRepository;
        private readonly ShiftRepository _shiftRepository;
        private readonly DailyCloseRepository _dailyCloseRepository;
        private readonly PartialCloseRepository _partialCloseRepository;
        private readonly AuthService _authService;
        
        public ReportService(AuthService authService)
        {
            _movementRepository = new MovementRepository();
            _shiftRepository = new ShiftRepository();
            _dailyCloseRepository = new DailyCloseRepository();
            _partialCloseRepository = new PartialCloseRepository();
            _authService = authService;
        }
        
        public DailySummary? GenerateDailySummary(DateTime date)
        {
            // Solo administradores y contadores pueden generar reportes
            if (_authService.CurrentUser?.Role == UserRole.Vendedor)
                return null;
            
            var shifts = _shiftRepository.GetShiftsByDate(date);
            var movements = _movementRepository.GetMovementsByDateRange(date, date)
                .Where(m => m.Type == MovementType.Venta)
                .ToList();
            
            var summary = new DailySummary
            {
                Date = date,
                TotalSales = movements.Sum(m => m.TotalAmount),
                TotalTransactions = movements.Count,
                Shifts = new List<ShiftSummary>(),
                ProductSales = movements
                    .GroupBy(m => m.Product?.Name ?? "")
                    .Select(g => new ProductSale
                    {
                        ProductName = g.Key,
                        TotalQuantity = g.Sum(m => m.Quantity),
                        TotalAmount = g.Sum(m => m.TotalAmount)
                    })
                    .OrderByDescending(ps => ps.TotalAmount)
                    .ToList()
            };
            
            // Agregar resúmenes de turnos cerrados
            foreach (var shift in shifts.Where(s => s.IsClosed))
            {
                var shiftMovements = movements
                    .Where(m => m.UserId == shift.UserId && 
                               m.Date >= shift.StartTime && 
                               (shift.EndTime == null || m.Date <= shift.EndTime))
                    .ToList();
                
                summary.Shifts.Add(new ShiftSummary
                {
                    ShiftId = shift.Id,
                    UserName = shift.User?.Name ?? "",
                    StartTime = shift.StartTime,
                    EndTime = shift.EndTime ?? DateTime.Now,
                    TotalSales = shiftMovements.Sum(m => m.TotalAmount),
                    TransactionCount = shiftMovements.Count,
                    SaleDetails = shiftMovements.Select(m => new SaleDetail
                    {
                        ProductName = m.Product?.Name ?? "",
                        Quantity = m.Quantity,
                        UnitPrice = m.UnitPrice,
                        Total = m.TotalAmount
                    }).ToList()
                });
            }
            
            return summary;
        }
        
        public bool CreateDailyClose(DateTime date)
        {
            // Solo administradores pueden crear cierres diarios
            if (_authService.CurrentUser?.Role != UserRole.Admin)
                return false;
            
            // Verificar que no existe un cierre para esa fecha
            var existingClose = _dailyCloseRepository.GetDailyCloseByDate(date);
            if (existingClose != null)
                return false;
            
            var summary = GenerateDailySummary(date);
            if (summary == null)
                return false;
            
            var dailyClose = new DailyClose
            {
                Date = date,
                TotalGeneral = summary.TotalSales,
                TotalTransactions = summary.TotalTransactions,
                GeneratedBy = _authService.CurrentUser.Id
            };
            
            return _dailyCloseRepository.CreateDailyClose(dailyClose);
        }
        
        public List<DailyClose> GetDailyClosesByDateRange(DateTime startDate, DateTime endDate)
        {
            // Solo administradores y contadores pueden ver cierres
            if (_authService.CurrentUser?.Role == UserRole.Vendedor)
                return new List<DailyClose>();
                
            return _dailyCloseRepository.GetDailyClosesByDateRange(startDate, endDate);
        }
        
        public List<Movement> GetMovementReport(DateTime startDate, DateTime endDate, MovementType? type = null)
        {
            // Solo administradores y contadores pueden ver reportes de movimientos
            if (_authService.CurrentUser?.Role == UserRole.Vendedor)
                return new List<Movement>();
            
            var movements = _movementRepository.GetMovementsByDateRange(startDate, endDate);
            
            if (type.HasValue)
            {
                movements = movements.Where(m => m.Type == type.Value).ToList();
            }
            
            return movements;
        }
        
        public bool CanCreatePartialClose(DateTime date)
        {
            // Solo administradores pueden crear cierres
            if (_authService.CurrentUser?.Role != UserRole.Admin)
                return false;
                
            // Verificar que no existan más de 2 cierres para la fecha
            var closesToday = _partialCloseRepository.GetCloseCountForDate(date);
            return closesToday < 2;
        }
        
        public bool CreatePartialClose(DateTime date, string closeType = "PARCIAL")
        {
            if (!CanCreatePartialClose(date))
                return false;
            
            var summary = GenerateDailySummary(date);
            if (summary == null) return false;
            
            var sequence = _partialCloseRepository.GetCloseCountForDate(date) + 1;
            
            var partialClose = new PartialClose
            {
                CloseDate = date,
                CloseTime = DateTime.Now.TimeOfDay,
                CloseType = closeType,
                UserId = _authService.CurrentUser!.Id,
                TotalSales = summary.TotalSales,
                TransactionCount = summary.TotalTransactions
            };
            
            return _partialCloseRepository.CreatePartialClose(partialClose);
        }
        
        public List<PartialClose> GetClosesForDate(DateTime date)
        {
            // Solo administradores y contadores pueden ver cierres
            if (_authService.CurrentUser?.Role == UserRole.Vendedor)
                return new List<PartialClose>();
                
            return _partialCloseRepository.GetPartialClosesByDate(date);
        }
        
        public CloseData? GetLastCloseForDate(DateTime date)
        {
            var closes = GetClosesForDate(date);
            var lastClose = closes.OrderByDescending(c => c.CreatedAt).FirstOrDefault();
            
            if (lastClose == null) return null;
            
            var summary = GenerateDailySummary(date);
            if (summary == null) return null;
            
            return new CloseData
            {
                Id = lastClose.Id,
                Date = lastClose.CloseDate,
                CloseType = lastClose.CloseType,
                CloseSequence = closes.Count,
                TotalSales = lastClose.TotalSales,
                TransactionCount = lastClose.TransactionCount,
                UserName = lastClose.User?.Name ?? "",
                CreatedAt = lastClose.CreatedAt,
                Shifts = summary.Shifts,
                TopProducts = summary.ProductSales.Take(5).ToList()
            };
        }
    }
}