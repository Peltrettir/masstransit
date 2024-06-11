namespace Consumers
{
    public record ConsumerOptions()
    {
        public Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Address { get; init; }
    }
}
