using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNight.WebApi.Services;

/// <summary>
/// 10 dakikada bir tüm health check'leri çalıştırıp sonucu loglar.
/// Docker container'ın ayakta olduğunu doğrulamak için kullanılır.
/// </summary>
public class HealthLoggerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<HealthLoggerBackgroundService> _logger;
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(10);

    public HealthLoggerBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<HealthLoggerBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🟢 HealthLoggerBackgroundService started — will check every {Interval} min", Interval.TotalMinutes);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(Interval, stoppingToken);

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var healthCheckService = scope.ServiceProvider.GetRequiredService<HealthCheckService>();
                var report = await healthCheckService.CheckHealthAsync(stoppingToken);

                if (report.Status == HealthStatus.Healthy)
                {
                    _logger.LogInformation(
                        "✅ Health Check OK | Status: {Status} | DB: {DbStatus} | Uptime: {Uptime}",
                        report.Status,
                        report.Entries.TryGetValue("postgresql", out var dbEntry) ? dbEntry.Status.ToString() : "N/A",
                        GetUptime());
                }
                else
                {
                    foreach (var entry in report.Entries)
                    {
                        _logger.LogWarning(
                            "⚠️ Health Check {Name}: {Status} — {Description}",
                            entry.Key,
                            entry.Value.Status,
                            entry.Value.Description ?? entry.Value.Exception?.Message ?? "no details");
                    }
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Health Check failed with exception");
            }
        }

        _logger.LogInformation("🔴 HealthLoggerBackgroundService stopped");
    }

    private static string GetUptime()
    {
        var uptime = DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime();
        return $"{(int)uptime.TotalHours}h {uptime.Minutes}m {uptime.Seconds}s";
    }
}
