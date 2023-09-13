using DataAccess.Entities;
using ShopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMVC.Interfaces
{
    public interface IOrdersService
    {
        Task<OrdersProductViewModel> GetAll();
        Task Create();
    }
}
