namespace Company.Consumers
{
    using MassTransit;

    public class Masstransit_GettingStartedConsumerDefinition :
        ConsumerDefinition<Masstransit_GettingStartedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<Masstransit_GettingStartedConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}