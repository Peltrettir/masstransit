using Consumers;
using MassTransit;

namespace SincronizzatorePOC.Consumers.HelloWorld
{
    public class SincronizzatoreDefinition : ConsumerDefinition<SincronizzatoreConsumer>
    {
        public SincronizzatoreDefinition()
        {
            EndpointName = "updates";
        }
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SincronizzatoreConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}
