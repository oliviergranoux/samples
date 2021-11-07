using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Worker.Services
{
  public class FirstService : IFirstService
  {
    private readonly ILogger<FirstService> _logger;
    
    public FirstService(ILogger<FirstService> logger)
    { 
      _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
      if (cancellationToken.IsCancellationRequested)
      {
        _logger.LogInformation($"Cancellation required");
      }

      await HelloWorldAsync(cancellationToken);
    }

    private Task HelloWorldAsync(CancellationToken cancellationToken)
    {
      return Task.Run(() => {
        Task.Delay(3000, cancellationToken);
        _logger.LogInformation($"Execute HelloWorld at '{DateTimeOffset.Now}'");
      });
    }
  }
}