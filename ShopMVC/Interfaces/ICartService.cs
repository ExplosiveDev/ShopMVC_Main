using DataAccess.Entities;
using ShopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMVC.Interfaces
{
    public interface ICartService
    {
        Task <List<ProductCartViewModel>> GetProducts();
        Task Create(int id);
        Task Delete(int id);
        Task UpdateQuantity(int productId, int quantity);
    }
}
