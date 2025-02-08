using Marketplace.Shared.Models;
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

        public async Task<(bool, string?)> ProcessPaymentAsync(BuyTransactionModel buyTransaction)
        {
            string? errorMessage = null;

            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var res = await ProcessAsync(buyTransaction);

                errorMessage = res.Item2;
                bool isSuccess = res.Item1;
                if (!isSuccess)
                    return (false, errorMessage);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                if (errorMessage != null)
                    errorMessage = "PaymentServiceError";

                return (false, errorMessage);
            }

            return (true, null);
        }

        private async Task<(bool, string?)> ProcessAsync(BuyTransactionModel transactionModel)
        {
            Dictionary<string, Balance> sellerBalances = new Dictionary<string, Balance>();
            
            Balance? userBalance = await GetBalanceByUserIdAsync(transactionModel.ConsumerIds[0]);
            if (userBalance == null)
                return (false, "NotEnoughMoney");
            
            for (int i = 0; i < transactionModel.ProductIds.Count; i++)
            {
                Balance? sellerBalance = null;

                string sellerId = transactionModel.SellerIds[i];
                decimal price = transactionModel.Prices[i];
                int count = transactionModel.Counts[i];

                sellerBalance = await GetSellerBalanceAsync(sellerBalances, sellerId);

                if (userBalance.Account < price * count)
                    return (false, "NotEnoughMoney");

                userBalance.Account -= price * count;
                sellerBalance.Account += price * count;
            }

            return (true, null);
        }

        public async Task<bool> RestorePaymentProcessAsync(BuyTransactionModel transactionModel)
        {
            try
            {
                var transaction = await _context.Database.BeginTransactionAsync();
                await RestoreProcessAsync(transactionModel);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private async Task RestoreProcessAsync(BuyTransactionModel transactionModel)
        {
            Dictionary<string, Balance> sellerBalances = new Dictionary<string, Balance>();

            Balance userBalance = (await GetBalanceByUserIdAsync(transactionModel.ConsumerIds[0]))!;
            for (int i = 0; i < transactionModel.ProductIds.Count; i++)
            {
                decimal price = transactionModel.Prices[i];
                string sellerId = transactionModel.SellerIds[i];
                int count = transactionModel.Counts[i];

                Balance sellerBalance = await GetSellerBalanceAsync(sellerBalances, sellerId);
                
                userBalance.Account += price * count;
                sellerBalance.Account -= price * count;
            }
        }

        private async Task<Balance> GetSellerBalanceAsync(Dictionary<string, Balance> sellerBalances, string sellerId)
        {
            Balance? sellerBalance;
            if (!(sellerBalances.TryGetValue(sellerId, out sellerBalance)))
            {
                sellerBalance = await GetBalanceByUserIdAsync(sellerId);
                if (sellerBalance == null)
                    sellerBalance = await CreateBalanceAsync(sellerId);

                sellerBalances.Add(sellerId, sellerBalance);
            }

            return sellerBalance;
        }
    }
}
