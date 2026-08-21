using api.Data;
using api.Dtos;
using api.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReviewerModel = api.models.Reviewer;

namespace api.Repositories;

public class ReviewerRepository(DataContext context, IMapper mapper) : IReviewerInterface
{
    public async Task<IEnumerable<Reviewer>> ListAllReviewers() =>
        mapper.Map<IEnumerable<Reviewer>>(await context.Reviewers.AsNoTracking().ToListAsync());

    public async Task<Reviewer?> GetReviewerById(int id) =>
        mapper.Map<Reviewer>(await context.Reviewers.AsNoTracking().FirstOrDefaultAsync(reviewer => reviewer.Id == id));

    public async Task<Reviewer> CreateReviewer(ReviewerCreateDtos reviewer)
    {
        var entity = mapper.Map<ReviewerModel>(reviewer);
        context.Reviewers.Add(entity);
        await context.SaveChangesAsync();
        return mapper.Map<Reviewer>(entity);
    }

    public async Task<Reviewer?> UpdateReviewer(int id, Reviewer reviewer)
    {
        var entity = await context.Reviewers.FindAsync(id);
        if (entity is null) return null;
        mapper.Map(reviewer, entity);
        await context.SaveChangesAsync();
        return mapper.Map<Reviewer>(entity);
    }

    public async Task<bool?> DeleteReviewer(int id)
    {
        var entity = await context.Reviewers.FindAsync(id);
        if (entity is null) return null;
        context.Reviewers.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
