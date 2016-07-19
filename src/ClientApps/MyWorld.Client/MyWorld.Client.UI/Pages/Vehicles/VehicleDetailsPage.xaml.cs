using MyWorld.Client.Core.Model;
using MyWorld.Client.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using MyWorld.Client.Core.Services;

namespace MyWorld.Client.UI
{
    public partial class VehicleDetailsPage : ContentPage
    {
        public delegate VehicleDetailsPage Factory(Vehicle vehicle,
                                                   IVehiclesService vehicleService, 
                                                   VehicleDetailsViewModel.Factory vehicleDetailsViewModelFactory);
        public VehicleDetailsPage(
                                  Vehicle vehicle,
                                  IVehiclesService vehicleService,
                                  VehicleDetailsViewModel.Factory vehicleDetailsViewModelFactory
                                 )
        {
            InitializeComponent();

            Guid vehicleId = vehicle.Id;

            this.BindingContext = vehicleDetailsViewModelFactory.Invoke(vehicleService, vehicle);
        }
    }
}


