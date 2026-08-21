using api.Dtos;
using api.models;
using AutoMapper;

namespace api.Mapping;

public class PokemonCategoryProfile : Profile
{
    public PokemonCategoryProfile() => CreateMap<PokemonCategory, PokemonCategoryResponseDtos>();
}
