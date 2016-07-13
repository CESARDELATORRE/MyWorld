using System;
using System.Collections.Generic;
using Xamarin.Forms;

using MyWorld.Client.Core.ViewModel;
using MyWorld.Client.Core.Model;
using MyWorld.Client.UI.Helpers;

namespace MyWorld.Client.UI
{
    public partial class VehiclesListPage : ContentPage
    {
        public VehiclesListPage(MyWorldViewModel injectedMyWorldViewModel)
        {
            InitializeComponent();

            this.BindingContext = injectedMyWorldViewModel;

            //Page appearing/disappearing events
            this.Appearing += (sender, args) =>
            {
                injectedMyWorldViewModel.Appearing();
            };

            ListViewVehicles.ItemTapped += (sender, e) => ListViewVehicles.SelectedItem = null;
            ListViewVehicles.ItemSelected += async (sender, e) =>
            {
                var vehicle = ListViewVehicles.SelectedItem as Vehicle;
                if (vehicle == null)
                    return;

                await PageNavigationController.PushAsync(Navigation, new VehicleDetailsPage(vehicle));

                ListViewVehicles.SelectedItem = null;
            };

        }
    }
}
