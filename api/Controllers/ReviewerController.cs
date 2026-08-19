using api.Dtos;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewerController(IReviewerInterface reviewerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reviewer>>> ListAllReviewers() => Ok(new { message = "Reviewers retrieved successfully", data = await reviewerService.ListAllReviewers() });

    [HttpGet("{id}")]
    public async Task<ActionResult<Reviewer>> GetReviewerById(int id) =>
        await reviewerService.GetReviewerById(id) is { } reviewer ? Ok(new { message = "Reviewer retrieved successfully", data = reviewer }) : NotFound(new { message = "Reviewer not found" });

    [HttpPost]
    public async Task<ActionResult<Reviewer>> CreateReviewer(Reviewer reviewer)
    {
        var created = await reviewerService.CreateReviewer(reviewer);
        return CreatedAtAction(nameof(GetReviewerById), new { id = created.Id }, new { message = "Reviewer created successfully", data = created });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Reviewer>> UpdateReviewer(int id, Reviewer reviewer) =>
        await reviewerService.UpdateReviewer(id, reviewer) is { } updated ? Ok(new { message = "Reviewer updated successfully", data = updated }) : NotFound(new { message = "Reviewer not found" });

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReviewer(int id) =>
        await reviewerService.DeleteReviewer(id) is not null ? Ok(new { message = "Reviewer deleted successfully" }) : NotFound(new { message = "Reviewer not found" });
}
