using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Messaging;
using Plugin.Contacts;

namespace XamarinSmrdi
{
    public partial class MainPage : ContentPage
    {
        public MainPage(string DisplayName, string PhoneNumber)
        {
            InitializeComponent();
            ContanctName.Text = DisplayName;
            ContanctNumber.Text = PhoneNumber;

            CircleImage image = new CircleImage
            {
                BorderColor = Color.White,
                BorderThickness = 1,
                HeightRequest = 150,
                WidthRequest = 150,
                FillColor = Color.DimGray,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Source = UriImageSource.FromUri(new Uri("https://i.pinimg.com/564x/74/fc/a2/74fca23df4c3657d1c5d836523f34e70.jpg"))
            };
            StackL.Children.Add(image);
            StackLayout CardLayout = new StackLayout()
            {
                Spacing = 3,

            };
            for (int i = 0; i < 10; i++)
            {
                CardLayout.Children.Add(AddCard("19:35", "0 min 35 s"));
                if (i != 9)
                {
                    CardLayout.Children.Add(new BoxView()
                    {
                        Color = Color.Gray,
                        HeightRequest = 2,
                        Margin = new Thickness(60, 0, 50, 0)

                    });
                }

            }
            Call.Command = new Command(() => MakeACall(PhoneNumber));
            CardsView.Content = CardLayout;

        }
        public StackLayout AddCard(string Text, string TextContent)
        {
            StackLayout CardText = new StackLayout()
            {
                Spacing = 3,
                Children =
                {
                    new Label()
                    {
                        Text = Text
                    },
                    new Label()
                    {
                        Text = TextContent
                    },
                }
            };
            StackLayout Card = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Children =
                {
                    new Image()
                    {
                        WidthRequest = 50,
                        HeightRequest = 40,
                        Source = "https://www.shareicon.net/data/128x128/2015/07/27/75828_phone_256x256.png"
                    },
                    CardText
                }
            };
            return Card;
        }
        public void MakeACall(string PhoneNumber)
        {
            Device.OpenUri(new Uri("tel:" + PhoneNumber));
        }
        protected override bool OnBackButtonPressed()
        {
            Page1 fpm = new Page1();
            Application.Current.MainPage = fpm;
            return true;
        }
    }
}
