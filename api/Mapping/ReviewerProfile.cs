using api.Dtos;
using AutoMapper;
using ReviewerModel = api.models.Reviewer;

namespace api.Mapping;

public class ReviewerProfile : Profile
{
    public ReviewerProfile()
    {
        CreateMap<ReviewerModel, Reviewer>();
        CreateMap<Reviewer, ReviewerModel>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.Reviews, options => options.Ignore());
        CreateMap<ReviewerCreateDtos, ReviewerModel>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.Reviews, options => options.Ignore());
    }
}
