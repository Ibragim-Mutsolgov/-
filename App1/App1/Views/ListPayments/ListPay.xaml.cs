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
    public partial class ListPay : ContentPage
    {
        String agrmid;

        public ListPay(String agrmid)
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, false);

            if (Device.RuntimePlatform == "iOS")
            {
                btnBack.Source = ImageSource.FromResource("App1.Views.Images.Back.png");
            }
            else
            {
                btnBack.Source = "Back.png";
            }

            this.agrmid = agrmid;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                meth();
            }
            else
            {
                notInformation();
            }

            GC.Collect();
        }
        private async Task meth()
        {
            await listP();
        }
        private async Task listP()
        {
            Soap soap = new Soap();

            String str;

            str = await soap.SoapRequest("<ns1:getPayments><flt><payhistory>0</payhistory><agrmid>" + agrmid + "</agrmid><dtfrom>" + dateFrom.Date.Day + "." + dateFrom.Date.Month + "." + dateFrom.Date.Year + "</dtfrom><dtto>" + dateto.Date.Day + "." + dateto.Date.Month + "." + dateto.Date.Year + "</dtto></flt></ns1:getPayments>");

            if (str != "Не удалось обновить. Повторите попытку.")
            {
                String[] ret = soap.xmlDocument(str, "ret");

                String[] paydate = soap.xmlDocument(str, "paydate");

                String[] classname = soap.xmlDocument(str, "classname");

                String[] amount = soap.xmlDocument(str, "amount");

                String[] currsymb = soap.xmlDocument(str, "currsymb");

                String[] lbapi = soap.xmlDocument(str, "lbapi:getPaymentsResponse");

                if (paydate == null | classname == null | amount == null | currsymb == null)
                {
                    notInformation();
                }
                else
                {
                    Label labelData = new Label
                    {
                        Text = "Дата",

                        TextColor = Color.White,

                        FontFamily = "akzi",

                        HorizontalOptions = LayoutOptions.StartAndExpand,

                        VerticalOptions = LayoutOptions.CenterAndExpand,

                        WidthRequest = 100,

                        HorizontalTextAlignment = TextAlignment.Center,
                    };

                    StackLayout stack11 = new StackLayout();

                    stack11.Orientation = StackOrientation.Horizontal;

                    stack11.HorizontalOptions = LayoutOptions.StartAndExpand;

                    stack11.WidthRequest = 100;

                    stack11.Children.Add(labelData);

                    Label labelSumm = new Label
                    {
                        Text = "Сумма",

                        TextColor = Color.White,

                        FontFamily = "akzi",

                        HorizontalOptions = LayoutOptions.CenterAndExpand,

                        VerticalOptions = LayoutOptions.CenterAndExpand,

                        WidthRequest = 100,

                        HorizontalTextAlignment = TextAlignment.Center,
                    };

                    StackLayout stack12 = new StackLayout();

                    stack12.Orientation = StackOrientation.Horizontal;

                    stack12.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    stack12.WidthRequest = 100;

                    stack12.Children.Add(labelSumm);

                    Label labelSystem = new Label
                    {
                        Text = "Система",

                        TextColor = Color.White,

                        FontFamily = "akzi",

                        HorizontalOptions = LayoutOptions.CenterAndExpand,

                        VerticalOptions = LayoutOptions.CenterAndExpand,

                        WidthRequest = 100,

                        HorizontalTextAlignment = TextAlignment.Center,
                    };
                    StackLayout stack13 = new StackLayout();

                    stack13.Orientation = StackOrientation.Horizontal;

                    stack13.HorizontalOptions = LayoutOptions.EndAndExpand;

                    stack13.WidthRequest = 100;

                    stack13.Children.Add(labelSystem);

                    StackLayout stack1 = new StackLayout();

                    stack1.Padding = new Thickness(20, 0, 20, 0);

                    stack1.Orientation = StackOrientation.Horizontal;

                    stack1.HorizontalOptions = LayoutOptions.FillAndExpand;

                    stack1.Children.Add(stack11);
                    stack1.Children.Add(stack12);
                    stack1.Children.Add(stack13);

                    stack.Children.Add(stack1);

                    for (int i = 0; i < ret.Length; i++)
                    {
                        BoxView box = new BoxView
                        {
                            HeightRequest = 1,
                            BackgroundColor = Color.White
                        };
                        stack.Children.Add(box);
                        StackLayout stack21 = new StackLayout();

                        stack21.Orientation = StackOrientation.Horizontal;

                        stack21.HorizontalOptions = LayoutOptions.StartAndExpand;

                        stack21.WidthRequest = 100;

                        String[] date = paydate[i].Split(' ');

                        Label label1 = new Label
                        {
                            Text = date[0],

                            FontSize = 13,

                            FontFamily = "akzi",

                            TextColor = Color.White,

                            HorizontalOptions = LayoutOptions.StartAndExpand,

                            VerticalOptions = LayoutOptions.CenterAndExpand,

                            HorizontalTextAlignment = TextAlignment.Center,

                            WidthRequest = 100
                        };

                        stack21.Children.Add(label1);

                        StackLayout stack22 = new StackLayout();

                        stack22.Orientation = StackOrientation.Horizontal;

                        stack22.HorizontalOptions = LayoutOptions.CenterAndExpand;

                        stack22.WidthRequest = 100;

                        Label label2 = new Label
                        {
                            Text = amount[i].Split('.')[0] + "Р",

                            FontSize = 13,

                            FontFamily = "akzi",

                            TextColor = Color.White,

                            HorizontalOptions = LayoutOptions.StartAndExpand,

                            VerticalOptions = LayoutOptions.CenterAndExpand,

                            HorizontalTextAlignment = TextAlignment.Center,

                            WidthRequest = 100
                        };

                        stack22.Children.Add(label2);

                        stack.Children.Add(stack22);

                        StackLayout stack23 = new StackLayout();

                        stack23.Orientation = StackOrientation.Horizontal;

                        stack23.HorizontalOptions = LayoutOptions.EndAndExpand;

                        stack23.WidthRequest = 100;

                        if (classname[i] == "Сбербанк-онлайн")
                        {
                            classname[i] = "Сбер Онлайн";
                        }
                        Label label3 = new Label
                        {
                            Text = classname[i],

                            FontSize = 13,

                            FontFamily = "akzi",

                            TextColor = Color.White,

                            HorizontalOptions = LayoutOptions.CenterAndExpand,

                            VerticalOptions = LayoutOptions.CenterAndExpand,

                            HorizontalTextAlignment = TextAlignment.Center,

                            WidthRequest = 100
                        };

                        stack23.Children.Add(label3);

                        StackLayout stack2 = new StackLayout();

                        stack2.Padding = new Thickness(20, 0, 20, 0);

                        stack2.Orientation = StackOrientation.Horizontal;

                        stack2.HorizontalOptions = LayoutOptions.FillAndExpand;

                        stack2.Children.Add(stack21);
                        stack2.Children.Add(stack22);
                        stack2.Children.Add(stack23);

                        stack.Children.Add(stack2);
                    }
                }
            }
            else
            {
                notInformation();
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
                notInformation();
            }

            GC.Collect();

            Refresh.IsRefreshing = false;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            stack.Children.Clear();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                meth();
            }
            else
            {
                notInformation();
            }

            GC.Collect();
        }
        private void notInformation()
        {
            Label label = new Label
            {
                Text = "Отсутствует информация о платежах.",

                TextColor = Color.White,

                FontFamily = "akzi",

                FontSize = 15,

                HorizontalOptions = LayoutOptions.CenterAndExpand,

                HorizontalTextAlignment = TextAlignment.Center
            };
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                label.Text = "Отсутствует подключение к интернету.";
            }
            stack.Children.Add(label);
        }

        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}