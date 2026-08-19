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
            CreateMap<api.models.Review, ReviewResponseDtos>();
            CreateMap<api.models.Reviewer, api.Dtos.Reviewer>();
            CreateMap<api.models.PokemonCategory, PokemonCategoryResponseDtos>();
            CreateMap<api.models.Category, api.Dtos.Category>();
            CreateMap<api.models.PokemonOwner, PokemonOwnerResponseDtos>();
            CreateMap<api.models.Owner, OwnerResponseDtos>();
            CreateMap<api.models.Country, api.Dtos.Country>();
            CreateMap<api.Dtos.Category, api.models.Category>()
                .ForMember(destination => destination.Id, options => options.Ignore())
                .ForMember(destination => destination.PokemonCategories, options => options.Ignore());
            CreateMap<api.Dtos.Country, api.models.Country>()
                .ForMember(destination => destination.Id, options => options.Ignore())
                .ForMember(destination => destination.Owners, options => options.Ignore());
            CreateMap<api.Dtos.Owner, api.models.Owner>()
                .ForMember(destination => destination.Id, options => options.Ignore())
                .ForMember(destination => destination.Country, options => options.Ignore())
                .ForMember(destination => destination.PokemonOwners, options => options.Ignore());
            CreateMap<api.models.Owner, api.Dtos.Owner>();
            CreateMap<api.Dtos.Reviewer, api.models.Reviewer>()
                .ForMember(destination => destination.Id, options => options.Ignore())
                .ForMember(destination => destination.Reviews, options => options.Ignore());
            CreateMap<api.Dtos.Review, api.models.Review>()
                .ForMember(destination => destination.Id, options => options.Ignore())
                .ForMember(destination => destination.Reviewer, options => options.Ignore())
                .ForMember(destination => destination.Pokemon, options => options.Ignore());
            CreateMap<api.models.Review, api.Dtos.Review>();
        }
    }
}
