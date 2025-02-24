using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Models.InputModels
{
    public class ProductInput
    {
        [Required(ErrorMessage = "ProductNameRequired")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "ProductCategoryRequired")]
        public string Category { get; set; }
        
        [Required(ErrorMessage = "ProductDescriptionRequired")]
        public string Description { get; set; }
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "ProductPriceRange")]
        public decimal Price { get; set; }
        
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "ProductStockRange")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "ProductImagesRequired")]
        public string[] Images { get; set; }

        public Dictionary<string, string>? Attributes { get; set; }
    }
}
