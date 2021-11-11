using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BetterAuth.Services;

internal class BroadcastingService : IBroadcastingService
{
    private readonly INotificationService _notificationService;
    private readonly IStorageService _storageService;
    private string _key;

    public BroadcastingService(INotificationService notificationService, IStorageService storageService)
    {
        _notificationService = notificationService;
        _storageService = storageService;
    }

    public async Task<string> GetKey()
    {
        var key = await _storageService.GetValue<string>("userKey");
        if (!string.IsNullOrEmpty(key))
        {
            _key = key;
        }

        return key;
    }

    public Task SaveKey(string key)
    {
        return _storageService.SaveValue("userKey", key);
    }

    public async Task<bool> TestAndSaveKey(string key)
    {
        await SaveKey(key);
        await BroadcastMessage(new UdpMessage
        {
            Type = "Handshake",
            Key = key
        });
        return true;
    }

    public async Task BroadcastMessage(UdpMessage message)
    {
        var key = await GetKey();
        await BroadcastMessage(JsonSerializer.Serialize(message with { Key = key }));
    }

    public async Task BroadcastMessage(string messageText)
    {
        using var client = new UdpClient();
        client.EnableBroadcast = true;
        var endpoint = new IPEndPoint(IPAddress.Broadcast, 15000);
        var message = Encoding.ASCII.GetBytes(messageText);
        await client.SendAsync(message, message.Length, endpoint);
        client.Close();
    }

    public async Task StartListening(Action<string> handshake = null)
    {
        var listener = new UdpListener();
        await listener.StartListening((message) =>
        {
            var messageObj = JsonSerializer.Deserialize<UdpMessage>(message);
            if (messageObj == null)
            {
                return;
            }
            if (messageObj.Type == "Handshake")
            {
                handshake?.Invoke(messageObj.Key);
                _notificationService.ShowNotification("Device connected", $"New device connected with key {messageObj.Key}");
            }
            else if (_key == messageObj.Key)
            {
                _notificationService.ShowNotification("New code received", messageObj.Message);
            }
        });
    }
}
