using Marketplace.Shared.Models;

namespace PaymentService.Services
{
    public static class AmountService
    {
        public static decimal GetAmount(BuyTransactionModel transactionModel)
        {
            decimal amount = 0;
            for (int i = 0; i < transactionModel.ProductIds.Count; i++)
            {
                amount += transactionModel.Prices[i] * transactionModel.Counts[i];
            }

            return amount;
        }
    }
}
