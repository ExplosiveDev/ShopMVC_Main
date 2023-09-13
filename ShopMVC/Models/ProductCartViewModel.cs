using BusinessLogic.DTOs;
using DataAccess.Entities;

namespace ShopMVC.Models
{
    public class ProductCartViewModel
    {
        //public Product Product { get; set; }
        public ProductDTO Product { get; set; }
        public bool IsInCart { get; set; }
        public int Quantity { get; set; }
        public int StorageQuantity { get; set; }
    }
}
