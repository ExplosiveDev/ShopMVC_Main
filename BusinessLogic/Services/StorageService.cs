using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Data;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class StorageService : IStorageService
    {
        private readonly IRepository<Storage> _storageRepository;
        private readonly IRepository<Product> _productRepository;
        public StorageService(IRepository <Storage> storageRepository, IRepository<Product> productRepository)
        {
            _storageRepository = storageRepository;
            _productRepository = productRepository;
        }
        public async Task Create(Storage storage)
        {
            await _storageRepository.Insert(storage);
            await _storageRepository.Save();
        }
        public Storage CreateByQuantity(Product product, int quantity)
        {
            Storage storage = new Storage
            {
                ProductId = product.Id,
                Products = product,
                ProductQuantity = quantity
            };
            return storage;
        }
        public async Task<Storage?> Get(int? id)
        {
            return GetAll().Result.FirstOrDefault(p => p.ProductId == id);
        }
        public async Task <Product> UpdateQuantity(int productId, int quantity)
        {
            var product = await _productRepository.GetByID(productId); 
            if (product != null)
            {
                var existingStorage = await Get(productId);
                if (existingStorage == null)
                {
                    Storage storage = CreateByQuantity(product, quantity);
                    await Create(storage);
                    product.StorageId = storage.Id;
                    product.Storage = storage;
                }
                else
                {
                    existingStorage.ProductQuantity += quantity;
                    await Update(existingStorage);
                    product.StorageId = existingStorage.Id;
                }

                var newpoduct = _productRepository.Update(product);
            }
            return product;
        }
        public async Task UpdateQuantityDecrease(List<int> productId, List<int> quantity)
        {
            for (int i = 0; i < productId.Count; i++)
            {
                int currentProductId = productId[i];
                int currentQuantity = quantity[i];

                var product = await _productRepository.GetByID(currentProductId);
                if (product != null)
                {
                    var existingStorage = await Get(currentProductId);
                    if (existingStorage == null)
                    {
                        Storage storage = CreateByQuantity(product, currentQuantity);
                        await Create(storage);
                        product.StorageId = storage.Id;
                        product.Storage = storage;
                    }
                    else
                    {
                        existingStorage.ProductQuantity -= currentQuantity;
                        await Update(existingStorage);
                        product.StorageId = existingStorage.Id;
                    }
                    await _productRepository.Update(product);
                }
            }
        }
        public async Task Delete(int id)
        {
            var storage = await _storageRepository.GetByID(id);
            if (storage == null) return;
            await Task.Run(
            () =>
            {
                _storageRepository.Delete(storage);
            });
            await _storageRepository.Save();
        }
        public async Task<List<Storage>> GetAll()
        {
            return _storageRepository.Get().ToList();
        }
        public async Task Update(Storage storage)
        {
            await _storageRepository.Update(storage);
            await _storageRepository.Save();
        }
        public async Task <int> GetStorageQuantityForProduct (int idProduct)
        {
            var storage = Get(idProduct);
            return storage.Result != null ? storage.Result.ProductQuantity : 0;
        }
    }
}
