using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Contacts;
using Plugin.Contacts.Abstractions;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace XamarinSmrdi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public ObservableCollection<Plugin.Contacts.Abstractions.Contact> Contacts { get; private set; }
        public Page1()
        {
            InitializeComponent();

            this.Contacts = new ObservableCollection<Plugin.Contacts.Abstractions.Contact>();
            ReloadContacts();

            //Contacts[0].

            for (int i = 0; i < Contacts.Count(); i++)
            {
                Main.Children.Add(AddConctact(Contacts[i].DisplayName,Contacts[i].Phones[0].Number));
                if (i != Contacts.Count()-1)
                {
                    Main.Children.Add(new BoxView()
                    {
                        Color = Color.LightGray,
                        HeightRequest = 2,
                       // Margin = new Thickness(60, 0, 50, 0)
                    });
                }
            }
            ScrollView Scroll = new ScrollView()
            {
                Content = Main
            };
           
            this.Content = Scroll;
        }
        public void ReloadContacts()
        {
            // Device may request user permission to get contacts access.
            var hasPermission = CrossContacts.Current.RequestPermission()
                .GetAwaiter()
                .GetResult();

            if (hasPermission)
            {
                this.Contacts.Clear();

                List<Plugin.Contacts.Abstractions.Contact> contacts = null;
                CrossContacts.Current.PreferContactAggregation = false;
                if (CrossContacts.Current.Contacts == null)
                {
                    return;
                }

                contacts = CrossContacts.Current.Contacts.ToList();
                foreach (var contact in contacts)
                {
                    this.Contacts.Add(contact);
                }
                
            }
        }
        public void ClickAnimation(StackLayout tag)
        {
            var ClickAnimation = new Animation(v => tag.Scale = v, 0.90, 1, Easing.Linear);
            ClickAnimation.Commit(this, "ClickAnimation", 25, 150, null);
        }
        public StackLayout AddConctact(string DisplayName, string PhoneNumber)
        {

            CircleImage Image = new CircleImage
            {
                Margin = new Thickness(0, 0, 10, 0),
                BorderColor = Color.White,
                BorderThickness = 1,
                HeightRequest = 75,
                WidthRequest = 75,
                FillColor = Color.DimGray,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Start,
                Source = UriImageSource.FromUri(new Uri("https://i.pinimg.com/564x/74/fc/a2/74fca23df4c3657d1c5d836523f34e70.jpg"))
            };
            StackLayout Info = new StackLayout()
            {
                Spacing = 3,

                Children =
                {
                    new Label
                    {
                        Text = DisplayName,
                        TextColor = Color.Black,
                        FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),

                    },
                    new Label
                    {
                        Text = PhoneNumber,
                        TextColor = Color.FromHex("#8c8c8c"),
                        FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
                    }
                }
            };
            StackLayout Card = new StackLayout()
            {
                Spacing = 0,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    Image,
                    Info
                }
            };
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) => {
                
                ClickAnimation(Card);
                await DelayAsync();
                MainPage fpm = new MainPage(DisplayName, PhoneNumber);
                Application.Current.MainPage = fpm;
            };
            Card.GestureRecognizers.Add(tapGestureRecognizer);
            return Card;
        }
        public async Task DelayAsync()
        {
            await Task.Delay(1000);
        }

    }
}