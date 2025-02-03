using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Shared.Models
{
    public class BuyTransactionModel
    {
        public List<string> ProductIds { get; set; }
        public List<string> SellerIds { get; set; }
        public List<string> ConsumerIds { get; set; }
        public List<decimal> Prices { get; set; }
        public List<int> Counts { get; set; }
        public string DeliveryPlace { get; set; }

        public BuyTransactionModel()
        {
            ProductIds = new List<string>();
            SellerIds = new List<string>();
            ConsumerIds = new List<string>();
            Prices = new List<decimal>();
            Counts = new List<int>();
        }
    }
}
