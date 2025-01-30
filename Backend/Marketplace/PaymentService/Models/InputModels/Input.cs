using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace PaymentService.Models.InputModels
{
    public class Input
    {
        [Required]
        [Range(0, 9999999999999999.99)]
        public decimal Amount { get; set; }
    }
}
