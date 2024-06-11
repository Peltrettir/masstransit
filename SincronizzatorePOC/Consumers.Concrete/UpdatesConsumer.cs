using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Consumers.Concrete
{
    public class UpdatesConsumer :IConsumer<UpdatesMessage>
    {
        readonly ILogger<UpdatesConsumer> _logger;

        public UpdatesConsumer(ILogger<UpdatesConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<UpdatesMessage> context)
        {
            _logger.LogInformation("Message received from {ConsumerName}: {Message}", context.Message.SenderName, context.Message.Message);
            return Task.CompletedTask;
        }
    }
}
