using MyWorld.Client.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyWorld.Client.UI
{
    public partial class VehicleDetailsPage : ContentPage
    {
        public VehicleDetailsPage(Vehicle vehicle)
        {
            InitializeComponent();

            Guid vehicleId = vehicle.Id;

            //TO DO - TBD - Still without DI for the ViewModel
            //this.BindingContext = new VehicleDetailsViewModel(vehicle);
        }
    }
}
