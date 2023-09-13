using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Data;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopMVC.Interfaces;
using ShopMVC.Models;

namespace ShopMVC.Services
{
    public class ProductShowService : IProductShowService
    {
        private readonly ShopMVCDbContext _context;
        private readonly IProductsService _productsService;
        private readonly IStorageService _storageService;
        private readonly ICategoriesService _categoriesService;
        public ProductShowService(IProductsService productsService, IStorageService storageService, ICategoriesService categoriesService, ShopMVCDbContext context)
        {
            _productsService = productsService;
            _storageService = storageService;
            _context = context;
            _categoriesService = categoriesService;
        }

        public async Task<List<HomePageViewModel>> CreateMainPageAsync(int? category_id)
        {
            return null;
        }
        public async Task<List<ProductCartViewModel>> GetTop4ProductsAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            var productSalesCount = new Dictionary<int, int>();

            foreach (var order in orders)
            {
                var idList = JsonConvert.DeserializeObject<Dictionary<int, int>>(order.IdsProduct);
                foreach (var kvp in idList)
                {
                    var productId = kvp.Key;
                    var quantity = kvp.Value;

                    if (productSalesCount.ContainsKey(productId))
                    {
                        productSalesCount[productId] += quantity;
                    }
                    else
                    {
                        productSalesCount[productId] = quantity;
                    }
                }
            }
            var top4Products = productSalesCount.OrderByDescending(kvp => kvp.Value)
                                               .Take(4)
                                               .ToList();

            var top4ProductData = new List<ProductCartViewModel>();
            foreach (var kvp in top4Products)
            {
                var productId = kvp.Key;
                var product = await _productsService.Get(productId);
                if (product != null)
                {
                    var productViewModel = new ProductCartViewModel
                    {
                        Product = product,
                        IsInCart = false,
                        Quantity = 0,
                        StorageQuantity = GetStorageQuantityForProduct(productId)
                    };

                    top4ProductData.Add(productViewModel);
                }
            }
            return top4ProductData;
        }
        private int GetStorageQuantityForProduct(int productId)
        {
            var storageInfo = _storageService.Get(productId).Result;
            return storageInfo?.ProductQuantity ?? 0;
        }
    }
}
