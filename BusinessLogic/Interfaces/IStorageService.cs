using DataAccess.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IStorageService
    {
        Task<List<Storage>> GetAll();
        Task<Storage?> Get(int? id);
        Storage CreateByQuantity(Product product, int quantity);
        Task Create(Storage storage);
        Task Update(Storage storage);
        Task Delete(int id);
        Task<int> GetStorageQuantityForProduct(int idProduct);
        Task<Product> UpdateQuantity(int productId, int quantity);
        Task UpdateQuantityDecrease(List<int> productId, List<int> quantity);
    }
}
