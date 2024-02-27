using GDP_API.Models;

public interface IProjectCategoryRepository
{
    Task<IEnumerable<ProjectCategory>> GetProjectCategories();
    Task<ProjectCategory> GetProjectCategoryById(int id);
    Task<ProjectCategory> CreateProjectCategory(ProjectCategory projectCategory);
    Task<ProjectCategory> UpdateProjectCategory(ProjectCategory projectCategory);
    Task<ProjectCategory> DeleteProjectCategory(int id);
}
