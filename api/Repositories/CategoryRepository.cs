using api.Data;
using api.Dtos;
using api.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CategoryModel = api.models.Category;

namespace api.Repositories;

public class CategoryRepository(DataContext context, IMapper mapper) : ICategoryInterface
{
    public async Task<IEnumerable<Category>> ListAllCategories() =>
        mapper.Map<IEnumerable<Category>>(await context.Categories.AsNoTracking().ToListAsync());

    public async Task<Category?> GetCategoryById(int id) =>
        mapper.Map<Category>(await context.Categories.AsNoTracking().FirstOrDefaultAsync(category => category.Id == id));

    public async Task<Category> CreateCategory(CategoryCreateDtos category)
    {
        var entity = mapper.Map<CategoryModel>(category);
        context.Categories.Add(entity);
        await context.SaveChangesAsync();
        return mapper.Map<Category>(entity);
    }

    public async Task<Category?> UpdateCategory(int id, Category category)
    {
        var entity = await context.Categories.FindAsync(id);
        if (entity is null) return null;
        mapper.Map(category, entity);
        await context.SaveChangesAsync();
        return mapper.Map<Category>(entity);
    }

    public async Task<bool?> DeleteCategory(int id)
    {
        var entity = await context.Categories.FindAsync(id);
        if (entity is null) return null;
        context.Categories.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
