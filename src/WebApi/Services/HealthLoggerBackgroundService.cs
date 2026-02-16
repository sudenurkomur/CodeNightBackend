using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeNight.WebApi.Services;

public class HealthLoggerBackgroundService : BackgroundService
{
    private readonly ILogger<HealthLoggerBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(10);

    public HealthLoggerBackgroundService(
        ILogger<HealthLoggerBackgroundService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Health Logger Background Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_interval, stoppingToken);

            if (stoppingToken.IsCancellationRequested) break;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var healthCheckService = scope.ServiceProvider.GetRequiredService<HealthCheckService>();
                var report = await healthCheckService.CheckHealthAsync(stoppingToken);

                var status = report.Status.ToString();
                var dbStatus = report.Entries.TryGetValue("PostgreSQL", out var dbEntry)
                    ? dbEntry.Status.ToString()
                    : "N/A";

                var uptime = DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime();

                if (report.Status == HealthStatus.Healthy)
                {
                    _logger.LogInformation(
                        "✅ Health Check OK | Status: {Status} | DB: {DbStatus} | Uptime: {Uptime}",
                        status, dbStatus, uptime);
                }
                else
                {
                    _logger.LogWarning(
                        "⚠️ Health Check WARNING | Status: {Status} | DB: {DbStatus} | Uptime: {Uptime}",
                        status, dbStatus, uptime);

                    foreach (var entry in report.Entries)
                    {
                        _logger.LogWarning("   - {Name}: {Status} - {Description}",
                            entry.Key, entry.Value.Status, entry.Value.Description);
                    }
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Health check logging failed.");
            }
        }

        _logger.LogInformation("Health Logger Background Service stopped.");
    }
}
