using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Interfaces
{
    public interface IFilesService
    {
        Task<string> SaveProductImage(IFormFile file);
        Task DeleteProductImage(string imagePath);
        Task <string> UpdateProductImage(string fileName,IFormFile file);
    }
}
