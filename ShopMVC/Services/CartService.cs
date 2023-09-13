using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Entities;
using Newtonsoft.Json;
using ShopMVC.Helper;
using ShopMVC.Interfaces;
using ShopMVC.Models;

namespace ShopMVC.Services
{
    public class CartService : ICartService
    {
        private readonly HttpContext _httpContext;
        private readonly IProductsService _productsService;
        private readonly IStorageService _storageService;
        public CartService(IHttpContextAccessor httpContext, IProductsService productsService, IStorageService storageService)
        {
            _httpContext = httpContext.HttpContext;
            _productsService = productsService;
            _storageService = storageService;
        }
        public async Task<List<ProductCartViewModel>> GetProducts()
        {
            Dictionary<int, int> IdCountProducts = _httpContext.Session.GetObject<Dictionary<int, int>>("mycart");
            if (IdCountProducts == null) IdCountProducts = new Dictionary<int, int>();
            List<ProductCartViewModel> products = new List<ProductCartViewModel>();
            foreach (var idProduct in IdCountProducts.Keys)
            {
                var product = await _productsService.Get(idProduct);
                products.Add(new ProductCartViewModel
                {
                    Product = product,
                    IsInCart = true,
                    Quantity = IdCountProducts[idProduct]
                });
            }
            return products;
        }
        public async Task Create(int id)
        {
            var product = await _productsService.Get(id);
            if (product == null)
            {
                return;
            }

            var cartJson = _httpContext.Session.GetString("mycart");
            Dictionary<int, int> cart;

            if (!string.IsNullOrEmpty(cartJson))
            {
                cart = JsonConvert.DeserializeObject<Dictionary<int, int>>(cartJson);
            }
            else
            {
                cart = new Dictionary<int, int>();
            }

            var existingItem = cart.ContainsKey(id);
            if (existingItem)
            {
                var res = cart[id];
                cart[id]++;

            }
            else
            {
                cart[id] = 1;
            }

            _httpContext.Session.SetString("mycart", JsonConvert.SerializeObject(cart));
        }
        public async Task Delete(int id)
        {
            var product = await _productsService.Get(id);
            if (product == null)
            {
                return;
            }

            var cartJson = _httpContext.Session.GetString("mycart");
            Dictionary<int, int> cart;

            if (!string.IsNullOrEmpty(cartJson))
            {
                cart = JsonConvert.DeserializeObject<Dictionary<int, int>>(cartJson);
            }
            else
            {
                cart = new Dictionary<int, int>();
            }

            var existingItem = cart.ContainsKey(id);

            if (existingItem)
            {
                if (cart[id] > 1)
                {
                    cart[id]--;
                }
                else
                {
                    cart.Remove(id);
                }
            }
            _httpContext.Session.SetString("mycart", JsonConvert.SerializeObject(cart));
        }
        public async Task UpdateQuantity(int productId, int quantity)
        {
            var product = await _productsService.Get(productId);
            //var productPrice = product.Price;
            var productSorage = await _storageService.Get(productId);
            if (product == null || productSorage == null)
            {
                return;
            }

            if (productSorage.ProductQuantity >= quantity)
            {
                var cartJson = _httpContext.Session.GetString("mycart");
                Dictionary<int, int> cart;

                if (!string.IsNullOrEmpty(cartJson))
                {
                    cart = JsonConvert.DeserializeObject<Dictionary<int, int>>(cartJson);
                }
                else
                {
                    cart = new Dictionary<int, int>();
                }

                var existingItem = cart.ContainsKey(productId);

                if (existingItem)
                {
                    cart[productId] = quantity;
                }
                _httpContext.Session.SetString("mycart", JsonConvert.SerializeObject(cart));
            }
        }
    }
}
