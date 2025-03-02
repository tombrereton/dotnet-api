using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Teeitup.Worker.Contracts;

namespace Teeitup.Worker;

public class Worker : BackgroundService
{
    private readonly IBus _bus;

    public Worker(IBus bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _bus.Publish(new GettingStarted("hello"), stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}