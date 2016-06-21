using System;
using System.Collections.Generic;
using Xamarin.Forms;

using MyWorld.Client.UI.Controls;
using MyWorld.Client.UI.Helpers;
using MyWorld.Client.UI;

using MyWorld.Client.Core.ViewModel;
using MyWorld.Client.Core.Model;

namespace MyWorld.Client.UI.Pages
{
    public partial class MapPage : ContentPage
    {
        public MapPage(MapViewModel injectedMapViewModel)
        {
            InitializeComponent();

            //(With NO Dependency Injection of the ViewModel)
            //MapViewModel viewModel = new MapViewModel();

            //Data binding assigned
            this.BindingContext = injectedMapViewModel;

            //Page appearing/disappearing events
            this.Appearing += (sender, args) =>
            {
                injectedMapViewModel.Appearing();
            };

            this.Disappearing += (sender, args) =>
            {
                injectedMapViewModel.Disappearing();
            };
        }

    }
}
