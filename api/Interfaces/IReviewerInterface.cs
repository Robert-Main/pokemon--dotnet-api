using api.Dtos;

namespace api.Interfaces;

public interface IReviewerInterface
{
    Task<IEnumerable<Reviewer>> ListAllReviewers();
    Task<Reviewer?> GetReviewerById(int id);
    Task<Reviewer> CreateReviewer(Reviewer reviewer);
    Task<Reviewer?> UpdateReviewer(int id, Reviewer reviewer);
    Task<bool?> DeleteReviewer(int id);
}
