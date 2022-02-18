using Android.Content;
using App1.Views;
using App1.Views.Setting;
using Plugin.LocalNotification;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            tr();

            if (Device.RuntimePlatform == "iOS")
            {
                mainImage.Source = ImageSource.FromResource("App1.Views.Images.pl.png");
                cl.Source = ImageSource.FromResource("App1.Views.Images.Close.png");
                user.Source = ImageSource.FromResource("App1.Views.Images.User.png");
                ser.Source = ImageSource.FromResource("App1.Views.Images.service.png");
                tar.Source = ImageSource.FromResource("App1.Views.Images.tarif.png");
                set.Source = ImageSource.FromResource("App1.Views.Images.sets.png");
                ex.Source = ImageSource.FromResource("App1.Views.Images.exit.png");
            }

            Username();


            /*
            var t = Task.Run(async delegate
            {
                await Task.Delay(20000);
                notification.send("Баланс", "Ваш баланс меньше 50 рублей");
            });
            
            */
            

            /*
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
            notification.Show();*/
        }


        public async Task<int> balance(string phone)
        {
            Soap soap = new Soap();

            String str;

            String[] str2;

            //2. Определение данных по номеру
            str = await soap.SoapRequest("<ns1:getAccounts><flt><phone>" + phone + "</phone></flt></ns1:getAccounts>");

            if (str != "Не удалось обновить. Повторите попытку.")
            {
                str2 = soap.xmlDocument(str, "uid");

                if (str2 != null)
                {
                    //3. Определение number по uid
                    str = await soap.SoapRequest("<ns1:getAgreements><flt><userid>" + str2[0] + "</userid></flt></ns1:getAgreements>");

                    if (str != "Не удалось обновить. Повторите попытку.")
                    {
                        string[] agrmid = soap.xmlDocument(str, "agrmid");
                        if (agrmid != null)
                        {
                            for (int i = 0; i < agrmid.Length; i++)
                            {
                                str = await soap.SoapRequest("<ns1:getVgroups><flt><agrmid>" + agrmid[i] + "</agrmid><archive>0</archive><agentid>1</agentid></flt></ns1:getVgroups>");

                                if (str != null)
                                {
                                    //int[] balance = soap.intDocument(str, "balance");
                                    return 0;
                                }
                            }
                        }
                    }
                }
            }
            return -1000000;
        }

        private void tr()
        {
            MessagingCenter.Subscribe<Page>(
            this, // кто подписывается на сообщения
            "open",   // название сообщения
            (sender) => { FlyoutIsPresented = true; });    // вызываемое действие
            
            MessagingCenter.Subscribe<Page>(
            this, // кто подписывается на сообщения
            "unsub",   // название сообщения
                (sender) => {
                MessagingCenter.Unsubscribe<Page>(
                subscriber: this,
                "open");
            });

            
        }

        private async void Username()
        {
            Prefer pr = new Prefer();

            Soap soap = new Soap();

            String str;

            String[] str2;

            //2. Определение данных по номеру
            str = await soap.SoapRequest("<ns1:getAccounts><flt><phone>" + pr.getName() + "</phone></flt></ns1:getAccounts>");

            if (!str.Equals("Не удалось обновить. Повторите попытку."))
            {
                //Определение uid
                str2 = soap.xmlDocument(str, "uid");

                if (str2 != null)
                {
                    //3. Определение number по uid
                    str = await soap.SoapRequest("<ns1:getAgreements><flt><userid>" + str2[0] + "</userid></flt></ns1:getAgreements>");

                    if (!str.Equals("Не удалось обновить. Повторите попытку."))
                    {
                        str2 = soap.xmlDocument(str, "username");

                        if (str2 != null)
                        {
                            lblUsername.Text = str2[0];
                        }
                        else
                        {
                            lblUsername.Text = "Не удалось обновить.";
                        }
                    }
                    else
                    {
                        lblUsername.Text = "Не удалось обновить.";
                    }
                }
                else
                {
                    lblUsername.Text = "Не удалось обновить.";
                }
            }
            else
            {
                lblUsername.Text = "Не удалось обновить.";
            }

        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            Prefer p = new Prefer();

            p.clear();

            DependencyService.Get<Interface1>().delete();

            MessagingCenter.Send<Page>(this, "unsub");

            Navigation.RemovePage(this);

            await Shell.Current.GoToAsync("//LoginPage");

            GC.Collect();
            //await Shell.Current.GoToAsync("//LoginPage");
        }

        private void cl_Clicked(object sender, EventArgs e)
        {
            FlyoutIsPresented = false;
        }

        private async void btnMyRoom_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//AboutPage");
            FlyoutIsPresented = false;
        }

        private async void Myservice_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("", "Чтобы сменить тариф, Вам необходимо позвонить в службу технической поддержки по номеру 8 (928) 919 55 50", "Назад", "Позвонить");

            if (!answer)
            {
                try
                {
                    PhoneDialer.Open("+79289195550");
                }
                catch (ArgumentNullException anEx)
                {
                    await DisplayAlert("Неверно введен номер", "", "Закрыть");
                }
                catch (FeatureNotSupportedException ex)
                {
                    await DisplayAlert("Не поддерживается на Вашем устройстве", "", "Закрыть");
                }
                catch (Exception ex)
                {
                    // Other error has occurred.
                }
            }
        }

        private async void btnSettings_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Fl");
            FlyoutIsPresented = false;
        }
    }
}
