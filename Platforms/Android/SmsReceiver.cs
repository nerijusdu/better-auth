using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Telephony;
using Java.Lang;
using Microsoft.Maui.Controls;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BetterAuth.Platforms.Android;

[BroadcastReceiver(Enabled = true, Label = "SMS Receiver", Exported = true)]
[IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED", Intent.CategoryDefault })]
public class SmsReceiver : BroadcastReceiver
{
    private const string IntentAction = "android.provider.Telephony.SMS_RECEIVED";
    private static readonly string Sender = "";
    private static readonly string[] OtpMessageBodyKeywordSet = { "verification", "code", "authentication" };

    public override void OnReceive(Context context, Intent intent)
    {
        try
        {
            if (intent.Action != IntentAction) return;
            var bundle = intent.Extras;
            if (bundle == null) return;
            var pdus = bundle.Get("pdus");
            var castedPdus = JNIEnv.GetArray<Java.Lang.Object>(pdus.Handle);
            var msgs = new SmsMessage[castedPdus.Length];
            var sb = new StringBuilder();
            string sender = null;
            for (var i = 0; i < msgs.Length; i++)
            {
                var bytes = new byte[JNIEnv.GetArrayLength(castedPdus[i].Handle)];
                JNIEnv.CopyArray(castedPdus[i].Handle, bytes);
                var format = bundle.GetString("format");
                msgs[i] = SmsMessage.CreateFromPdu(bytes, format);
                if (sender == null)
                {
                    sender = msgs[i].OriginatingAddress;
                }
                sb.Append(string.Format("SMS From: {0}{1}Body: {2}{1}", msgs[i].OriginatingAddress, System.Environment.NewLine, msgs[i].MessageBody));

                var msgBody = msgs[i].MessageBody;
                // TODO: if (!sender.Contains(Sender)) return;
                var foundKeyword = OtpMessageBodyKeywordSet.Any(k => msgBody.Contains(k));
                //if (!foundKeyword) return;

                MessagingCenter.Send(Microsoft.Maui.MauiApplication.Current, "OtpReceived", msgBody);
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
