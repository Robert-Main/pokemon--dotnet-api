using api.Dtos;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnerController(IOwnerInterface ownerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Owner>>> ListAllOwners() => Ok(new { message = "Owners retrieved successfully", data = await ownerService.ListAllOwners() });

    [HttpGet("{id}")]
    public async Task<ActionResult<Owner>> GetOwnerById(int id) =>
        await ownerService.GetOwnerById(id) is { } owner ? Ok(new { message = "Owner retrieved successfully", data = owner }) : NotFound(new { message = "Owner not found" });

    [HttpPost]
    public async Task<ActionResult<Owner>> CreateOwner(OwnerCreateDtos owner)
    {
        var created = await ownerService.CreateOwner(owner);
        return created is null
            ? BadRequest(new { message = "Country not found" })
            : CreatedAtAction(nameof(GetOwnerById), new { id = created.Id }, new { message = "Owner created successfully", data = created });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Owner>> UpdateOwner(int id, Owner owner)
    {
        var updated = await ownerService.UpdateOwner(id, owner);
        return updated is not null ? Ok(new { message = "Owner updated successfully", data = updated }) : NotFound(new { message = "Owner or country not found" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOwner(int id) =>
        await ownerService.DeleteOwner(id) is not null ? Ok(new { message = "Owner deleted successfully" }) : NotFound(new { message = "Owner not found" });
}
