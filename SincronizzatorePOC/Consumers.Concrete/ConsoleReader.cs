using Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace Consumers.Concrete
{
    public class ConsoleReader : BackgroundService
    {
        private readonly ConsumerOptions options;
        private readonly IBus bus;

        public ConsoleReader(ConsumerOptions options, IBus bus)
        {
            this.options = options;
            this.bus = bus;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = Console.ReadLine();
                if(!string.IsNullOrEmpty(message))
                {
                    await Process(message);
                }
            }
        }

        private async Task Process(string message)
        {
            var endpoint = await bus.GetSendEndpoint(new Uri($"queue:updates"));
            await endpoint.Send(new UpdatesMessage { Message = message }, ctx => ctx.CorrelationId = options.Id);

            Console.WriteLine($"Processing {message}");
        }
    }
}
