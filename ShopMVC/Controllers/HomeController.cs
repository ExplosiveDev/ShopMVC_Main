using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopMVC.Helper;
using ShopMVC.Interfaces;
using ShopMVC.Models;
using System.Diagnostics;

namespace ShopMVC.Controllers
{
    //[Controller]
    //public class Home
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductShowService _productShowService;
        private readonly ICategoriesService _categoriesService;
        private readonly IProductsService _productsService;
        private readonly IStorageService _storageService;
        public HomeController(ILogger<HomeController> logger, ICategoriesService categoriesService, IProductsService productsService, IStorageService storageService, IProductShowService productShowService)
        {
            _logger = logger;
            _categoriesService = categoriesService;
            _productsService = productsService;
            _storageService = storageService;
            _productShowService = productShowService;
        }
        public async Task<IActionResult> Index(int? category_id)
        {
            List<CategoryDTO> categories = await _categoriesService.GetAll();
            categories.Insert(0, new CategoryDTO { Id = 0, Name = "All", Description = "All Products" });
            ViewBag.ListCategories = categories;
            ViewData["ListCategories"] = categories;
            var products = _productsService.GetAll().Result;
            if (category_id != null && category_id > 0)
            {
                products = products.Where(p => p.CategoryId == category_id).ToList();
            }

            var allProducts = products.Select(
                p => new ProductCartViewModel
                {
                    Product = p,
                    IsInCart = IsProductInCart(p.Id),
                    StorageQuantity = GetStorageQuantityForProduct(p.Id)
                }
                ).ToList();
            if (category_id == null)
            {
                ViewBag.ActiveCategoryId = 0;
            }
            else
            {
                ViewBag.ActiveCategoryId = category_id;
            }

            var topProducts = await GetTop4ProductsAsync();
            return View(new HomePageViewModel
            {
                AllProducts = allProducts,
                TopProducts = topProducts
            });
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            var service = HttpContext.RequestServices.GetServices<UserManager<User>>();
            return View(service);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private async Task<List<ProductCartViewModel>> GetTop4ProductsAsync()
        {
            var top4ProductData = await _productShowService.GetTop4ProductsAsync();
            return top4ProductData;
        }
        private bool IsProductInCart(int id) {
            Dictionary<int, int> IdCount = HttpContext.Session.GetObject<Dictionary<int, int>>("mycart");
            if (IdCount == null) return false;
            return IdCount.ContainsKey(id);
        }
        private int GetStorageQuantityForProduct(int productId)
        {
            var storageInfo = _storageService.Get(productId).Result;
            return storageInfo?.ProductQuantity ?? 0;
        }
    }
}