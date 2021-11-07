using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Worker
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureLogging(logging => 
            {
              logging.ClearProviders();
              logging.AddConsole();
            })
            .ConfigureAppConfiguration((hostingContext, config) => {
              var env = hostingContext.HostingEnvironment;

              config
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
 
              if (env.IsDevelopment())
              {
                 config.AddUserSecrets<Settings.FirstBackgroundService>();
                 config.AddUserSecrets<Settings.SecondBackgroundService>();
              }

              config.AddEnvironmentVariables();
            })
            // no need as it works within ConfigureAppConfiguration
            // // .ConfigureHostConfiguration((builder) => {
            // //   builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            // //   var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            // //   builder.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false);
            // //   builder.AddEnvironmentVariables();
            // // })
            .ConfigureServices((hostContext, services) =>
            {
              var configuration = hostContext.Configuration;
              
              services.AddSingleton<Services.IFirstService, Services.FirstService>();
              services.AddSingleton<Services.ISecondService, Services.SecondService>();

              //configuration of option (solution 1)
              services.Configure<Settings.FirstBackgroundService>(configuration.GetSection(nameof(Settings.FirstBackgroundService)));  
              services.Configure<Settings.SecondBackgroundService>(configuration.GetSection(nameof(Settings.SecondBackgroundService)));
              
              //configuration of option (solution 2) - does not work when env = 'Development'
              // services.AddSingleton(configuration.GetSection(nameof(BackgroundServices.FirstBackgroundService)).Get<Settings.FirstBackgroundService>());
              // services.AddSingleton(configuration.GetSection(nameof(BackgroundServices.SecondBackgroundService)).Get<Settings.SecondBackgroundService>()); 

              services.AddHostedService<BackgroundServices.FirstBackgroundService>();
              services.AddHostedService<BackgroundServices.SecondBackgroundService>();
            })
;
  }
}
