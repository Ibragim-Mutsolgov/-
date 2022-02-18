using Android.Content;
using Android.Content.PM;
using App1.Views.Payments;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Плазмателеком;

[assembly: Dependency(typeof(AndroidIsInstalApplication))]
namespace Плазмателеком
{
    class AndroidIsInstalApplication : IsInstallApplication
    {
        [Obsolete]
        public async void IsInstall(string packageName)
        {
            try
            {
                var context = Forms.Context as Context;
                PackageInfo info = context.PackageManager.GetPackageInfo(packageName, Android.Content.PM.PackageInfoFlags.Activities);

                PackageManager pm = Android.App.Application.Context.PackageManager;
                Intent intent = pm.GetLaunchIntentForPackage(packageName);
                if (intent != null)
                {

                    intent.SetFlags(ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(intent);
                }
            }
            catch (Android.Content.PM.PackageManager.NameNotFoundException e)
            {
                await Browser.OpenAsync("https://web3.online.sberbank.ru/payments/main", new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.FromHex("#baf54b"),
                    PreferredControlColor = Color.Black
                });
            }
        }
    }
}