using System;
using System.Collections.Generic;
using Xamarin.Forms;

using MyWorld.Client.Core.ViewModel;
using MyWorld.Client.Core.Model;

namespace MyWorld.Client.UI
{
    public partial class VehiclesListPage : ContentPage
    {
        public VehiclesListPage(MyWorldViewModel injectedMyWorldViewModel)
        {
            InitializeComponent();

            //(CDLTLL - TBD - Still NO Dependency Injection of the ViewModel)
            //MyWorldViewModel viewModel = new MyWorldViewModel();

            this.BindingContext = injectedMyWorldViewModel;

            //Page appearing/disappearing events
            this.Appearing += (sender, args) =>
            {
                injectedMyWorldViewModel.Appearing();
            };

        }
    }
}
