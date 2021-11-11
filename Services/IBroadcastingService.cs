using System;
using System.Threading.Tasks;

namespace BetterAuth.Services;

public interface IBroadcastingService
{
    Task<bool> TestAndSaveKey(string key);
    Task<string> GetKey();
    Task SaveKey(string key);
    Task BroadcastMessage(string messageText);
    Task BroadcastMessage(UdpMessage message);
    Task StartListening(Action<string> handshake = null);
}
