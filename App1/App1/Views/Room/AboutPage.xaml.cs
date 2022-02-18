
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    public partial class AboutPage : ContentPage
    {
        string phone;
        public AboutPage()
        {
            InitializeComponent();

            Prefer pr = new Prefer();

            phone = pr.getName();

            Shell.SetNavBarIsVisible(this, false);

            if(Device.RuntimePlatform == "iOS")
            {
                bar.Source = ImageSource.FromResource("App1.Views.Images.pl.png");
                burger.Source = ImageSource.FromResource("App1.Views.Images.Burger.png");
                tool.Source = ImageSource.FromResource("App1.Views.Images.Logout.png");
            }

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                meth();
            }
            else
            {
                Soap soap = new Soap();
                soap.Label(stackRoom);
            }
        }

        private async Task meth()
        {
            await TarifAsync();
        }
        private async Task TarifAsync()
        {
            Soap soap = new Soap();

            String str;

            String[] str2;

            //2. Определение данных по номеру
            str = await soap.SoapRequest("<ns1:getAccounts><flt><phone>" + phone + "</phone></flt></ns1:getAccounts>");

            if (str == "Не удалось обновить. Повторите попытку.")
            {
                soap.LabelForm(stackRoom);
            }
            else
            {
                string[] name = soap.xmlDocument(str, "name");

                if (name != null)
                {
                    Label lblNam = new Label
                    {
                        Text = name[0],
                        FontFamily = "akzi",
                        FontSize = 20,
                        TextColor = Color.Black,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                    };
                    stackRoom.Children.Add(lblNam);
                }

                str2 = soap.xmlDocument(str, "uid");

                if (str2 == null)
                {
                    soap.LabelForm(stackRoom);
                }
                else
                {
                    //3. Определение number по uid
                    str = await soap.SoapRequest("<ns1:getAgreements><flt><userid>" + str2[0] + "</userid></flt></ns1:getAgreements>");

                    if (str == "Не удалось обновить. Повторите попытку.")
                    {
                        soap.LabelForm(stackRoom);
                    }
                    else
                    {
                        string[] agrmid = soap.xmlDocument(str, "agrmid");
                        if (agrmid == null)
                        {
                            soap.LabelForm(stackRoom);
                        }
                        else
                        {
                            for (int i = 0; i < agrmid.Length; i++)
                            {
                                str = await soap.SoapRequest("<ns1:getVgroups><flt><agrmid>" + agrmid[i] + "</agrmid><archive>0</archive><agentid>1</agentid></flt></ns1:getVgroups>");

                                if (str == null)
                                {
                                    soap.LabelForm(stackRoom);
                                }
                                else
                                {
                                    string[] ret = soap.xmlDocument(str, "ret");

                                    String[] number = soap.xmlDocument(str, "agrmnum");

                                    String[] balance = soap.xmlDocument(str, "balance");

                                    String[] tarifdescr = soap.xmlDocument(str, "tarifdescr");

                                    if (ret != null & number != null & balance != null & tarifdescr != null)
                                    {
                                        if(int.Parse(balance[0].Split('.')[0]) < 50)
                                        {
                                            DependencyService.Get<Interface1>().set2(balance[0]);
                                        }
                                        if(ret.Length != 1)
                                        {
                                            Boolean b = false;

                                            String oneNumber = number[0];

                                            String oneBalance = balance[0];

                                            String oneTarifdescr = tarifdescr[0];

                                            for (int i2 = 0; i2 < ret.Length; i2++)
                                            {
                                                if (i2 != 0 & oneNumber != number[i2])
                                                {
                                                    b = true;
                                                }
                                                if (i2 != 0 & oneBalance != balance[i2])
                                                {
                                                    b = true;
                                                }
                                                if (i != 0 & oneTarifdescr != tarifdescr[i])
                                                {
                                                    b = true;
                                                }
                                            }

                                            if (b == true)
                                            {
                                                for (int j = 0; j < ret.Length; j++)
                                                {
                                                    BoxView box2 = new BoxView
                                                    {
                                                        HeightRequest = 2,
                                                        BackgroundColor = Color.Black
                                                    };
                                                    stackRoom.Children.Add(box2);

                                                    Label numName = new Label
                                                    {
                                                        Text = "Договор:",
                                                        FontFamily = "akzi",
                                                        FontSize = 15,
                                                        TextColor = Color.Black,
                                                        HorizontalTextAlignment = TextAlignment.Center
                                                    };
                                                    Label num = new Label
                                                    {
                                                        Text = number[j],
                                                        FontFamily = "akzi",
                                                        FontSize = 30,
                                                        TextColor = Color.Black,
                                                        HorizontalTextAlignment = TextAlignment.Center
                                                    };

                                                    Label tarifName = new Label
                                                    {
                                                        Text = "Тариф:",
                                                        FontFamily = "akzi",
                                                        FontSize = 15,
                                                        TextColor = Color.Black,
                                                        HorizontalTextAlignment = TextAlignment.Center
                                                    };
                                                    Label tarif = new Label
                                                    {
                                                        Text = tarifdescr[j].Split(' ')[0],
                                                        FontFamily = "akzi",
                                                        FontSize = 30,
                                                        TextColor = Color.Black,
                                                        HorizontalTextAlignment = TextAlignment.Center
                                                    };

                                                    BoxView box = new BoxView
                                                    {
                                                        HeightRequest = 2,
                                                        BackgroundColor = Color.Black
                                                    };

                                                    Label balName = new Label
                                                    {
                                                        Text = "Баланс:",
                                                        FontFamily = "akzi",
                                                        FontSize = 15,
                                                        TextColor = Color.Black,
                                                        HorizontalTextAlignment = TextAlignment.Center
                                                    };
                                                    Label bal = new Label
                                                    {
                                                        Text = balance[j].Split('.')[0],
                                                        FontFamily = "akzi",
                                                        FontSize = 40,
                                                        TextColor = Color.Black,
                                                        HorizontalTextAlignment = TextAlignment.Center
                                                    };
                                                    Image im = new Image
                                                    {
                                                        HeightRequest = 25
                                                    };
                                                    if (Device.RuntimePlatform == "iOS")
                                                    {
                                                        im.Source = ImageSource.FromResource("App1.Views.Images.rubl.png");
                                                    }
                                                    else
                                                    {
                                                        im.Source = ImageSource.FromResource("App1.Views.Images.rubl.png");
                                                    }


                                                    StackLayout stackDogovor = new StackLayout
                                                    {
                                                        Orientation = StackOrientation.Horizontal
                                                    };

                                                    StackLayout stackBalance = new StackLayout
                                                    {
                                                        HorizontalOptions = LayoutOptions.StartAndExpand
                                                    };

                                                    //Для договора
                                                    StackLayout stack1 = new StackLayout
                                                    {
                                                        HorizontalOptions = LayoutOptions.StartAndExpand
                                                    };
                                                    stack1.Children.Add(numName);
                                                    StackLayout stackNum = new StackLayout
                                                    {
                                                        HorizontalOptions = LayoutOptions.StartAndExpand
                                                    };
                                                    stackNum.Children.Add(num);

                                                    //Для тарифа
                                                    StackLayout stack2 = new StackLayout
                                                    {
                                                        HorizontalOptions = LayoutOptions.EndAndExpand
                                                    };
                                                    stack2.Children.Add(tarifName);
                                                    StackLayout stackTar = new StackLayout
                                                    {
                                                        HorizontalOptions = LayoutOptions.EndAndExpand
                                                    };
                                                    stackTar.Children.Add(tarif);

                                                    StackLayout stackbal = new StackLayout
                                                    {
                                                        Orientation = StackOrientation.Horizontal,
                                                        HorizontalOptions = LayoutOptions.StartAndExpand
                                                    };

                                                    stackDogovor.Children.Add(stack1);
                                                    stackDogovor.Children.Add(stack2);

                                                    StackLayout stackEnd = new StackLayout
                                                    {
                                                        Orientation = StackOrientation.Horizontal
                                                    };
                                                    stackEnd.Children.Add(stackNum);
                                                    stackEnd.Children.Add(stackTar);

                                                    stackBalance.Children.Add(balName);
                                                    stackbal.Children.Add(bal);
                                                    stackbal.Children.Add(im);
                                                    stackBalance.Children.Add(stackbal);

                                                    stackRoom.Children.Add(stackDogovor);//Договор
                                                    stackRoom.Children.Add(stackEnd);
                                                    stackRoom.Children.Add(box);//Линия
                                                    stackRoom.Children.Add(stackBalance);//Тариф
                                                }
                                            }
                                            else
                                            {

                                                BoxView box2 = new BoxView
                                                {
                                                    HeightRequest = 2,
                                                    BackgroundColor = Color.Black
                                                };
                                                stackRoom.Children.Add(box2);

                                                Label numName = new Label
                                                {
                                                    Text = "Договор:",
                                                    FontFamily = "akzi",
                                                    FontSize = 15,
                                                    TextColor = Color.Black,
                                                    HorizontalTextAlignment = TextAlignment.Center
                                                };
                                                Label num = new Label
                                                {
                                                    Text = number[0],
                                                    FontFamily = "akzi",
                                                    FontSize = 20,
                                                    TextColor = Color.Black,
                                                    HorizontalTextAlignment = TextAlignment.Center
                                                };
                                                Label tarifName = new Label
                                                {
                                                    Text = "Тариф:",
                                                    FontFamily = "akzi",
                                                    FontSize = 15,
                                                    TextColor = Color.Black,
                                                    HorizontalTextAlignment = TextAlignment.Center
                                                };
                                                Label tarif = new Label
                                                {
                                                    Text = tarifdescr[0].Split(' ')[0],
                                                    FontFamily = "akzi",
                                                    FontSize = 20,
                                                    TextColor = Color.Black,
                                                    HorizontalTextAlignment = TextAlignment.Center
                                                };

                                                BoxView box = new BoxView
                                                {
                                                    HeightRequest = 2,
                                                    BackgroundColor = Color.Black
                                                };

                                                Label balName = new Label
                                                {
                                                    Text = "Баланс:",
                                                    FontFamily = "akzi",
                                                    FontSize = 15,
                                                    TextColor = Color.Black,
                                                    HorizontalTextAlignment = TextAlignment.Center
                                                };
                                                Label bal = new Label
                                                {
                                                    Text = balance[0].Split('.')[0],
                                                    FontFamily = "akzi",
                                                    FontSize = 30,
                                                    TextColor = Color.Black,
                                                    HorizontalTextAlignment = TextAlignment.Center,
                                                };
                                                Image im = new Image
                                                {
                                                    HeightRequest = 25
                                                };
                                                if (Device.RuntimePlatform == "iOS")
                                                {
                                                    im.Source = ImageSource.FromResource("App1.Views.Images.rubl.png");
                                                }
                                                else
                                                {
                                                    im.Source = ImageSource.FromResource("App1.Views.Images.rubl.png");
                                                }

                                                StackLayout stackDogovor = new StackLayout
                                                {
                                                    Orientation = StackOrientation.Horizontal,
                                                    HorizontalOptions = LayoutOptions.FillAndExpand
                                                };

                                                StackLayout stackBalance = new StackLayout
                                                {
                                                    HorizontalOptions = LayoutOptions.StartAndExpand
                                                };

                                                //Для договора
                                                StackLayout stack1 = new StackLayout
                                                {
                                                    HorizontalOptions = LayoutOptions.StartAndExpand
                                                };
                                                stack1.Children.Add(numName);
                                                StackLayout stackNum = new StackLayout
                                                {
                                                    HorizontalOptions = LayoutOptions.StartAndExpand
                                                };
                                                stackNum.Children.Add(num);

                                                //Для тарифа
                                                StackLayout stack2 = new StackLayout
                                                {
                                                    HorizontalOptions = LayoutOptions.EndAndExpand
                                                };
                                                stack2.Children.Add(tarifName);
                                                StackLayout stackTar = new StackLayout
                                                {
                                                    HorizontalOptions = LayoutOptions.EndAndExpand
                                                };
                                                stackTar.Children.Add(tarif);

                                                StackLayout stackbal = new StackLayout
                                                {
                                                    Orientation = StackOrientation.Horizontal,
                                                    HorizontalOptions = LayoutOptions.StartAndExpand
                                                };

                                                stackDogovor.Children.Add(stack1);
                                                stackDogovor.Children.Add(stack2);

                                                StackLayout stackEnd = new StackLayout
                                                {
                                                    Orientation = StackOrientation.Horizontal
                                                };
                                                stackEnd.Children.Add(stackNum);
                                                stackEnd.Children.Add(stackTar);

                                                stackBalance.Children.Add(balName);
                                                stackbal.Children.Add(bal);
                                                stackbal.Children.Add(im);
                                                stackBalance.Children.Add(stackbal);

                                                stackRoom.Children.Add(stackDogovor);//Договор
                                                stackRoom.Children.Add(stackEnd);
                                                stackRoom.Children.Add(box);//Линия
                                                stackRoom.Children.Add(stackBalance);//Тариф
                                            }
                                        }
                                        else
                                        {
                                            BoxView box2 = new BoxView
                                            {
                                                HeightRequest = 2,
                                                BackgroundColor = Color.Black
                                            };
                                            stackRoom.Children.Add(box2);

                                            Label numName = new Label
                                            {
                                                Text = "Договор:",
                                                FontFamily = "akzi",
                                                FontSize = 15,
                                                TextColor = Color.Black,
                                                HorizontalTextAlignment = TextAlignment.Center
                                            };
                                            Label num = new Label
                                            {
                                                Text = number[0],
                                                FontFamily = "akzi",
                                                FontSize = 20,
                                                TextColor = Color.Black,
                                                HorizontalTextAlignment = TextAlignment.Center
                                            };
                                            Label tarifName = new Label
                                            {
                                                Text = "Тариф:",
                                                FontFamily = "akzi",
                                                FontSize = 15,
                                                TextColor = Color.Black,
                                                HorizontalTextAlignment = TextAlignment.Center
                                            };
                                            Label tarif = new Label
                                            {
                                                Text = tarifdescr[0].Split(' ')[0],
                                                FontFamily = "akzi",
                                                FontSize = 20,
                                                TextColor = Color.Black,
                                                HorizontalTextAlignment = TextAlignment.Center
                                            };

                                            BoxView box = new BoxView
                                            {
                                                HeightRequest = 2,
                                                BackgroundColor = Color.Black
                                            };

                                            Label balName = new Label
                                            {
                                                Text = "Баланс:",
                                                FontFamily = "akzi",
                                                FontSize = 15,
                                                TextColor = Color.Black,
                                                HorizontalTextAlignment = TextAlignment.Start
                                            };
                                            Label bal = new Label
                                            {
                                                Text = balance[0].Split('.')[0],
                                                FontFamily = "akzi",
                                                FontSize = 30,
                                                TextColor = Color.Black,
                                                HorizontalTextAlignment = TextAlignment.Center,
                                            };
                                            Image im = new Image
                                            {
                                                HeightRequest = 25
                                            };
                                            if (Device.RuntimePlatform == "iOS")
                                            {
                                                im.Source = ImageSource.FromResource("App1.Views.Images.rubl.png");
                                            }
                                            else
                                            {
                                                im.Source = ImageSource.FromResource("App1.Views.Images.rubl.png");
                                            }

                                            StackLayout stackDogovor = new StackLayout
                                            {
                                                Orientation = StackOrientation.Horizontal,
                                                HorizontalOptions = LayoutOptions.FillAndExpand
                                            };

                                            StackLayout stackBalance = new StackLayout
                                            {
                                                HorizontalOptions = LayoutOptions.StartAndExpand
                                            };

                                            //Для договора
                                            StackLayout stack1 = new StackLayout
                                            {
                                                HorizontalOptions = LayoutOptions.StartAndExpand
                                            };
                                            stack1.Children.Add(numName);
                                            StackLayout stackNum = new StackLayout
                                            {
                                                HorizontalOptions = LayoutOptions.StartAndExpand
                                            };
                                            stackNum.Children.Add(num);

                                            //Для тарифа
                                            StackLayout stack2 = new StackLayout
                                            {
                                                HorizontalOptions = LayoutOptions.EndAndExpand
                                            };
                                            stack2.Children.Add(tarifName);
                                            StackLayout stackTar = new StackLayout
                                            {
                                                HorizontalOptions = LayoutOptions.EndAndExpand
                                            };
                                            stackTar.Children.Add(tarif);

                                            StackLayout stackbal = new StackLayout
                                            {
                                                Orientation = StackOrientation.Horizontal,
                                                HorizontalOptions = LayoutOptions.StartAndExpand
                                            };

                                            stackDogovor.Children.Add(stack1);
                                            stackDogovor.Children.Add(stack2);

                                            StackLayout stackEnd = new StackLayout
                                            {
                                                Orientation = StackOrientation.Horizontal
                                            };
                                            stackEnd.Children.Add(stackNum);
                                            stackEnd.Children.Add(stackTar);

                                            stackBalance.Children.Add(balName);
                                            stackbal.Children.Add(bal);
                                            stackbal.Children.Add(im);
                                            stackBalance.Children.Add(stackbal);

                                            stackRoom.Children.Add(stackDogovor);//Договор
                                            stackRoom.Children.Add(stackEnd);
                                            stackRoom.Children.Add(box);//Линия
                                            stackRoom.Children.Add(stackBalance);//Тариф
                                            
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private async void buttonPromisePayments(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new PromisePayments(phone));
        }
        private async void buttonPayNumbers(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new PayNumbers(phone));
        }
        private async void buttonTP(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new ListPayments(phone));
        }
        private async void buttonSposob(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new Sposob());
        }
        private async void Refresh_Refreshing(object sender, EventArgs e)
        {
            stackRoom.Children.Clear();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                meth();
            }
            else
            {
                Soap soap = new Soap();
                soap.Label(stackRoom);
            }

            GC.Collect();

            Refresh.IsRefreshing = false;
        }
        private async void tool_Clicked(object sender, EventArgs e)
        {
            Prefer p = new Prefer();

            p.clear();

            DependencyService.Get<Interface1>().delete();

            MessagingCenter.Send<Page>(this, "unsub");

            Navigation.RemovePage(this);

            await Shell.Current.GoToAsync("//LoginPage");

            GC.Collect();
        }

        [Obsolete]
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

        private void burger_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<Page>(this, "open");
        }

        private void btnSms_Clicked(object sender, EventArgs e)
        {
            Browser.OpenAsync("https://wa.me/79289195550");
        }
    }
}