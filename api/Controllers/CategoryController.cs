using api.Dtos;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryInterface categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> ListAllCategories() => Ok(new { message = "Categories retrieved successfully", data = await categoryService.ListAllCategories() });

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategoryById(int id) =>
        await categoryService.GetCategoryById(id) is { } category ? Ok(new { message = "Category retrieved successfully", data = category }) : NotFound(new { message = "Category not found" });

    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(CategoryCreateDtos category)
    {
        var created = await categoryService.CreateCategory(category);
        return CreatedAtAction(nameof(GetCategoryById), new { id = created.Id }, new { message = "Category created successfully", data = created });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Category>> UpdateCategory(int id, Category category) =>
        await categoryService.UpdateCategory(id, category) is { } updated ? Ok(new { message = "Category updated successfully", data = updated }) : NotFound(new { message = "Category not found" });

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id) =>
        await categoryService.DeleteCategory(id) is not null ? Ok(new { message = "Category deleted successfully" }) : NotFound(new { message = "Category not found" });
}
