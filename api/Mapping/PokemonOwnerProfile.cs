using api.Dtos;
using api.models;
using AutoMapper;

namespace api.Mapping;

public class PokemonOwnerProfile : Profile
{
    public PokemonOwnerProfile() => CreateMap<PokemonOwner, PokemonOwnerResponseDtos>();
}
