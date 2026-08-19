using api.Data;
using api.Dtos;
using api.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReviewModel = api.models.Review;

namespace api.Repositories;

public class ReviewRepository(DataContext context, IMapper mapper) : IReviewInterface
{
    public async Task<IEnumerable<Review>> ListAllReviews() =>
        mapper.Map<IEnumerable<Review>>(await context.Reviews.AsNoTracking().ToListAsync());

    public async Task<Review?> GetReviewById(int id) =>
        mapper.Map<Review>(await context.Reviews.AsNoTracking().FirstOrDefaultAsync(review => review.Id == id));

    public async Task<Review?> CreateReview(Review review)
    {
        if (!await ReferencesExist(review)) return null;
        var entity = mapper.Map<ReviewModel>(review);
        context.Reviews.Add(entity);
        await context.SaveChangesAsync();
        return mapper.Map<Review>(entity);
    }

    public async Task<Review?> UpdateReview(int id, Review review)
    {
        if (!await ReferencesExist(review)) return null;
        var entity = await context.Reviews.FindAsync(id);
        if (entity is null) return null;
        mapper.Map(review, entity);
        await context.SaveChangesAsync();
        return mapper.Map<Review>(entity);
    }

    public async Task<bool?> DeleteReview(int id)
    {
        var entity = await context.Reviews.FindAsync(id);
        if (entity is null) return null;
        context.Reviews.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> ReferencesExist(Review review) =>
        await context.Reviewers.AnyAsync(reviewer => reviewer.Id == review.ReviewerId)
        && await context.Pokemons.AnyAsync(pokemon => pokemon.Id == review.PokemonId);
}
