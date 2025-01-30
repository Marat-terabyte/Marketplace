using System.Globalization;

namespace PaymentService.Models
{
    public class Balance
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Account { get; set; }
    }
}
