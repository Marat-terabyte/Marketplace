using Marketplace.Shared.Models;
using MongoDB.Driver;
using ProductService.Models.Products.Repositories;

namespace ProductService.Services
{
    public class ProductStockService
    {
        private readonly ItemService _itemService;

        public ProductStockService(ItemService productRepository)
        {
            _itemService = productRepository;
        }

        public async Task<bool> ModifyStockAsync(BuyTransactionModel transactionModel)
        {
            bool isSuccess = false;

            try
            {
                bool res = await ModifyAsync(transactionModel);
                if (!res)
                    return false;

                isSuccess = true;
            }
            catch (Exception ex)
            {
            }

            return isSuccess;
        }

        private async Task<bool> ModifyAsync(BuyTransactionModel model)
        {
            for (int i = 0; i < model.ProductIds.Count; i++)
            {
                var product = await _itemService.GetByIdAsync(model.ProductIds[i]);
                if (product == null)
                    return false;

                if (product.Stock < model.Counts[i])
                    return false;
                
                product.Stock -= model.Counts[i];
                await _itemService.UpdateAsync(product);
            }

            return true;
        }

        public async Task<bool> RestoreStockAsync(CompensBuyTrans compensation)
        {
            bool isSuccess = false;
            try
            {
                bool res = await RestoreAsync(compensation);
                if (!res)
                    return false;

                isSuccess = true;
            }
            catch
            {
            }


            return isSuccess;
        }

        private async Task<bool> RestoreAsync(CompensBuyTrans compenstation)
        {
            for (int i = 0; i < compenstation.ProductIds.Count; i++)
            {
                var product = await _itemService.GetByIdAsync(compenstation.ProductIds[i]);
                if (product == null)
                    return false;

                product.Stock += compenstation.Counts[i];
                await _itemService.UpdateAsync(product);
            }

            return true;
        }
    }
}
