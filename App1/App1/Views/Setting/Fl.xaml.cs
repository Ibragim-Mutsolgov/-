using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views.Setting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Fl : ContentPage
    {
        String phone;
        public Fl()
        {
            InitializeComponent();

            Prefer pr = new Prefer();

            phone = pr.getName();

            if (Device.RuntimePlatform == "iOS")
            {
                btnBack.Source = ImageSource.FromResource("App1.Views.Images.Burger.png");
            }
            else
            {
                btnBack.Source = "Burger.png";
            }

            Shell.SetNavBarIsVisible(this, false);

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                meth();
            }
            else
            {
                Soap soap = new Soap();
                soap.Label(stackSettings);
            }

            GC.Collect();
        }
        private async Task meth()
        {
            await setSettings();
        }
        private async Task setSettings()
        {
            Soap soap = new Soap();

            String str;

            //2. Определение данных по номеру
            str = await soap.SoapRequest("<ns1:getAccounts><flt><phone>" + phone + "</phone></flt></ns1:getAccounts>");

            if (str == "Не удалось обновить. Повторите попытку.")
            {
                soap.LabelForm(stackSettings);
            }
            else
            {
                //Определение uid
                String[] name = soap.xmlDocument(str, "name");//0

                String[] mobile = soap.xmlDocument(str, "mobile");//0

                String[] uid = soap.xmlDocument(str, "uid");//0

                String[] login = soap.xmlDocument(str, "login");//0

                if (name == null | mobile == null | uid == null | login == null)
                {
                    soap.LabelForm(stackSettings);
                }
                else
                {
                    Label lblName = new Label
                    {
                        Text = "Имя пользователя:",

                        FontFamily = "akzi",

                        FontSize = 15,

                        HorizontalOptions = LayoutOptions.Start,

                        HorizontalTextAlignment = TextAlignment.Start
                    };
                    stackSettings.Children.Add(lblName);

                    Label lblName2 = new Label
                    {
                        Text = name[0],

                        FontFamily = "akzi",

                        FontSize = 20,

                        TextColor = Color.Black,

                        HorizontalOptions = LayoutOptions.Center,

                        LineBreakMode = LineBreakMode.WordWrap,

                        HorizontalTextAlignment = TextAlignment.Center
                    };
                    stackSettings.Children.Add(lblName2);


                    BoxView box = new BoxView
                    {
                        HeightRequest = 3,

                        BackgroundColor = Color.Black
                    };
                    stackSettings.Children.Add(box);

                    Label lblMobile = new Label
                    {
                        Text = "Мобильный телефон:",

                        FontFamily = "akzi",

                        FontSize = 15,

                        HorizontalOptions = LayoutOptions.Start,

                        HorizontalTextAlignment = TextAlignment.Start
                    };
                    stackSettings.Children.Add(lblMobile);

                    if (mobile[0].Length == 10)
                    {
                        mobile[0] = "7" + mobile[0];
                    }

                    if (mobile[0].StartsWith("7"))
                    {
                        mobile[0] = "+" + mobile[0];
                    }

                    Label lblMobile2 = new Label
                    {
                        Text = mobile[0],

                        FontFamily = "akzi",

                        FontSize = 20,

                        TextColor = Color.Black,

                        HorizontalOptions = LayoutOptions.Center,

                        HorizontalTextAlignment = TextAlignment.Center
                    };
                    stackSettings.Children.Add(lblMobile2);

                    BoxView box2 = new BoxView
                    {
                        HeightRequest = 3,

                        BackgroundColor = Color.Black
                    };
                    stackSettings.Children.Add(box2);

                    //3. Определение number по uid
                    str = await soap.SoapRequest("<ns1:getAgreements><flt><userid>" + uid[0] + "</userid></flt></ns1:getAgreements>");
                    if (str == "Не удалось обновить. Повторите попытку.")
                    {
                        soap.LabelForm(stackSettings);
                    }
                    else
                    {
                        String[] agrmid = soap.xmlDocument(str, "agrmid");

                        String[] num = soap.xmlDocument(str, "number");

                        if (agrmid == null | num == null)
                        {
                            soap.LabelForm(stackSettings);
                        }
                        else
                        {
                            if (num.Length == 1)
                            {
                                Label numbers = new Label
                                {
                                    Text = "Номер договора:",

                                    FontFamily = "akzi",

                                    FontSize = 15,

                                    HorizontalOptions = LayoutOptions.Start,

                                    HorizontalTextAlignment = TextAlignment.Start
                                };
                                stackSettings.Children.Add(numbers);
                            }
                            else if (num.Length > 1)
                            {
                                Label numbers = new Label
                                {
                                    Text = "Номера договоров:",

                                    FontFamily = "akzi",

                                    FontSize = 15,

                                    HorizontalOptions = LayoutOptions.Start,

                                    HorizontalTextAlignment = TextAlignment.Start
                                };
                                stackSettings.Children.Add(numbers);
                            }

                            for (int i = 0; i < num.Length; i++)
                            {
                                Label number = new Label
                                {
                                    Text = num[i],

                                    FontFamily = "akzi",

                                    FontSize = 20,

                                    TextColor = Color.Black,

                                    HorizontalOptions = LayoutOptions.Center,

                                    HorizontalTextAlignment = TextAlignment.Center
                                };
                                stackSettings.Children.Add(number);

                                if (i == num.Length - 1)
                                {
                                    BoxView box3 = new BoxView
                                    {
                                        HeightRequest = 3,

                                        BackgroundColor = Color.Black
                                    };
                                    stackSettings.Children.Add(box3);
                                }
                            }

                            Label label = new Label
                            {
                                Text = "Логин:",

                                FontFamily = "akzi",

                                FontSize = 15,

                                HorizontalOptions = LayoutOptions.Start,

                                HorizontalTextAlignment = TextAlignment.Start
                            };
                            stackSettings.Children.Add(label);

                            Label lblLogin2 = new Label
                            {
                                Text = login[0],

                                FontFamily = "akzi",

                                FontSize = 20,

                                TextColor = Color.Black,

                                HorizontalOptions = LayoutOptions.Center,

                                HorizontalTextAlignment = TextAlignment.Center
                            };
                            stackSettings.Children.Add(lblLogin2);
                        }
                    }
                }
                str = await soap.SoapRequest("<ns1:getAccount><id>" + uid[0] + "</id></ns1:getAccount>");

                if (str != "Не удалось обновить. Повторите попытку.")
                {

                    String[] str2 = soap.xmlDocument(str, "address");
                    String[] birth = soap.xmlDocument(str, "birthdate");

                    if (str2 != null)
                    {
                        BoxView box4 = new BoxView
                        {
                            HeightRequest = 3,

                            BackgroundColor = Color.Black
                        };
                        stackSettings.Children.Add(box4);

                        Label br = new Label
                        {
                            Text = "Дата рождения:",

                            FontFamily = "akzi",

                            FontSize = 15,

                            HorizontalOptions = LayoutOptions.Start,

                            HorizontalTextAlignment = TextAlignment.Start
                        };
                        stackSettings.Children.Add(br);
                        Label br1 = new Label
                        {
                            Text = birth[0],

                            FontFamily = "akzi",

                            FontSize = 20,

                            TextColor = Color.Black,

                            HorizontalOptions = LayoutOptions.Center,

                            HorizontalTextAlignment = TextAlignment.Center
                        };
                        stackSettings.Children.Add(br1);

                        String[] region = str2[1].Split(',');

                        //Регион - region[1]
                        BoxView box3 = new BoxView
                        {
                            HeightRequest = 3,

                            BackgroundColor = Color.Black
                        };
                        stackSettings.Children.Add(box3);

                        Label label = new Label
                        {
                            Text = "Регион:",

                            FontFamily = "akzi",

                            FontSize = 15,

                            HorizontalOptions = LayoutOptions.Start,

                            HorizontalTextAlignment = TextAlignment.Start
                        };
                        stackSettings.Children.Add(label);
                        Label label1 = new Label
                        {
                            Text = region[1],

                            FontFamily = "akzi",

                            FontSize = 20,

                            TextColor = Color.Black,

                            HorizontalOptions = LayoutOptions.Center,

                            HorizontalTextAlignment = TextAlignment.Center
                        };
                        stackSettings.Children.Add(label1);

                        BoxView box2 = new BoxView
                        {
                            HeightRequest = 3,

                            BackgroundColor = Color.Black
                        };
                        stackSettings.Children.Add(box2);

                        Label adr1 = new Label
                        {
                            FontSize = 20,

                            FontFamily = "akzi",

                            TextColor = Color.Black,

                            HorizontalOptions = LayoutOptions.Center,

                            LineBreakMode = LineBreakMode.WordWrap,

                            HorizontalTextAlignment = TextAlignment.Center
                        };
                        for (int i = 2; i < region.Length; i++)
                        {
                            if (region[i] != region[region.Length - 1])
                            {
                                if (region[i] != "" & region[i] != null)
                                {
                                    if (i != 2 & adr1.Text != null)
                                    {
                                        adr1.Text += ", ";
                                    }
                                    adr1.Text += region[i];
                                }
                            }
                        }

                        Label adr = new Label
                        {
                            Text = "Адрес проживания:",

                            FontFamily = "akzi",

                            FontSize = 15,

                            HorizontalOptions = LayoutOptions.Start,

                            HorizontalTextAlignment = TextAlignment.Start
                        };
                        stackSettings.Children.Add(adr);

                        stackSettings.Children.Add(adr1);

                        BoxView box1 = new BoxView
                        {
                            HeightRequest = 3,

                            BackgroundColor = Color.Black
                        };
                        stackSettings.Children.Add(box1);

                        Label code = new Label
                        {
                            Text = "Индекс:",

                            FontFamily = "akzi",

                            FontSize = 15,

                            HorizontalOptions = LayoutOptions.Start,

                            HorizontalTextAlignment = TextAlignment.Start
                        };
                        stackSettings.Children.Add(code);
                        Label code1 = new Label
                        {
                            Text = region[region.Length - 1],

                            FontFamily = "akzi",

                            FontSize = 20,

                            TextColor = Color.Black,

                            HorizontalOptions = LayoutOptions.Center,

                            HorizontalTextAlignment = TextAlignment.Center
                        };
                        stackSettings.Children.Add(code1);
                    }
                }
            }

        }
        private async void Refresh_Refreshing(object sender, EventArgs e)
        {
            stackSettings.Children.Clear();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                meth();
            }
            else
            {
                Soap soap = new Soap();
                soap.Label(stackSettings);
            }

            GC.Collect();

            Refresh.IsRefreshing = false;
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<Page>(this, "open");
        }
    }
}