using ShopMVC.Models;

namespace ShopMVC.Interfaces
{
    public interface IProductShowService
    {
        Task <List<ProductCartViewModel>> GetTop4ProductsAsync();
        Task<List<HomePageViewModel>> CreateMainPageAsync(int? category_id);
    }
}
