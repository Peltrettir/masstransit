using MassTransit;

namespace Consumers.Concrete
{
    public class UpdatesDefinition : ConsumerDefinition<UpdatesConsumer>
    {
        private readonly ConsumerOptions options;

        public UpdatesDefinition(ConsumerOptions options)
        {
            this.options = options;
            EndpointName = options.Address;
        }
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UpdatesConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}
