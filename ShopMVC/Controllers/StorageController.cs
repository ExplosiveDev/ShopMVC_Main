using BusinessLogic.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopMVC.Helper;
using ShopMVC.Models;

namespace ShopMVC.Controllers
{
    public class StorageController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IStorageService _storageService;

        public StorageController(IProductsService productsService, ICategoriesService categoriesService, IStorageService storageService)
        {
            _productsService = productsService;
            _categoriesService = categoriesService;
            _storageService = storageService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productsService.GetAll();
            var productsCartViewModel = products.Select(p => new ProductCartViewModel
            {
                Product = p,
                IsInCart = IsProductInCart(p.Id),
                StorageQuantity = GetStorageQuantityForProduct(p.Id)
            }).ToList();

            return View(productsCartViewModel);

        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var product = await _storageService.UpdateQuantity(productId, quantity);
            return RedirectToAction("Index", product);   
        }
        private bool IsProductInCart(int id)
        {
            Dictionary<int, int> IdCount = HttpContext.Session.GetObject<Dictionary<int, int>>("mycart");
            if (IdCount == null) return false;
            return IdCount.ContainsKey(id);
        }
        private int GetStorageQuantityForProduct(int productId)
        {
            var quantity = _storageService.GetStorageQuantityForProduct(productId);
            if (quantity != null )
            {
                return quantity.Result;
            }
            return 0; 
        }
    }
}
