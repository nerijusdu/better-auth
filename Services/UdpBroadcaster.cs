using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BetterAuth.Services;

public static class UdpBroadcaster
{
    public static async Task BroadcastMessage(string messageText)
    {
        using var client = new UdpClient();
        client.EnableBroadcast = true;
        var endpoint = new IPEndPoint(IPAddress.Broadcast, 15000);
        var message = Encoding.ASCII.GetBytes(messageText);
        await client.SendAsync(message, message.Length, endpoint);
        client.Close();
    }
}
