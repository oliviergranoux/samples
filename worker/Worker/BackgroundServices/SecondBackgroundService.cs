using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Worker.BackgroundServices
{
    public class SecondBackgroundService : BackgroundService
    {
        private readonly ILogger<SecondBackgroundService> _logger;

        private readonly Settings.SecondBackgroundService _options;

        private readonly Services.ISecondService _service;

        public SecondBackgroundService(ILogger<SecondBackgroundService> logger, IOptions<Settings.SecondBackgroundService> options, Services.ISecondService service)
        {
            _logger = logger;
            _options = options.Value;
            _service = service;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting service '{_options.Name}'");
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var DOTNET_ENVIRONMENT = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            var ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            _logger.LogDebug($"ENVIRONMENT: '{DOTNET_ENVIRONMENT}', '{ASPNETCORE_ENVIRONMENT}'");

            _logger.LogDebug($"Option ==>  Name '{_options.Name}'; IsEnable '{_options.IsEnable}'");
            if (!_options.IsEnable)
            {
                _logger.LogInformation($"service disabled");
                return;
            }
            
            while (!cancellationToken.IsCancellationRequested)
            {
                await _service.ExecuteAsync(cancellationToken);
                await Task.Delay(_options.DelayInSeconds*1000, cancellationToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                _logger.LogInformation($"Cancellation required");

            _logger.LogInformation($"Stopping service...");
            await base.StopAsync(cancellationToken);
        }

    }
}
