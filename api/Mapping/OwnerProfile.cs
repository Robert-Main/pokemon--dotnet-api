using api.Dtos;
using AutoMapper;
using OwnerModel = api.models.Owner;

namespace api.Mapping;

public class OwnerProfile : Profile
{
    public OwnerProfile()
    {
        CreateMap<OwnerModel, Owner>();
        CreateMap<OwnerModel, OwnerResponseDtos>();
        CreateMap<Owner, OwnerModel>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.Country, options => options.Ignore())
            .ForMember(destination => destination.PokemonOwners, options => options.Ignore());
        CreateMap<OwnerCreateDtos, OwnerModel>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.Country, options => options.Ignore())
            .ForMember(destination => destination.PokemonOwners, options => options.Ignore());
    }
}
