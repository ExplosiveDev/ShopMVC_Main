using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLogic.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IFilesService _filesService;
        public ProductsService(IRepository<Product> productRepository, IRepository<Category> categoryRepository, IMapper mapper, IFilesService filesService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _filesService = filesService;
        }
        public async Task Create(CreateProductDTO createProductDTO)
        {
            var product = _mapper.Map<Product>(createProductDTO);
            product.ImagePath = _filesService.SaveProductImage(createProductDTO.Image).Result;
            product.Category = await _categoryRepository.GetByID(createProductDTO.CategoryId);
            await _productRepository.Insert(product);
            await _productRepository.Save();
        }
        public async Task Delete(int id)
        {
            var product = await _productRepository.GetByID(id);
            if (product == null) return;
            await Task.Run(
            () =>
            {
                _filesService.DeleteProductImage(product.ImagePath!);
                _productRepository.Delete(product);
            });
            await _productRepository.Save();
        }
        public async Task Update(ProductDTO ProductDTO)
        {
            var baseProduct = _productRepository.GetByID(ProductDTO.Id).Result;
            if (baseProduct != null)
            {
                baseProduct.ImagePath = await _filesService.UpdateProductImage(baseProduct.ImagePath!, ProductDTO.Image);
                await _productRepository.Update(baseProduct);
                await _productRepository.Save();
            }
        }
        public async Task<ProductDTO?> Get(int? id)
        {
            return GetAll().Result.FirstOrDefault(p => p.Id == id);
        }
        public async Task<List<ProductDTO>> GetAllByPrice()
        {
            var products = _productRepository.Get(orderBy: q => q.OrderBy(p => p.Price), includeProperties: new[] { "Category" }).ToList();
            return _mapper.Map<List<ProductDTO>>(products);
            //return _productRepository.Get(orderBy: q => q.OrderBy(p => p.Price), includeProperties: new[] { "Category" }).ToList();
        }
        public async Task<List<ProductDTO>> GetAll()
        {
            var products = _productRepository.Get(includeProperties: new[] { "Category" }).ToList();
            return _mapper.Map<List<ProductDTO>>(products);
        }
        //public async Task<List<Category>> GetAllCategory()
        //{
        //    return _categoryRepository.Get().ToList();
        //}
    }
}
