using api.Dtos;
using AutoMapper;
using CountryModel = api.models.Country;

namespace api.Mapping;

public class CountryProfile : Profile
{
    public CountryProfile()
    {
        CreateMap<CountryModel, Country>();
        CreateMap<Country, CountryModel>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.Owners, options => options.Ignore());
        CreateMap<CountryCreateDtos, CountryModel>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.Owners, options => options.Ignore());
    }
}
