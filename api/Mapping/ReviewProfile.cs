using api.Dtos;
using AutoMapper;
using ReviewModel = api.models.Review;

namespace api.Mapping;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<ReviewModel, Review>();
        CreateMap<ReviewModel, ReviewResponseDtos>();
        CreateMap<Review, ReviewModel>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.Reviewer, options => options.Ignore())
            .ForMember(destination => destination.Pokemon, options => options.Ignore());
        CreateMap<ReviewCreateDtos, ReviewModel>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.Reviewer, options => options.Ignore())
            .ForMember(destination => destination.Pokemon, options => options.Ignore());
    }
}
