using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShopMVC.Helper;
using ShopMVC.Interfaces;
using ShopMVC.Models;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace ShopMVC.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly HttpContext _httpContext;
        private readonly ShopMVCDbContext _context;
        private readonly IMapper _mapper;

        public OrdersService(IHttpContextAccessor httpContext, ShopMVCDbContext context, IMapper mapper)
        {
            _httpContext = httpContext.HttpContext;
            _context = context;
            _mapper = mapper;
        }
        public async Task<OrdersProductViewModel> GetAll()
        {
            string userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var orders = await _context.Orders.Where(user => user.UserId == userId).ToListAsync();
            var products = new List<Product>();
            foreach (var order in orders)
            {
                var productIdsJson = order.IdsProduct;
                if (!string.IsNullOrEmpty(productIdsJson))
                {
                    var productIdsDictionary = JsonSerializer.Deserialize<Dictionary<int, int>>(productIdsJson);
                    foreach (var productId in productIdsDictionary.Keys)
                    {
                        var productItem = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                        if (productItem != null)
                        {
                            products.Add(productItem);
                        }
                    }
                }
            }
            var productsDTO = _mapper.Map<List<ProductDTO>>(products);
            var ordersDTO = _mapper.Map<List<OrderDTO>>(orders);

            var viewModel = new OrdersProductViewModel
            {
                Orders = ordersDTO,
                Products = productsDTO
            };

            return viewModel;
        }
        public async Task Create()
        {
            Dictionary<int, int> idList = _httpContext.Session.GetObject<Dictionary<int, int>>("mycart");
            if (idList == null) return;
            string _userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            List<int> productIds = idList.Keys.ToList();
            List<Product> products = productIds.Select(id => _context.Products.Find(id)).ToList();

            decimal totalSum = 0;

            foreach (var productId in productIds)
            {
                int quantity = idList[productId];

                var product = products.FirstOrDefault(p => p.Id == productId);

                if (product != null)
                {
                    totalSum += (decimal)(product.Price * quantity);
                }
            }

            Order newOrder = new Order()
            {
                OrderDate = DateTime.Now,
                IdsProduct = JsonSerializer.Serialize(idList),
                TotalPrice = totalSum,
                UserId = _userId
            };
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();
        }
    }
}
