using MassTransit;

namespace Consumers
{
    public record ConsumerSendEndpoint(ConsumerOptions Options, ISendEndpoint SendEndpoint);
}
