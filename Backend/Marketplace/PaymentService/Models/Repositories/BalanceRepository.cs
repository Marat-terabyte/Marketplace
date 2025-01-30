using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Exceptions;

namespace PaymentService.Models.Repositories
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly ApplicationContext _context;

        public BalanceRepository(ApplicationContext db)
        {
            _context = db;
        }

        public async Task<Balance?> GetBalanceByUserIdAsync(string userId)
        {
            return await _context.Balances.FirstOrDefaultAsync(b => b.UserId == userId);
        }

        public async Task TopUpByUserIdAsync(string userId, decimal amount)
        {
            Balance? balance = await GetBalanceByUserIdAsync(userId);
            if (balance == null)
                balance = await CreateBalanceAsync(userId);

            balance.Account += amount;
            await _context.SaveChangesAsync();
        }

        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="LowBalanceException"></exception>
        public async Task WithdrawByUserIdAsync(string userId, decimal amount)
        {
            Balance? balance = await GetBalanceByUserIdAsync(userId);
            if (balance == null)
                throw new NullReferenceException($"The balance of {userId} doesn't exist");

            if (balance.Account < amount)
                throw new LowBalanceException("Not enough money");

            balance.Account -= amount;

            await _context.SaveChangesAsync();
        }

        public async Task<Balance> CreateBalanceAsync(string userId)
        {
            Balance balance = new Balance() { UserId = userId, Account = 0 };
            _context.Balances.Add(balance);
            await _context.SaveChangesAsync();

            return balance;
        }
    }
}
