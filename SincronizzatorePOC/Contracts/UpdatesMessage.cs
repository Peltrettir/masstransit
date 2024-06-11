namespace Contracts
{
    public record UpdatesMessage
    {
        public string? SenderName { get; init; }
        public string? Message { get; init; }
    }
}
