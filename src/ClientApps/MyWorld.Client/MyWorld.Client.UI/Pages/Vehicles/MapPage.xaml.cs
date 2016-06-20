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
        public MapPage()
        {
            InitializeComponent();

            //(CDLTLL - TBD - Still NO Dependency Injection of the ViewModel)
            MapViewModel viewModel = new MapViewModel();

            this.BindingContext = viewModel;

            //this.Appearing += (sender, args) =>
            //{
            //    viewModel.Appearing();
            //};

            //this.Disappearing += (sender, args) =>
            //{
            //    viewModel.Disappearing();
            //};
        }

    }
}
