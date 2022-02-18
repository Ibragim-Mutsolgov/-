using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PromisePayments : ContentPage
    {
        String phone;

        List<String> list = new List<string>();

        public PromisePayments(String phone)
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);

            btnBack.Source = "Back.png";

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
                soap.Label(stackPromisePayments);
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

            if (str.Equals("Не удалось обновить. Повторите попытку."))
            {
                soap.LabelForm(stackPromisePayments);
            }
            else
            {
                str2 = soap.xmlDocument(str, "uid");

                if (str2 == null)
                {
                    soap.LabelForm(stackPromisePayments);
                }
                else
                {
                    //3. Определение number по uid
                    str = await soap.SoapRequest("<ns1:getAgreements><flt><userid>" + str2[0] + "</userid></flt></ns1:getAgreements>");

                    if (str.Equals("Не удалось обновить. Повторите попытку."))
                    {
                        soap.LabelForm(stackPromisePayments);
                    }
                    else
                    {
                        String[] ret = soap.xmlDocument(str, "ret");

                        String[] agrmid = soap.xmlDocument(str, "agrmid");

                        String[] number = soap.xmlDocument(str, "number");

                        if (ret == null | number == null | agrmid == null)
                        {
                            soap.LabelForm(stackPromisePayments);
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

                                button.Clicked += onPay;

                                button.Text = number[i];

                                stackPromisePayments.Children.Add(button);
                            }
                        }
                    }
                }
            }
        }
        private async void onPay(object sender, EventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].StartsWith(sender.GetHashCode().ToString() + "_"))
                {
                    await Promisepayments(list[i].Replace(sender.GetHashCode().ToString() + "_", ""));

                    break;
                }
            }
        }
        private async Task Promisepayments(String agrmid)
        {
            Soap soap = new Soap();

            String str;

            String[] str2;

            //Проверка есть ли не истекший обещанный платеж
            str = await soap.SoapRequest("<ns1:getPromisePayments><flt><agrmid>" + agrmid + "</agrmid></flt></ns1:getPromisePayments>");

            if (str.Equals("Не удалось обновить. Повторите попытку."))
            {
                await DisplayAlert("", "Не удалось подключить услугу \"Обещанный платеж\". Обратитесь в техническую поддержку.", "Закрыть");
            }
            else
            {
                str2 = soap.xmlDocument(str, "promtill");

                if (str2 == null)
                {
                    str = await soap.SoapRequest("<ns1:PromisePayment><agrm>" + agrmid + "</agrm><summ>100</summ></ns1:PromisePayment>");

                    if (str.Equals("Не удалось обновить. Повторите попытку."))
                    {
                        await DisplayAlert("", "Не удалось подключить услугу \"Обещанный платеж\". Обратитесь в техническую поддержку.", "Закрыть");
                    }
                    else
                    {
                        str2 = soap.xmlDocument(str, "ret");

                        if (str2 == null)
                        {
                            await DisplayAlert("", "Не удалось подключить услугу \"Обещанный платеж\". Обратитесь в техническую поддержку.", "Закрыть");
                        }
                        else
                        {
                            if (str2[0] == "1")
                            {
                                await DisplayAlert("", "Услуга \"Обещанный платеж\" подключена.", "Закрыть");
                            }
                            else
                            {
                                await DisplayAlert("", "Не удалось подключить услугу \"Обещанный платеж\". Обратитесь в техническую поддержку.", "Закрыть");
                            }
                        }
                    }
                }
                else
                {
                    String promtill = str2[str2.Length - 1];

                    DateTime prom = DateTime.Parse(promtill);

                    DateTime dateNow = DateTime.Now;

                    if (dateNow < prom)
                    {
                        await DisplayAlert("", "Услуга \"Обещанный платеж\" активирована. Вторично активирована быть не может.", "Закрыть");
                    }
                    else if (dateNow > prom)
                    {
                        str = await soap.SoapRequest("<ns1:PromisePayment><agrm>" + agrmid + "</agrm><summ>100</summ></ns1:PromisePayment>");

                        if (str.Equals("Не удалось обновить. Повторите попытку."))
                        {
                            await DisplayAlert("", "Не удалось подключить услугу \"Обещанный платеж\". Обратитесь в техническую поддержку.", "Закрыть");
                        }
                        else
                        {
                            str2 = soap.xmlDocument(str, "ret");

                            if (str2 == null)
                            {
                                await DisplayAlert("", "Не удалось подключить услугу \"Обещанный платеж\". Обратитесь в техническую поддержку.", "Закрыть");
                            }
                            else
                            {
                                if (str2[0] == "1")
                                {
                                    await DisplayAlert("", "Услуга \"Обещанный платеж\" подключена.", "Закрыть");
                                }
                                else
                                {
                                    await DisplayAlert("", "Не удалось подключить услугу \"Обещанный платеж\". Обратитесь в техническую поддержку.", "Закрыть"); ;
                                }
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("", "Услуга \"Обещанный платеж\" активирована. Вторично активирована быть не может.", "Закрыть");
                    }
                }
            }
        }
        private async void Refresh_Refreshing(object sender, EventArgs e)
        {
            stackPromisePayments.Children.Clear();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                meth();
            }
            else
            {
                Soap soap = new Soap();
                soap.Label(stackPromisePayments);
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