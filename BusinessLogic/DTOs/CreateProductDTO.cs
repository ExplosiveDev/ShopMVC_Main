using Microsoft.AspNetCore.Http;

namespace BusinessLogic.DTOs
{
    public class CreateProductDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int? StorageId { get; set; }
        public int StorageQuantity { get; set; }
    }
}
