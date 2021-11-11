namespace BetterAuth.Services;

public record UdpMessage
{
    public string Type { get; set; }
    public string Key { get; set; }
    public string Message { get; set; }
}
