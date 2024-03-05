using GDP_API.Models.DTOs;
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