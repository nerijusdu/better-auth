using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Telephony;
using BetterAuth.Models;
using BetterAuth.Services;
using Java.Lang;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetterAuth.Platforms.Android;

[BroadcastReceiver(Enabled = true, Label = "SMS Receiver", Exported = true)]
[IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED", Intent.CategoryDefault })]
public class SmsReceiver : BroadcastReceiver
{
    private const string IntentAction = "android.provider.Telephony.SMS_RECEIVED";

    public override async void OnReceive(Context context, Intent intent)
    {

        try
        {
            if (intent.Action != IntentAction) return;

            var bundle = intent.Extras;
            if (bundle == null) return;

            var pdus = bundle.Get("pdus");
            var castedPdus = JNIEnv.GetArray<Java.Lang.Object>(pdus.Handle);
            var msgs = new SmsMessage[castedPdus.Length];
            string sender = null;

            var storageService = new StorageService();
            var settings = storageService.GetValue<SmsSettings>(Constants.SmsSettings).Result;
            var key = await storageService.GetValue<string>(Constants.UserKey);

            for (var i = 0; i < msgs.Length; i++)
            {
                var bytes = new byte[JNIEnv.GetArrayLength(castedPdus[i].Handle)];
                JNIEnv.CopyArray(castedPdus[i].Handle, bytes);
                var format = bundle.GetString("format");
                msgs[i] = SmsMessage.CreateFromPdu(bytes, format);
                sender ??= msgs[i].OriginatingAddress;
                var msgBody = msgs[i].MessageBody;

                if (string.IsNullOrEmpty(key) || !MatchSenderAdnKeywords(sender, msgBody, settings))
                {
                    return;
                }

                await UdpBroadcaster.BroadcastMessage(JsonSerializer.Serialize(new UdpMessage
                {
                    Key = key,
                    Message = ParseCode(msgBody),
                    Type = Constants.MsgTypeCode
                }));
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private bool MatchSenderAdnKeywords(string sender, string msgBody, SmsSettings settings)
    {
        var foundSender = (settings?.Senders ?? Array.Empty<string>()).Contains(sender.ToLower());
        var foundKeywords = (settings?.Keywords ?? Array.Empty<string>()).Any(k => msgBody.ToLower().Contains(k));
        return foundSender && foundKeywords;
    }

    private string ParseCode(string messageText)
    {
        return new Regex(@"(\d+)").Match(messageText).Groups[1].Value;
    }
}
