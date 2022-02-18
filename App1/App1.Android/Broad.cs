//будильник
/*using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.App;
using Android.Widget;
using System;
using App1;

namespace Плазмателеком
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "com.xamarin.example.TEST" })]
    class Broad : BroadcastReceiver
    {
        const string channel_id = "default";
        const string channel_name = "Баланс";
        int notify_index = 0;
        public async override void OnReceive(Context context, Intent intent)
        {
            App1.PreferAgrmid id;
            NotificationManager manager =
                (NotificationManager)Android.App.Application.Context.GetSystemService(Android.App.Application.NotificationService);
            var java_channel_name = new Java.Lang.String(channel_name);
            var channel = new NotificationChannel(
                channel_id,
                java_channel_name,
                NotificationImportance.High)
            {
                Description = "Channel Discription",
            };
            manager.CreateNotificationChannel(channel);
            NotificationCompat.Builder builder = new NotificationCompat.Builder(Android.App.Application.Context, channel_id)
                .SetContentTitle("Заголовок")
                .SetContentText("Подзаголовок")
                .SetSmallIcon(Resource.Drawable.notification_action_background)
                .SetLargeIcon(BitmapFactory.DecodeResource(Android.App.Application.Context.Resources, Resource.Drawable.notify_panel_notification_icon_bg))
                .SetPriority((int)Android.App.NotificationPriority.High)
                .SetVisibility((int)NotificationVisibility.Public)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);
            Notification notification = builder.Build();
            manager.Notify(notify_index++, notification);
        }
    }
}*/