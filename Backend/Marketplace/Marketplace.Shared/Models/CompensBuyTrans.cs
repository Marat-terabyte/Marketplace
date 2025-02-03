using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Shared.Models
{
    public class CompensBuyTrans : BuyTransactionModel
    {
        public CompensBuyTrans() { }

        public CompensBuyTrans(BuyTransactionModel model)
        {
            ProductIds = model.ProductIds;
            ConsumerIds = model.ConsumerIds;
            SellerIds = model.SellerIds;
            Prices = model.Prices;
            Counts = model.Counts;
            DeliveryPlace = model.DeliveryPlace;
        }

        public string ErrorMessage { get; set; }
    }
}
