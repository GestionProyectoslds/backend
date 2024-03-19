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
    public async Task ExpertUserAsync(string jwt)
    {
        // Extract the user ID from the JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(jwt);
        //TODO - Research if claim type can be changed to use a .net constant instead of a string
        //  ClaimTypes.NameIdentifier and  ClaimTypes.Name do not work
        int userIdFromJwt = int.Parse(jwtToken.Claims.First(claim => claim.Type == "unique_name").Value);
        int userTypeFromJwt = int.Parse(jwtToken.Claims.Last(claim => claim.Type == "unique_name").Value);

        if ((userTypeFromJwt == 2) || (userTypeFromJwt == 3))
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IStatisticsRepository>();

                var newStatistics = new DashboardStatisticsDTO
                {
                    ActiveProjectsCount = await repo.ExpertGetActiveProjectsCount(userIdFromJwt),
                    NormalUsersCount = await repo.ExpertGetNormalUsersCount(userIdFromJwt),
                    ExpertUsersCount = await repo.ExpertGetExpertUsersCount(userIdFromJwt),
                    ActivitiesCount = await repo.ExpertGetActivitiesCount(userIdFromJwt),
                    OverdueActivitiesCount = await repo.ExpertGetOverdueActivitiesCount(userIdFromJwt),
                    CompleteProjectsCount = await repo.ExpertGetCompleteProjectsCount(userIdFromJwt),
                    InProgressProjectsCount = await repo.ExpertGetInProgressProjectsCount(userIdFromJwt),
                    ActivitiesCountByStatus = await repo.ExpertGetActivitiesCountByStatus(userIdFromJwt)
                };

                lock (_updateLock)
                {
                    _statistics = newStatistics;
                }
            }
            await StopAsync(_cts.Token);
            return;
        }
        else
        {
            return;
        }
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

    public DashboardStatisticsDTO GetStatistics()
    {
        lock (_updateLock)
        {
            return _statistics;
        }
    }
}