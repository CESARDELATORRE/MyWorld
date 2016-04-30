using System;
using System.Collections.Generic;
using Xamarin.Forms;

using MyWorld.Client.Core.ViewModel;
using MyWorld.Client.Core.Model;

namespace MyWorld.Client.UI
{
    public partial class VehiclesListPage : ContentPage
    {
        public VehiclesListPage()
        {
            InitializeComponent();


            //(CDLTLL - TBD - Still NO Dependency Injection of the ViewModel)
            MyWorldViewModel viewModel = new MyWorldViewModel();

            //List<Vehicle> tmpVehicles = viewModel.MyWorld.Vehicles;
            //string model = tmpVehicles[1].Model;

            this.BindingContext = viewModel;
        }
    }
}
