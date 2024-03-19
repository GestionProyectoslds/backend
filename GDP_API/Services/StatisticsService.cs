using GDP_API;
using GDP_API.Models;
using GDP_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
public class StatisticsService : IHostedService, IStatisticsService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private DashboardStatisticsDTO _statistics;
    private readonly object _updateLock = new object();
    private Task? _backgroundTask;
    private CancellationTokenSource? _cts;

    public StatisticsService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;

        _statistics = new DashboardStatisticsDTO
        {
            ActiveProjectsCount = 0,
            NormalUsersCount = 0,
            ExpertUsersCount = 0,
            ActivitiesCount = 0,
            OverdueActivitiesCount = 0,
            ActivitiesCountByStatus = new List<StatusCountDTO>()
        };
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cts = new CancellationTokenSource();
        _backgroundTask = ExecuteAsync(_cts.Token);
        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts!.Cancel();
        return Task.WhenAny(_backgroundTask!, Task.Delay(-1, cancellationToken));
    }

    protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IStatisticsRepository>();

                var newStatistics = new DashboardStatisticsDTO
                {
                    ActiveProjectsCount = await repo.GetActiveProjectsCount(),
                    NormalUsersCount = await repo.GetNormalUsersCount(),
                    ExpertUsersCount = await repo.GetExpertUsersCount(),
                    ActivitiesCount = await repo.GetActivitiesCount(),
                    OverdueActivitiesCount = await repo.GetOverdueActivitiesCount(),
                    CompleteProjectsCount = await repo.GetCompleteProjectsCount(),
                    InProgressProjectsCount = await repo.GetInProgressProjectsCount(),
                    ActivitiesCountByStatus = await repo.GetActivitiesCountByStatus()
                };

                lock (_updateLock)
                {
                    _statistics = newStatistics;
                }
            }

            try
            {
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }
    }

    public async Task<DashboardStatisticsDTO> NonAdminUserAsync(string jwt)
    {
        // Extract the user ID from the JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(jwt);
        int userIdFromJwt = int.Parse(jwtToken.Claims.First(claim => claim.Type == "unique_name").Value);
        UserType userTypeFromJwt = (UserType)Enum.Parse(typeof(UserType), jwtToken.Claims.Last(claim => claim.Type == "role").Value);

        if (userTypeFromJwt == UserType.Expert || userTypeFromJwt == UserType.Admin)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IStatisticsRepository>();

                var newStatistics = new DashboardStatisticsDTO
                {
                    ActiveProjectsCount = await repo.GetActiveProjectsCount(userIdFromJwt),
                    NormalUsersCount = await repo.GetNormalUsersCount(userIdFromJwt),
                    ExpertUsersCount = await repo.GetExpertUsersCount(userIdFromJwt),
                    ActivitiesCount = await repo.GetActivitiesCount(userIdFromJwt),
                    OverdueActivitiesCount = await repo.GetOverdueActivitiesCount(userIdFromJwt),
                    CompleteProjectsCount = await repo.GetCompleteProjectsCount(userIdFromJwt),
                    InProgressProjectsCount = await repo.GetInProgressProjectsCount(userIdFromJwt),
                    ActivitiesCountByStatus = await repo.GetActivitiesCountByStatus(userIdFromJwt)
                };

                return newStatistics;
            }
        }

        // Default return statement
        return new DashboardStatisticsDTO
        {
            ActiveProjectsCount = 0,
            NormalUsersCount = 0,
            ExpertUsersCount = 0,
            ActivitiesCount = 0,
            OverdueActivitiesCount = 0,
            CompleteProjectsCount = 0,
            InProgressProjectsCount = 0,
            ActivitiesCountByStatus = new List<StatusCountDTO>()
        };
    }

    public async Task<DashboardStatisticsDTO> GetStatistics(string jwt)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(jwt);
        var userTypeFromJwt = jwtToken.Claims.Last(claim => claim.Type == "role").Value;

        if (Enum.TryParse(userTypeFromJwt, out UserType userType))
        {
            if (userType == UserType.Expert || userType == UserType.Normal)
            {
                return await NonAdminUserAsync(jwt);
            }

            else if (userType == UserType.Admin)
            {
                return _statistics;
            }
        }

        return new DashboardStatisticsDTO
        {
            ActiveProjectsCount = 0,
            NormalUsersCount = 0,
            ExpertUsersCount = 0,
            ActivitiesCount = 0,
            OverdueActivitiesCount = 1,
            ActivitiesCountByStatus = new List<StatusCountDTO>()
        };
    }

}