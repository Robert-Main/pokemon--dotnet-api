using api.Dtos;

namespace api.Interfaces;

public interface IReviewInterface
{
    Task<IEnumerable<Review>> ListAllReviews();
    Task<Review?> GetReviewById(int id);
    Task<Review?> CreateReview(ReviewCreateDtos review);
    Task<Review?> UpdateReview(int id, Review review);
    Task<bool?> DeleteReview(int id);
}
