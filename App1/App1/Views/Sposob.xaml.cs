using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Sposob : ContentPage
	{
		public Sposob ()
		{
			InitializeComponent ();

			Shell.SetNavBarIsVisible(this, false);

            if (Device.RuntimePlatform == "iOS")
            {
                btnBack.Source = ImageSource.FromResource("App1.Views.Images.Back.png");
                heightRow.Height = 280;
            }
            else
            {
                btnBack.Source = "Back.png";
            }

            nal.BackgroundColor = Color.Black;

            nal.TextColor = Color.FromHex("#baf54b");

            bez.BackgroundColor = Color.FromHex("#baf54b");

            bez.TextColor = Color.Black;
		}
        private async void btnBack_Clicked(object sender, EventArgs e)
        {
			await Navigation.PopAsync();
		}
        //наличными
        private void Button_Clicked(object sender, EventArgs e)
        {
            nal.BackgroundColor = Color.Black;

            nal.TextColor = Color.FromHex("#baf54b");

            bez.BackgroundColor = Color.FromHex("#baf54b");

            bez.TextColor = Color.Black;

            stack.Children.Clear();

            Label label1 = new Label
            {
                Text = "Оплата наличными в офисе.",
                FontFamily = "akzi",
                FontSize = 17,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center
            };
            Label label2 = new Label
            {
                Text = "  Для пополнения баланса Вам необходимо прийти в наш офис.",
                FontFamily = "akzi",
                FontSize = 17,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            Label label3 = new Label
            {
                Text = "  Мы работаем 7 дней в неделю с 9:00 до 19:00.",
                FontFamily = "akzi",
                FontSize = 17,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            stack.Children.Add(label1);
            stack.Children.Add(label2);
            stack.Children.Add(label3);
        }
        //безналичными
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            bez.BackgroundColor = Color.Black;

            bez.TextColor = Color.FromHex("#baf54b");

            nal.BackgroundColor = Color.FromHex("#baf54b");

            nal.TextColor = Color.Black;
            
            stack.Children.Clear();

            Label label1 = new Label
            {
                Text = "Оплата безналичными средствами через Сбер Онлайн или через банкомат",
                FontFamily = "akzi",
                FontSize = 17,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center
            };
            Label label2 = new Label
            {
                Text = "Для пополнения счета Вам необходимо:",
                FontFamily = "akzi",
                FontSize = 17,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HorizontalTextAlignment = TextAlignment.Start
            };
            Label label3 = new Label
            {
                Text = "1) Зайти в Сбер Онлайн;",
                FontFamily = "akzi",
                FontSize = 17,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HorizontalTextAlignment = TextAlignment.Start
            };
            Label label4 = new Label
            {
                Text = "2) Набрать в поисковой строке \"Плазмателеком\";",
                FontFamily = "akzi",
                FontSize = 17,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HorizontalTextAlignment = TextAlignment.Start

            };
            Label label5 = new Label
            {
                Text = "3) Ввести Ваш номер договора в поле \"Лицевой счет\";",
                FontFamily = "akzi",
                FontSize = 17,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HorizontalTextAlignment = TextAlignment.Start
            };
            Label label6 = new Label
            {
                Text = "4) Ввести сумму платежа в соответствующее поле;",
                FontFamily = "akzi",
                FontSize = 17,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HorizontalTextAlignment = TextAlignment.Start
            };
            Label label7 = new Label
            {
                Text = "5) Готово!",
                FontFamily = "akzi",
                FontSize = 17,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HorizontalTextAlignment = TextAlignment.Start
            };
            stack.Children.Add(label1);
            stack.Children.Add(label2);
            stack.Children.Add(label3);
            stack.Children.Add(label4);
            stack.Children.Add(label5);
            stack.Children.Add(label6);
            stack.Children.Add(label7);
            
        }
    }
}