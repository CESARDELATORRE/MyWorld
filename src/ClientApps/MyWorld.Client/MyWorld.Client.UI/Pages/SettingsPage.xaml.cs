﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using MyWorld.Client.Core.ViewModel;

namespace MyWorld.Client.UI.Pages
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage(SettingsViewModel injectedMapViewModel)
        {
            InitializeComponent();

            //Data binding assigned
            this.BindingContext = injectedMapViewModel;
        }
    }
}
