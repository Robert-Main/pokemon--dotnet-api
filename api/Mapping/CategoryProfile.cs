using api.Dtos;
using AutoMapper;
using CategoryModel = api.models.Category;

namespace api.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryModel, Category>();
        CreateMap<Category, CategoryModel>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.PokemonCategories, options => options.Ignore());
        CreateMap<CategoryCreateDtos, CategoryModel>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.PokemonCategories, options => options.Ignore());
    }
}
