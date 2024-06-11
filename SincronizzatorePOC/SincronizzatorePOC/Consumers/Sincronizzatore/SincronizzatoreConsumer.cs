using Consumers;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace SincronizzatorePOC.Consumers.HelloWorld
{
    public class SincronizzatoreConsumer : IConsumer<UpdatesMessage>
    {
        readonly IBus _bus;
        private readonly List<ConsumerOptions> _consumers;
        readonly ILogger<SincronizzatoreConsumer> _logger;

        public SincronizzatoreConsumer(IBus bus, List<ConsumerOptions> consumers, ILogger<SincronizzatoreConsumer> logger)
        {
            _bus = bus;
            _consumers = consumers;
            _logger = logger;
        }


        public Task Consume(ConsumeContext<UpdatesMessage> context)
        {
            var senderOptions = _consumers.FirstOrDefault(c => c.Id == context.CorrelationId);
            if (senderOptions == null)
            {
                _logger.LogError("Unknown sender {id}", context.CorrelationId);
                return Task.CompletedTask;
            }

            var consumerConfigTask = _consumers.Where(c => c.Id != context.CorrelationId)
                                               .Select(async opt => new ConsumerSendEndpoint(opt, await _bus.GetSendEndpoint(new Uri($"queue:{opt.Address}"))));

            Task.WaitAll(consumerConfigTask.ToArray(), context.CancellationToken);

            var sendChannels = consumerConfigTask.Select(t => t.Result).ToList();

            _logger.LogInformation("Message received from {ConsumerName}: {Message}\r\nForwading to {ConsumerNames}",
                                    senderOptions.Name,
                                    context.Message.Message,
                                    string.Join(", ", sendChannels.Select(c => c.Options.Name)));

            Task.WaitAll(sendChannels.Select(async c => await c.SendEndpoint.Send(new UpdatesMessage 
            {
                SenderName = senderOptions.Name,
                Message = context.Message.Message
            }, ctx => ctx.CorrelationId = context.CorrelationId)).ToArray());

            return Task.CompletedTask;
        }
    }
}
