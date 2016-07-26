using System;
using System.Collections.Generic;
using Xamarin.Forms;

using MyWorld.Client.Core.ViewModel;
using MyWorld.Client.Core.Model;
using MyWorld.Client.UI.Helpers;

using MyWorld.Client.Core.Services;

namespace MyWorld.Client.UI
{
    public partial class VehiclesListPage : ContentPage
    {
        public VehiclesListPage(MyWorldViewModel injectedMyWorldViewModel,
                                IVehiclesService injectedVehiclesService,
                                VehicleDetailsViewModel.Factory vehicleDetailsViewModelFactory,
                                VehicleDetailsPage.Factory vehicleDetailsPageFactory
                                )
        {
            InitializeComponent();

            this.BindingContext = injectedMyWorldViewModel;

            //Page appearing/disappearing events
            this.Appearing += (sender, args) =>
            {
                injectedMyWorldViewModel.Appearing();
            };

            //When any item-vehicle is clicked, need to show the Vehicle's Detail page
            ListViewVehicles.ItemTapped += (sender, e) => ListViewVehicles.SelectedItem = null;
            ListViewVehicles.ItemSelected += async (sender, e) =>
            {
                var vehicle = ListViewVehicles.SelectedItem as Vehicle;

                if (vehicle == null)
                    return;

                VehicleDetailsPage vehicleDetailsPage = vehicleDetailsPageFactory.Invoke(vehicle,
                                                                                         injectedVehiclesService,
                                                                                         vehicleDetailsViewModelFactory);
                await PageNavigationController.PushAsync(Navigation,
                                                         vehicleDetailsPage);

                ListViewVehicles.SelectedItem = null;
            };

        }
    }
}
