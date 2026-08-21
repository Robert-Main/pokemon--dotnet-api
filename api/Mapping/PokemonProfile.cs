using api.Dtos;
using api.models;
using AutoMapper;

namespace api.Mapping
{
    public class PokemonProfile : Profile
    {
        public PokemonProfile()
        {
            CreateMap<PokemonCreateDtos, Pokemon>()
                .ForMember(destination => destination.Id, options => options.Ignore())
                .ForMember(destination => destination.CreatedAt, options => options.Ignore())
                .ForMember(destination => destination.Reviews, options => options.Ignore())
                .ForMember(destination => destination.PokemonCategories, options => options.Ignore())
                .ForMember(destination => destination.PokemonOwners, options => options.Ignore());
            CreateMap<PokemonDtos, Pokemon>()
                .ForMember(destination => destination.Id, options => options.Ignore())
                .ForMember(destination => destination.Reviews, options => options.Ignore())
                .ForMember(destination => destination.PokemonCategories, options => options.Ignore())
                .ForMember(destination => destination.PokemonOwners, options => options.Ignore());
            CreateMap<Pokemon, PokemonDtos>();
            CreateMap<Pokemon, PokemonResponseDtos>();
        }
    }
}
