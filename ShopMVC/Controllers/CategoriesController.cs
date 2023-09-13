using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;
using DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using BusinessLogic.DTOs;

namespace ShopMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            List<CategoryDTO> categories = await _categoriesService.GetAll();
            return View(categories);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            var category = await _categoriesService.Get((int)id);
            return View(category);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] CategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                await _categoriesService.Create(categoryDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(categoryDTO);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _categoriesService.GetAll() == null)
            {
                return NotFound();
            }

            var category = await _categoriesService.Get((int)id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] CategoryDTO categoryDTO)
        {
            await _categoriesService.Update(categoryDTO);
            return View(categoryDTO);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            var category = await _categoriesService.Get((int)id);
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _categoriesService.GetAll() == null)
            {
                return Problem("Entity set 'ShopMVCContext.Category'  is null.");
            }
            var category = await _categoriesService.Get((int)id);
            if (category != null)
            {
                await _categoriesService.Delete(category.Id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
