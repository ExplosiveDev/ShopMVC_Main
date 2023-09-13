using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Org.BouncyCastle.Crypto.Macs;
using ShopMVC.Helper;
using ShopMVC.Interfaces;
using ShopMVC.Models;
using System.Security.Claims;
using System.Text.Json;

namespace ShopMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly IOrdersService _ordersService;
        private readonly IStorageService _storageService;
        private readonly IMailService _mailService;
        public OrderController(IOrdersService cordersService, IProductsService productsService, IStorageService storageService, IMailService mailService) 
        {
            _ordersService = cordersService;
            _productsService = productsService;
            _storageService = storageService;
            _mailService = mailService;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var viewModel = await _ordersService.GetAll();
            return View(viewModel);
        }
        public async Task <IActionResult> CreateOrder(List<int> productId, List<int> quantity) 
        {
            await _ordersService.Create();
            await _storageService.UpdateQuantityDecrease(productId, quantity);

            await _mailService.SendMailAsync("Your Order", "password" ,"gmail.com", "@gmail.com");

            HttpContext.Session.Remove("mycart");
            return RedirectToAction(nameof(Index));
        }
    }
}