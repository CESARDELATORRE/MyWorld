using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MyWorld.Client.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            Xamarin.FormsMaps.Init("jPKIGmy6mJauYNubz1DH~JVgCiix388DdQDhmBGhWIw~AuEm-OgmWlIsU05qD6vjEmPhRez3lQKtMH5H-TNNyQ4-sYYuGCjw5bgKfFUyXRNb");

            LoadApplication(new MyWorld.Client.UI.App());
        }
    }
}
