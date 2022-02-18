using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;  
using Android.Views;
using Plugin.LocalNotification;
using Android.Support.V4.App;
using Android.Content;
using Плазмателеком;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
[assembly: Dependency(typeof(App1.Droid.MainActivity))]
namespace App1.Droid
{
    
    [Activity(Label = "Плазмателеком", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //Broad myreceiver = new Broad();
        [System.Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStatusBarColor(true);

            PreferAgrmid pr = new PreferAgrmid();
            if (pr.get())
            {
                Intent myIntent = new Intent(this, typeof(PeriodicService));
                PeriodicService p = new PeriodicService();
                p.OnStartCommand(myIntent, StartCommandFlags.Redelivery, TaskId);
            }

            //будильник
            /*{
                var alarmIntent = new Intent(this, typeof(Broad));

                var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

                var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
                alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 20*1000, pending);
            }*/

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        [System.Obsolete]
        public void SetStatusBarColor(bool darkStatusBarTint)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                var activity = Xamarin.Essentials.Platform.CurrentActivity as MainActivity;
                var window = activity?.Window;
                Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#baf54b"));
                if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
                {
                    Window?.InsetsController?.SetSystemBarsAppearance((int)WindowInsetsControllerAppearance.LightStatusBars, (int)WindowInsetsControllerAppearance.LightStatusBars);
                }
            } 
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //будильник
        /*protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(myreceiver, new IntentFilter("com.xamarin.example.TEST"));
            // Code omitted for clarity
        }

        protected override void OnPause()
        {
            UnregisterReceiver(myreceiver);
            // Code omitted for clarity
            base.OnPause();
        }*/
    }
}