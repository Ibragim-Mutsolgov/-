using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPayments : ContentPage
    {
        String phone;

        List<String> list = new List<string>();

        public ListPayments(String phone)
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
                soap.Label(stack);
            }

            GC.Collect();
        }
        private async Task meth()
        {
            await contractNumbers();
        }
        private async Task contractNumbers()
        {
            Soap soap = new Soap();

            String str;

            String[] str2;

            //2. Определение данных по номеру
            str = await soap.SoapRequest("<ns1:getAccounts><flt><phone>" + phone + "</phone></flt></ns1:getAccounts>");

            if (str == "Не удалось обновить. Повторите попытку.")
            {
                soap.LabelForm(stack);
            }
            else
            {
                //Определение uid
                str2 = soap.xmlDocument(str, "uid");

                if (str2 == null)
                {
                    soap.LabelForm(stack);
                }
                else
                {
                    //3. Определение number по uid
                    str = await soap.SoapRequest("<ns1:getAgreements><flt><userid>" + str2[0] + "</userid></flt></ns1:getAgreements>");

                    if (str == "Не удалось обновить. Повторите попытку.")
                    {
                        soap.LabelForm(stack);
                    }
                    else
                    {
                        String[] ret = soap.xmlDocument(str, "ret");

                        String[] agrmid = soap.xmlDocument(str, "agrmid");

                        String[] number = soap.xmlDocument(str, "number");

                        if (ret == null | number == null | agrmid == null)
                        {
                            soap.LabelForm(stack);
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

                                list.Add(button.GetHashCode().ToString() + "_" + agrmid[i]);

                                button.Clicked += btnNumbers;

                                button.Text = number[i];

                                stack.Children.Add(button);
                            }
                        }
                    }
                }
            }
        }
        private async void btnNumbers(object sender, EventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].StartsWith(sender.GetHashCode().ToString() + "_"))
                {
                    await Shell.Current.Navigation.PushAsync(new ListPay(list[i].Replace(sender.GetHashCode().ToString() + "_", "")));

                    break;
                }
            }
        }
        private async void Refresh_Refreshing(object sender, EventArgs e)
        {
            stack.Children.Clear();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                meth();
            }
            else
            {
                Soap soap = new Soap();
                soap.Label(stack);
            }

            GC.Collect();

             Refresh.IsRefreshing = false;
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}