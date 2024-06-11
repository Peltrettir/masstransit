using Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SincronizzatorePOC;
using System.Reflection;

public class Program
{

    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .ConfigureLogging(logBuilder =>
              {
                  logBuilder.AddConsole();
              })
              .ConfigureAppConfiguration(cfgBuilder =>
              {
                  cfgBuilder.SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("config.json", false, true)
                            .Build();
              })
              .ConfigureServices((hostContext, services) =>
              {

                  services.AddMassTransit(x =>
                  {
                      x.SetKebabCaseEndpointNameFormatter();

                      var entryAssembly = Assembly.GetEntryAssembly();

                      x.AddConsumers(entryAssembly);
                      x.AddSagaStateMachines(entryAssembly);
                      x.AddSagas(entryAssembly);
                      x.AddActivities(entryAssembly);

                      x.UsingRabbitMq((context, cfg) =>
                      {
                          cfg.Host("localhost", "/", hostContext =>
                          {
                              hostContext.Username("guest");
                              hostContext.Password("guest");
                          });

                          cfg.ConfigureEndpoints(context);
                      });
                  });

                  services.AddHostedService<ConsumersLauncher>();
                  services.AddSingleton(_ => hostContext.Configuration.GetSection("consumers").Get<List<ConsumerOptions>>() ?? throw new ArgumentNullException("Missing consumers list from config"));
              });
}