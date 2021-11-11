using BetterAuth.Services;
using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;

namespace BetterAuth.Platforms.Android
{
    public class SmsService : ISmsService
    {
        private readonly IBroadcastingService _broadcastingService;

        public SmsService(IBroadcastingService broadcastingService)
        {
            _broadcastingService = broadcastingService;

            MessagingCenter.Subscribe<Microsoft.Maui.MauiApplication, string>(
                Microsoft.Maui.MauiApplication.Current,
                "OtpReceived",
                async (sender, message) =>
                {
                    await _broadcastingService.BroadcastMessage(new UdpMessage
                    {
                        Type = "Code",
                        Message = ParseCode(message)
                    });
                });
        }

        private string ParseCode(string messageText)
        {
            return messageText;
            // return new Regex(@"code\s*(\d+)").Match(messageText).Groups[1].Value;
        }
    }
}
