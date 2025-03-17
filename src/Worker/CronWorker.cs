using MassTransit;
using Teeitup.Core.Contracts;

namespace Worker;

public class CronWorker : BackgroundService
{
    private readonly ILogger<CronWorker> _logger;
    private readonly IBus _bus;

    public CronWorker(ILogger<CronWorker> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            
            // await _bus.Publish(new GettingStarted("Hello, World!"), stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}