using api.Dtos;

namespace api.Interfaces;

public interface ICategoryInterface
{
    Task<IEnumerable<Category>> ListAllCategories();
    Task<Category?> GetCategoryById(int id);
    Task<Category> CreateCategory(CategoryCreateDtos category);
    Task<Category?> UpdateCategory(int id, Category category);
    Task<bool?> DeleteCategory(int id);
}
