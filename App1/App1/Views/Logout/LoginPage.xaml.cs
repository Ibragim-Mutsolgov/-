using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            tr();

            MessagingCenter.Send<Page>(this, "close");

            NavigationPage.SetHasNavigationBar(this, false);

            if (Device.RuntimePlatform == "iOS")
            {
                image.Source = ImageSource.FromResource("App1.Views.Images.pl.png");
                box1.IsVisible = false;
                box2.IsVisible = false;
            }

            GC.Collect();
        }
        private void tr()
        {
            MessagingCenter.Send<Page>(this, "closePresent");
        }
        private async void onLogin(object sender, EventArgs e)
        {
            if (login.Text != "" & login.Text != null & pass.Text != "" & pass.Text != null)
            {

                char[] ch = login.Text.ToCharArray();
                bool tr = false;
                for (int i = 0; i < ch.Length; i++)
                {
                    if (ch[i].ToString() == "@")
                    {
                        tr = true;
                        break;
                    }
                }
                //Для email
                if (tr)
                {
                    if (login.Text == "@" | pass.Text == "@")
                    {
                        errorLabel();
                    }
                    else
                    {
                        await email(login.Text);
                    }

                }
                //Для телефона
                else if (login.Text.Length >= 10)
                {
                    try
                    {
                        long i = long.Parse(login.Text);

                        String numberPhone;
                        if (login.Text.Length == 10)
                        {
                            await Meth(login.Text);
                        }
                        else if (login.Text.Length == 11)
                        {
                            if (login.Text.StartsWith("7") | login.Text.StartsWith("8"))
                            {
                                numberPhone = login.Text.Substring(1);
                                await Meth(numberPhone);
                            }
                        }
                        else if (login.Text.Length == 12)
                        {
                            if (login.Text.StartsWith("+7"))
                            {
                                numberPhone = login.Text.Substring(2);
                                await Meth(numberPhone);
                            }
                        }
                        else
                        {
                            login.Text = "";

                            pass.Text = "";

                            lblFalsePhoneOrPass.IsVisible = true;
                        }
                    }
                    catch (FormatException ex)
                    {
                        errorLabel();
                    }
                }
                //Для номера договора
                else
                {
                    try
                    {
                        long i = long.Parse(login.Text);
                        await numberDogovor(login.Text);
                    }
                    catch (FormatException ex)
                    {
                        errorLabel();
                    }
                }
            }
            else
            {
                errorLabel();
            }
        }
        private async Task Meth(String numberPhone)
        {
            Soap soap = new Soap();

            string str;

            string[] str2;

            //2. Определение данных по номеру
            str = await soap.SoapRequest("<ns1:getAccounts><flt><phone>" + numberPhone + "</phone></flt></ns1:getAccounts>");

            if (str == "Не удалось обновить. Повторите попытку.")
            {
                errorLabel();
            }
            else
            {
                //Определение uid
                str2 = soap.xmlDocument(str, "uid");

                if (str2 == null)
                {
                    errorLabel();
                }
                else
                {
                    //3. Определение number по uid
                    str = await soap.SoapRequest("<ns1:getAccount><id>" + str2[0] + "</id></ns1:getAccount>");

                    if (str == "Не удалось обновить. Повторите попытку.")
                    {
                        errorLabel();
                    }
                    else
                    {
                        String[] type = soap.xmlDocument(str, "type");

                        String[] password = soap.xmlDocument(str, "pass");

                        if (password != null | type != null)
                        {
                            if (type[0] != "1")
                            {
                                if (pass.Text == password[0])
                                {
                                    Prefer p = new Prefer();

                                    p.set(numberPhone);

                                    App.Current.MainPage = new AppShell();

                                    lblFalsePhoneOrPass.IsVisible = false;
                                }
                                else
                                {
                                    errorLabel();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Юридическому лицу доступ запрещен", "", "Закрыть");
                                errorLabel();
                            }
                        }
                        else
                        {
                            errorLabel();
                        }
                    }
                }
            }
            GC.Collect();
        }

        private async Task email(String email)
        {
            String str;

            String[] str2;

            Soap soap = new Soap();

            str = await soap.SoapRequest("<ns1:getAccounts><flt><email>" + email + "</email></flt></ns1:getAccounts>");

            if (str == "Не удалось обновить. Повторите попытку.")
            {
                errorLabel();
            }
            else
            {
                str2 = soap.xmlDocument(str, "uid");

                if (str2 != null)
                {
                    str = await soap.SoapRequest("<ns1:getAccount><id>" + str2[0] + "</id></ns1:getAccount>");

                    if (str == "Не удалось обновить. Повторите попытку.")
                    {
                        errorLabel();
                    }
                    else
                    {
                        str2 = soap.xmlDocument(str, "pass");

                        string[] type = soap.xmlDocument(str, "type");

                        if (str2 != null | type != null)
                        {
                            if (type[0] != "1")
                            {
                                if (pass.Text == str2[0])
                                {
                                    str2 = soap.xmlDocument(str, "mobile");

                                    if (str2 != null)
                                    {
                                        if(str2[0] != null)
                                        {
                                            Prefer p = new Prefer();

                                            p.set(str2[0]);

                                            App.Current.MainPage = new AppShell();

                                            lblFalsePhoneOrPass.IsVisible = false;
                                        }
                                    }
                                    else
                                    {
                                        errorLabel();
                                    }
                                }
                                else
                                {
                                    errorLabel();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Юридическому лицу доступ запрещен", "", "Закрыть");
                                errorLabel();
                            }
                        }
                        else
                        {
                            errorLabel();
                        }
                    }
                }
                else
                {
                    errorLabel();
                }
            }
        }

        private async Task numberDogovor(String dogovor)
        {
            String str;

            String[] str2;

            Soap soap = new Soap();

            str = await soap.SoapRequest("<ns1:getAccounts><flt><agrmnum>" + dogovor + "</agrmnum></flt></ns1:getAccounts>");

            if (str == "Не удалось обновить. Повторите попытку.")
            {
                errorLabel();
            }
            else
            {
                str2 = soap.xmlDocument(str, "uid");

                if (str2 != null)
                {
                    str = await soap.SoapRequest("<ns1:getAccount><id>" + str2[0] + "</id></ns1:getAccount>");

                    if (str == "Не удалось обновить. Повторите попытку.")
                    {
                        errorLabel();
                    }
                    else
                    {
                        str2 = soap.xmlDocument(str, "pass");

                        string[] type = soap.xmlDocument(str, "type");

                        if (str2 != null)
                        {
                            if (type[0] != "1")
                            {
                                if (pass.Text == str2[0])
                                {
                                    str2 = soap.xmlDocument(str, "mobile");

                                    if (str2 != null)
                                    {
                                        if(str2[0] != "")
                                        {
                                            Prefer p = new Prefer();

                                            p.set(str2[0]);

                                            App.Current.MainPage = new AppShell();

                                            lblFalsePhoneOrPass.IsVisible = false;
                                        }
                                    }
                                    else
                                    {
                                        errorLabel();
                                    }
                                }
                                else
                                {
                                    errorLabel();
                                }
                            }
                            else
                            {
                                await DisplayAlert("Юридическому лицу доступ запрещен", "", "Закрыть");
                                errorLabel();
                            }
                        }
                        else
                        {
                            errorLabel();
                        }
                    }
                }
                else
                {
                    errorLabel();
                }
            }
        }

        private void errorLabel()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                login.Text = "";

                pass.Text = "";

                lblFalsePhoneOrPass.Text = "Неверно введен логин или пароль";

                lblFalsePhoneOrPass.IsVisible = true;
            }
            else
            {
                lblFalsePhoneOrPass.Text = "Отсутствует подключение к интернету";
            }
        }

        private async void btnCall_Clicked(object sender, EventArgs e)
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

        private void btnSms_Clicked(object sender, EventArgs e)
        {
            Browser.OpenAsync("https://wa.me/79289195550");
        }
    }
}