using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopMVC.Helper;
using ShopMVC.Interfaces;
using ShopMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShopMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _cartService.GetProducts();
            return View(products);
        }
        public async Task<IActionResult> Add(int id)
        {
            await _cartService.Create(id);
            return RedirectToAction(nameof(Index), "Cart");
        }

        public async Task <IActionResult> Remove(int id)
        {
            await _cartService.Delete(id);
            return RedirectToAction(nameof(Index), "Cart");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            if(quantity > 0)
            {
                await _cartService.UpdateQuantity(productId, quantity);
            }
            return RedirectToAction(nameof(Index), "Cart");
        }
    }
}
