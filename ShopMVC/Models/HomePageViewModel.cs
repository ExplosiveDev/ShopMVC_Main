using DataAccess.Entities;

namespace ShopMVC.Models
{
    public class HomePageViewModel
    {
        public List<ProductCartViewModel> AllProducts { get; set; }
        public List<ProductCartViewModel> TopProducts { get; set; }
    }
}
