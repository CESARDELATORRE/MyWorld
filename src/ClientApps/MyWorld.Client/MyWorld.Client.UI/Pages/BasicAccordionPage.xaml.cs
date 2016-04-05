using System;
using System.Collections.Generic;
using Xamarin.Forms;

using MyWorld.Client.UI.Controls;
using MyWorld.Client.UI.Helpers;
using MyWorld.Client.UI.Pages.Vehicles;

namespace MyWorld.Client.UI.Pages
{
    public partial class BasicAccordionPage : ContentPage
    {
        public BasicAccordionPage()
        {
            InitializeComponent();

            MyWorldBasicAccordion.DataSource = GetSampleData();
            MyWorldBasicAccordion.DataBind();
        }


        void OnListItemClicked(object o, ItemTappedEventArgs e)
        {

            var vListItem = e.Item as SimpleObject;
            var vMessage = "You Clicked on " + vListItem.TextValue + " With Value " + vListItem.DataValue;
            DisplayAlert("Message", vMessage, "Ok");

            var vehicleDetailsPage = new VehicleDetailsPage();

            //(TODO)vehicleDetailspage.Vehicle = veic;
            //await PageNavigationController.PushAsync(Navigation, vehicleDetailsPage);
            Navigation.PushAsync(vehicleDetailsPage, true);

            //(CDLTLL-TODO) --> Check This code from XAMARIN-EVOLVE
            //ListViewEvents.ItemTapped += (sender, e) => ListViewEvents.SelectedItem = null;
            //ListViewEvents.ItemSelected += async (sender, e) =>
            //{
            //    var ev = ListViewEvents.SelectedItem as FeaturedEvent;
            //    if (ev == null)
            //        return;

            //    var eventDetails = new EventDetailsPage();

            //    eventDetails.Event = ev;
            //    App.Logger.TrackPage(AppPage.Event.ToString(), ev.Title);
            //    await NavigationService.PushAsync(Navigation, eventDetails);  // <--------------

            //    ListViewEvents.SelectedItem = null;
            //};

        }

        public class ListDataViewCell : ViewCell
        {
            public ListDataViewCell()
            {
                var label = new Label()
                {
                    Font = Font.SystemFontOfSize(NamedSize.Default),
                    TextColor = Color.Blue
                };
                label.SetBinding(Label.TextProperty, new Binding("TextValue"));
                label.SetBinding(Label.ClassIdProperty, new Binding("DataValue"));
                View = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Padding = new Thickness(12, 8),
                    Children = { label }
                };
            }
        }

        public class SimpleObject
        {
            public string TextValue
            { get; set; }
            public string DataValue
            { get; set; }
        }

        public List<AccordionSource> GetSampleData()
        {
            var vResult = new List<AccordionSource>();

            #region First List View
            var vListOne = new List<SimpleObject>();
            for (var iCount = 0; iCount < 6; iCount++)
            {
                var vObject = new SimpleObject()
                {
                    TextValue = "ObjectNo-" + iCount.ToString(),
                    DataValue = iCount.ToString()
                };
                vListOne.Add(vObject);
            }
            var vListViewOne = new ListView()
            {
                ItemsSource = vListOne,
                ItemTemplate = new DataTemplate(typeof(ListDataViewCell))
            };
            vListViewOne.ItemTapped += OnListItemClicked;
            #endregion

            #region Second List
            var vListTwo = new List<SimpleObject>();
            var vObjectRavi = new SimpleObject()
            {
                TextValue = "Cesar De la Torre",
                DataValue = "1"
            };
            vListTwo.Add(vObjectRavi);
            var vObjectFather = new SimpleObject()
            {
                TextValue = "Program Manager",
                DataValue = "2"
            };
            vListTwo.Add(vObjectFather);
            var vObjectTrainer = new SimpleObject()
            {
                TextValue = "Software Architect",
                DataValue = "3"
            };
            vListTwo.Add(vObjectTrainer);
            var vObjectConsultant = new SimpleObject()
            {
                TextValue = "Developer",
                DataValue = "4"
            };
            vListTwo.Add(vObjectConsultant);
            var vObjectArchitect = new SimpleObject()
            {
                TextValue = "Solution Architect",
                DataValue = "5"
            };
            vListTwo.Add(vObjectArchitect);

            var vListViewTwo = new ListView()
            {
                ItemsSource = vListTwo,
                ItemTemplate = new DataTemplate(typeof(ListDataViewCell))
            };
            vListViewTwo.ItemTapped += OnListItemClicked;
            #endregion

            #region StackLayout
            var vViewLayout = new StackLayout()
            {
                Children = {
                    new Label { Text = "Static Content:" },
                    new Label { Text = "Name : Erika De la Torre" },
                    new Label { Text = "Roles : Student,Daughter,Makeup fan,Friend" }
                }
            };
            #endregion

            var vFirstAccord = new AccordionSource()
            {
                HeaderText = "First",
                HeaderTextColor = Color.Black,
                HeaderBackGroundColor = Color.Yellow,
                ContentItems = vListViewTwo
            };
            vResult.Add(vFirstAccord);
            var vSecond = new AccordionSource()
            {
                HeaderText = "Second ",
                HeaderTextColor = Color.White,
                HeaderBackGroundColor = Color.FromHex("#77d065"),
                ContentItems = vViewLayout
            };
            vResult.Add(vSecond);
            var vThird = new AccordionSource()
            {
                HeaderText = "Third",
                HeaderTextColor = Color.White,
                HeaderBackGroundColor = Color.Purple,
                ContentItems = vListViewOne
            };
            vResult.Add(vThird);
            return vResult;
        }
    }
}
