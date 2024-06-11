using Consumers;
using Consumers.Concrete;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

                  services.AddSingleton(_ => new ConsumerOptions
                  {
                      Id = Guid.Parse(args[0]),
                      Name = args[1],
                      Address = args[2]
                  });

                  services.AddHostedService<ConsoleReader>();
              });
}