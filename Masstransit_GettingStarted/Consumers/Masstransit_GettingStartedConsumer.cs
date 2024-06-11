namespace Company.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Microsoft.Extensions.Logging;

    public class Masstransit_GettingStartedConsumer :
        IConsumer<HelloWorldMessage>
    {
        readonly ILogger<Masstransit_GettingStartedConsumer> _logger;

        public Masstransit_GettingStartedConsumer(ILogger<Masstransit_GettingStartedConsumer> logger)
        {
            _logger = logger;
        }


        public Task Consume(ConsumeContext<HelloWorldMessage> context)
        {
            _logger.LogInformation("[x] Message received: {Message}", context.Message.Value);
            return Task.CompletedTask;
        }
    }
}