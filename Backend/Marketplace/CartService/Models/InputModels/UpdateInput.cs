using System.ComponentModel.DataAnnotations;

namespace CartService.Models.InputModels
{
    public class UpdateInput
    {
        [Range(0, int.MaxValue, ErrorMessage = "CountRangeError")]
        public int Count { get; set; }
    }
}
