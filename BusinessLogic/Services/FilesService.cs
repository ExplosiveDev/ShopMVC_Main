using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Services
{
    public class FilesService : IFilesService
    {
        private const string imageFolder = "images";
        private readonly IWebHostEnvironment _environment;
        public FilesService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> SaveProductImage(IFormFile file)
        {
            string root = _environment.WebRootPath;
            string newNameFile = Guid.NewGuid().ToString();
            string extensionFile = Path.GetExtension(file.FileName);
            string fullFileName = newNameFile + extensionFile;
            string imagePath = Path.Combine(imageFolder, fullFileName);
            string imageFullPath = Path.Combine(root, imagePath);
            using(FileStream fileStream = new FileStream(imageFullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return imagePath;
        }
        public async Task DeleteProductImage(string imagePath)
        {
            await Task.Run(() =>
            {
                string root = _environment.WebRootPath;
                string imageFullPath = Path.Combine(root, imagePath);
                if (File.Exists(imageFullPath))
                {
                    File.Delete(imageFullPath);
                }
            });
        }
        public async Task<string> UpdateProductImage(string fileName, IFormFile file)
        {
            string root = _environment.WebRootPath;
            string imageFullPath = Path.Combine(root, fileName);
            if (File.Exists(imageFullPath))
            {
                File.Delete(imageFullPath);
            }

            using (FileStream fileStream = new FileStream(imageFullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }
    }
}
