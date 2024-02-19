using GDP_API.Models;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectService(IProjectRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Project>> GetAllProjects()
    {
        return _repository.GetAllProjects();
    }

    public Task<Project> GetProjectById(int id)
    {
        return _repository.GetProjectById(id);
    }

    public Task<Project> CreateProject(Project project)
    {
        return _repository.CreateProject(project);
    }

    public Task UpdateProject(Project project)
    {
        return _repository.UpdateProject(project);
    }

    public Task DeleteProject(int id)
    {
        return _repository.DeleteProject(id);
    }
}