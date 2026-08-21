using api.Dtos;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController(IReviewInterface reviewService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> ListAllReviews() => Ok(new { message = "Reviews retrieved successfully", data = await reviewService.ListAllReviews() });

    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetReviewById(int id) =>
        await reviewService.GetReviewById(id) is { } review ? Ok(new { message = "Review retrieved successfully", data = review }) : NotFound(new { message = "Review not found" });

    [HttpPost]
    public async Task<ActionResult<Review>> CreateReview(ReviewCreateDtos review)
    {
        var created = await reviewService.CreateReview(review);
        return created is null
            ? BadRequest(new { message = "Pokemon or reviewer not found" })
            : CreatedAtAction(nameof(GetReviewById), new { id = created.Id }, new { message = "Review created successfully", data = created });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Review>> UpdateReview(int id, Review review)
    {
        var updated = await reviewService.UpdateReview(id, review);
        return updated is not null ? Ok(new { message = "Review updated successfully", data = updated }) : NotFound(new { message = "Review, pokemon, or reviewer not found" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id) =>
        await reviewService.DeleteReview(id) is not null ? Ok(new { message = "Review deleted successfully" }) : NotFound(new { message = "Review not found" });
}
