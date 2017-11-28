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
    public partial class TabbedPage1 : TabbedPage
    {
        public ObservableCollection<Plugin.Contacts.Abstractions.Contact> Contacts { get; private set; }
        public TabbedPage1()
        {
            InitializeComponent();

            this.Contacts = new ObservableCollection<Plugin.Contacts.Abstractions.Contact>();
            ReloadContacts();

            StackLayout Main = new StackLayout()
            {
                BackgroundColor = Color.White,
            };
            for (int i = 0; i < Contacts.Count(); i++)
            {
                Main.Children.Add(AddConctact(Contacts[i].DisplayName, Contacts[i].Phones[0].Number));
                if (i != Contacts.Count() - 1)
                {
                    Main.Children.Add(new BoxView()
                    {
                        Color = Color.LightGray,
                        HeightRequest = 2,
                        // Margin = new Thickness(60, 0, 50, 0)
                    });
                }
            }
            ContactPic = new CircleImage
            {
                BorderColor = Color.White,
                BorderThickness = 1,
                HeightRequest = 150,
                WidthRequest = 150,
                FillColor = Color.DimGray,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
               // Source = UriImageSource.FromUri(new Uri("http://i0.kym-cdn.com/photos/images/original/001/265/980/58e.jpg"))
            };
            StackL.Children.Add(ContactPic);
            ScrollView Scroll = new ScrollView()
            {
                Content = Main
            };

            ContactsTab.Content = Scroll;
            
        }
        public void DetailInit(string DisplayName, string PhoneNumber)
        {
            ContanctName.Text = DisplayName;
            ContanctNumber.Text = PhoneNumber;
            //StackL.Children.RemoveAt(0);
            ContactPic.Source = "Umaru.jpg";
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
            CardsView.Content = CardLayout;
            Call.Command = new Command(() => MakeACall(PhoneNumber));
            Msg.Command = new Command(() => SendSms(PhoneNumber));
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
                        Source = "Phone.png"
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
        public void SendSms(string PhoneNumber)
        {
            Device.OpenUri(new Uri("sms:" + PhoneNumber));
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
                Source = "megu.jpg"
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
                await DelayAsync(1000);
                DetailInit(DisplayName, PhoneNumber);
                //await DelayAsync(1000);
                this.CurrentPage = this.Children[1];
            };
            Card.GestureRecognizers.Add(tapGestureRecognizer);
            return Card;
        }
        public async Task DelayAsync(int time)
        {
            await Task.Delay(time);
        }
        protected override bool OnBackButtonPressed()
        {
            if (this.CurrentPage == this.Children[1])
            {
                this.CurrentPage = this.Children[0];
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}