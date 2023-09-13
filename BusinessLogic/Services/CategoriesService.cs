using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Data;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesService(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task Create(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            await _categoryRepository.Insert(category);
            await _categoryRepository.Save();
        }
        public async Task Delete(int id)
        {
            var category = await _categoryRepository.GetByID(id);
            if (category == null) return;

            await Task.Run(
            () =>
            {
                _categoryRepository.Delete(category);
            });
            await _categoryRepository.Save();
        }
        public async Task Update(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            await _categoryRepository.Update(category);
            await _categoryRepository.Save();
        }
        public async Task<CategoryDTO?> Get(int id)
        {
            var category = GetAll().Result.FirstOrDefault(p => p.Id == id);
            return _mapper.Map<CategoryDTO>(category);
        }
        public async Task<List<CategoryDTO>> GetAll()
        {
            var category = _categoryRepository.Get().ToList();
            return _mapper.Map<List<CategoryDTO>>(category);
        }
    }
}
