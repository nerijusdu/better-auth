using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BetterAuth.Services;

internal class UdpListener
{
    private readonly UdpClient _udpClient = new UdpClient(15000);

    public async Task StartListening(Action<string> onReceive)
    {
        while (true)
        {
            var result = await _udpClient.ReceiveAsync();
            var message = Encoding.ASCII.GetString(result.Buffer);
            onReceive(message);
        }
    }
}
