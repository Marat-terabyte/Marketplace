namespace PaymentService.Models.Repositories
{
    public interface IBalanceRepository
    {
        Task<Balance?> GetBalanceByUserIdAsync(string userId);
        Task TopUpByUserIdAsync(string userId, decimal amount);
        Task WithdrawByUserIdAsync(string userId, decimal amount);
        Task<Balance> CreateBalanceAsync(string userId);
    }
}
