using BusinessLogic.DTOs;
using DataAccess.Entities;

namespace BusinessLogic.Mapper
{
    public class MapperProfile:AutoMapper.Profile
    {

        public MapperProfile()
        {
            CreateMap<Product, ProductDTO>().ForMember(dto => dto.CategoryName, opt => opt.MapFrom(o => o.Category!.Name));
            CreateMap<Product, ProductDTO>().ForMember(dto => dto.StorageQuantity, opt => opt.MapFrom(o => o.Storage!.ProductQuantity));
            CreateMap<Product, ProductDTO>().ForMember(dto => dto.Image, opt => opt.Ignore());

            CreateMap<ProductDTO, Product>();
            CreateMap<CreateProductDTO, Product>()
                    .ForPath(dest => dest.Category!.Name, opt => opt.MapFrom(src => src.CategoryName))
                    .ForPath(dest => dest.Storage!.ProductQuantity, opt => opt.MapFrom(src => src.StorageQuantity));
            CreateMap<Order, OrderDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
        }
    }
}
