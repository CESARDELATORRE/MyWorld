using System;
using System.Collections.Generic;
using Xamarin.Forms;

using MyWorld.Client.Core.ViewModel;
using MyWorld.Client.UI.Controls;
using MyWorld.Client.UI.Helpers;
using MyWorld.Client.UI;

namespace MyWorld.Client.UI.Pages
{
    public partial class BasicAccordionPage : ContentPage
    {
        public BasicAccordionPage()
        {
            InitializeComponent();

            //(CDLTLL - TBD - Still NO Dependency Injection of the ViewModel)
            MyWorldViewModel viewModel = new MyWorldViewModel();

            this.BindingContext = viewModel;

            MyWorldBasicAccordion.DataSource = PrepareAccordionData();
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
                var label = new Label();
                label.Font = Font.SystemFontOfSize(NamedSize.Default);
                //label.TextColor = Color.Blue;

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

        public List<AccordionSource> PrepareAccordionData()
        {
            var vResult = new List<AccordionSource>();

            #region First List View --> List of Vehicles
            var vVehiclesList = new List<SimpleObject>();

            MyWorldViewModel myWorldViewModel = (MyWorldViewModel) this.BindingContext;
            for (var iCount = 0; iCount < myWorldViewModel.MyWorld.Vehicles.Count; iCount++)
            {
                var vObject = new SimpleObject()
                {
                    TextValue = myWorldViewModel.MyWorld.Vehicles[iCount].Make + " " + myWorldViewModel.MyWorld.Vehicles[iCount].Model,
                    DataValue = myWorldViewModel.MyWorld.Vehicles[iCount].Id.ToString()
                };
                vVehiclesList.Add(vObject);
            }
            var vVehiclesListView = new ListView()
            {
                ItemsSource = vVehiclesList,
                ItemTemplate = new DataTemplate(typeof(ListDataViewCell))
                
            };
            vVehiclesListView.ItemTapped += OnListItemClicked;
            #endregion

            #region StackLayout //NOT USED
            var vViewLayout1 = new StackLayout()
            {
                Children = {
                    new Label { Text = "Static Content:" },
                    new Label { Text = "QWERTY" },
                    new Label { Text = "ASDFGH" }
                }
            };

            var vViewLayout2 = new StackLayout()
            {
                Children = {
                    new Label { Text = "Static Content:" },
                    new Label { Text = "QWERTY" },
                    new Label { Text = "ASDFGH" }
                }
            };
            #endregion

            var vFirstAccordionSection = new AccordionSource()
            {
                HeaderText = "Vehicles",
                HeaderTextColor = Color.Black,
                HeaderBackGroundColor = Color.FromRgb(65,197,255),
                ContentItems = vVehiclesListView
            };
            vResult.Add(vFirstAccordionSection);

            var vSecondAccordionSection = new AccordionSource()
            {
                HeaderText = "People",
                HeaderTextColor = Color.Black,
                HeaderBackGroundColor = Color.FromRgb(65, 197, 255),
                ContentItems = vViewLayout1
            };
            vResult.Add(vSecondAccordionSection);

            var vThirdAccordionSection = new AccordionSource()
            {
                HeaderText = "Tech Items",
                HeaderTextColor = Color.Black,
                HeaderBackGroundColor = Color.FromRgb(65, 197, 255),
                ContentItems = vViewLayout2
            };
            vResult.Add(vThirdAccordionSection);

            return vResult;
        }
    }
}
