using App1.Views.Payments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayNumbers : ContentPage
    {
        String phone;

        List<String> list = new List<string>();

        public PayNumbers(String phone)
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);

            if (Device.RuntimePlatform == "iOS")
            {
                btnBack.Source = ImageSource.FromResource("App1.Views.Images.Back.png");
                heightRow.Height = 205;
            }
            else
            {
                btnBack.Source = "Back.png";
            }

            this.phone = phone;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                meth();
            }
            else
            {
                Soap soap = new Soap();
                soap.Label(stackNumbers);
            }

            GC.Collect();
        }
        private async Task meth()
        {
            await setNumbers();
        }
        private async Task setNumbers()
        {
            Soap soap = new Soap();

            String str;

            String[] str2;

            //2. Определение данных по номеру
            str = await soap.SoapRequest("<ns1:getAccounts><flt><phone>" + phone + "</phone></flt></ns1:getAccounts>");

            if (str.Equals("Не удалось обновить. Повторите попытку.") | str == null)
            {
                soap.LabelForm(stackNumbers);
            }
            else
            {
                str2 = soap.xmlDocument(str, "uid");

                if (str2 == null)
                {
                    soap.LabelForm(stackNumbers);
                }
                else
                {
                    //3. Определение number по uid
                    str = await soap.SoapRequest("<ns1:getAgreements><flt><userid>" + str2[0] + "</userid></flt></ns1:getAgreements>");

                    if (str.Equals("Не удалось обновить. Повторите попытку."))
                    {
                        soap.LabelForm(stackNumbers);
                    }
                    else
                    {
                        String[] ret = soap.xmlDocument(str, "ret");

                        String[] number = soap.xmlDocument(str, "number");

                        if (ret == null | number == null)
                        {
                            soap.LabelForm(stackNumbers);
                        }
                        else
                        {
                            for (int i = 0; i < ret.Length; i++)
                            {
                                Button button = new Button();

                                button.CornerRadius = 10;

                                button.FontFamily = "akzi";

                                button.BackgroundColor = Color.FromHex("#baf54b");

                                button.TextColor = Color.Black;

                                list.Add(button.GetHashCode().ToString() + "_" + number[i]);

                                button.Clicked += onPay;

                                button.Text = number[i];

                                stackNumbers.Children.Add(button);
                            }
                        }
                    }
                }
            }
        }
        private async void onPay(object sender, EventArgs e)
        {
            DependencyService.Get<IsInstallApplication>().IsInstall("ru.sberbankmobile");
        }
        

        private async void Refresh_Refreshing(object sender, EventArgs e)
        {
            stackNumbers.Children.Clear();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                meth();
            }
            else
            {
                Soap soap = new Soap();
                soap.Label(stackNumbers);
            }

            GC.Collect();

            Refresh.IsRefreshing = false;
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}