using BusinessLogic.DTOs;

namespace BusinessLogic.Interfaces
{
    public interface IProductsService
    {
        Task <List<ProductDTO>> GetAll();
        Task <List<ProductDTO>> GetAllByPrice();
        Task <ProductDTO?> Get(int? id);
        Task Create(CreateProductDTO createProductDTO);
        Task Update(ProductDTO ProductDTO);
        Task Delete(int id);
    }
}
