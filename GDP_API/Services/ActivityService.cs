using GDP_API.Models.DTOs;
using GDP_API.Models;

public class ActivityService : IActivityService
{
    private readonly IActivityRepository _repository;
    private readonly IProjectService _projectService;
    private readonly IUserService _userService;
    private readonly ILogger<ActivityService> _logger;
    const string ANF = "Activity not found";
    const string UNF = "User not found";
    const string PDE = "Project does not exist";
    public ActivityService(IActivityRepository repository, IProjectService projectService, IUserService userService, ILogger<ActivityService> logger)
    {
        _repository = repository;
        _projectService = projectService;
        _userService = userService;
        _logger = logger;
    }

    public async Task<Activity> CreateActivity(ActivityDto activityDto)
    {
        try
        {
            if (await _projectService.GetProjectById(activityDto.ProjectId) is null)
            {
                throw new KeyNotFoundException(PDE);
            }
            var activity = BuildFromDto(activityDto);
            return await _repository.CreateActivity(activity);
        }
        catch (KeyNotFoundException e)
        {
            throw new KeyNotFoundException(e.Message);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }

    public async Task<List<Activity>> GetActivities()
    {
        return await _repository.GetActivities();
    }

    public async Task<Activity?> GetActivity(int id)
    {
        _logger.LogInformation($"Getting activity with ID {id}");
        return await _repository.GetActivity(id);
    }

    /// <summary>
    /// Updates an activity with the specified ID using the provided activity DTO.
    /// </summary>
    /// <param name="id">The ID of the activity to update.</param>
    /// <param name="activityDto">The activity DTO containing the updated information.</param>
    /// <returns>The updated activity.</returns>
    public async Task<Activity> UpdateActivity(int id, ActivityDto activityDto)
    {
        try
        {
            Activity? activity = await _repository.GetActivity(id);
            if (activity is null)
            {
                throw new KeyNotFoundException(ANF);
            }
            //This exception should never be thrown as the project should be checked when the activity is created
            //However, it is here as a safety measure
            if (await _projectService.GetProjectById(activity.ProjectId) is null)
            {
                throw new KeyNotFoundException(PDE);
            }
            UpdateFromDto(activity, activityDto);
            return await _repository.UpdateActivity(activity);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task DeleteActivity(int id)
    {
        try
        {
            return _repository.DeleteActivity(id);
        }
        catch (KeyNotFoundException e)
        {
            throw new KeyNotFoundException(e.Message);
        }
    }

    public async Task LinkUserToActivity(int activityId, int userId)
    {
        try
        {
            _logger.LogInformation($"Linking user {userId} to activity {activityId}");
            var user = await _userService.GetUser(userId);
            var activity = await GetActivity(activityId);
            if (user is null)
            {
                throw new KeyNotFoundException(UNF);
            }
            if (activity is null)
            {
                throw new KeyNotFoundException(ANF);
            }
            await _repository.LinkUserToActivity(userId, activityId);
        }
        catch (KeyNotFoundException e)
        {
            throw new KeyNotFoundException(e.Message);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task UnlinkUserFromActivity(int activityId, int userId)
    {
        _logger.LogInformation($"Unlinking user {userId} from activity {activityId}");
        try
        {
            var user = await _userService.GetUser(userId);
            var activity = await GetActivity(activityId);
            if (user is null)
            {
                throw new KeyNotFoundException(UNF);
            }
            if (activity is null)
            {
                throw new KeyNotFoundException(ANF);
            }
            await _repository.UnlinkUserFromActivity(userId, activityId);
        }
        catch (KeyNotFoundException e)
        {
            throw new KeyNotFoundException(e.Message);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }
    #region Private Methods


    private Activity BuildFromDto(ActivityDto activityDto, bool isUpdate = false)
    {
        return new Activity
        {
            Description = activityDto.Description,
            AcceptanceCriteria = activityDto.AcceptanceCriteria,
            RequestedChanges = activityDto.RequestedChanges,
            Status = activityDto.Status,
            StartDate = activityDto.StartDate,
            EndDate = activityDto.EndDate,
            Blockers = activityDto.Blockers,
            Priority = activityDto.Priority,
            ProjectId = activityDto.ProjectId
        };
    }
    private void UpdateFromDto(Activity activity, ActivityDto activityDto)
    {
        activity.Description = activityDto.Description;
        activity.AcceptanceCriteria = activityDto.AcceptanceCriteria;
        activity.RequestedChanges = activityDto.RequestedChanges;
        activity.Status = activityDto.Status;
        activity.StartDate = activityDto.StartDate;
        activity.EndDate = activityDto.EndDate;
        activity.Blockers = activityDto.Blockers;
        activity.Priority = activityDto.Priority;
        //ProjectId should not be updated

    }
    #endregion
}