using Android.App;
using Plugin.LocalNotification;
using Android.Support.V4.App;
using Xamarin.Forms;
using Android.Graphics;
using Android.OS;
using Android.Content;
using Android.Runtime;
using System;
using System.Threading.Tasks;

[assembly: Dependency(typeof(App1.Droid.PeriodicService))]
namespace App1.Droid
{
    public class PeriodicService : Service
    {
        const string channel_id = "default";
        const string channel_name = "Баланс";
        int notify_index = 0;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Console.WriteLine("я тут4");
            /*NotificationManager manager =
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
            NotificationImage im = new NotificationImage();

            im.FilePath = "pl.png";
            NotificationCompat.Builder builder = new NotificationCompat.Builder(Android.App.Application.Context, channel_id)
                .SetContentTitle("Заголовок")
                .SetContentText("Подзаголовок")
                .SetSmallIcon(int.Parse("pl.png"))
                .SetLargeIcon(BitmapFactory.DecodeResource(Android.App.Application.Context.Resources, Resource.Drawable.notify_panel_notification_icon_bg))
                .SetPriority((int)Android.App.NotificationPriority.High)
                .SetVisibility((int)NotificationVisibility.Public)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);
            Notification notification = builder.Build();*/
            Plugin.LocalNotification.NotificationImage im = new NotificationImage();

            im.FilePath = "pl.png";

            var notification = new NotificationRequest
            {
                BadgeNumber = 1,
                Description = "Ваш баланс ниже 50 рублей",
                Title = "Баланс",
                Image = im,
                NotificationId = 1340,
            };
            notification.ReturningData = "10";
            
            Task.Run(() =>
            {
                while (true)
                {
                    if(DateTime.Now.Hour == 1 & DateTime.Now.Minute == 0 | DateTime.Now.Hour == 17 & DateTime.Now.Minute == 0)
                    {
                        PreferAgrmid pr = new PreferAgrmid();

                        if (pr.get())
                        {
                            notification.Show();
                            //manager.Notify(notify_index++, notification);
                            pr.clear();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            });
            return base.OnStartCommand(intent, flags, startId);
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}