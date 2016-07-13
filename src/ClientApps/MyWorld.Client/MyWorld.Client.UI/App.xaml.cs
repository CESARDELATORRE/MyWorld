using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using MyWorld.Client.Core.ViewModel;
using MyWorld.Client.UI.Pages;
using MyWorld.Client.UI;

using Autofac;

using MyWorld.Client.Core.DependencyInjection;
using MyWorld.Client.UI.DependencyInjection;

namespace MyWorld.Client.UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //Bootstrap from DI/IoC container
            var builder = new ContainerBuilder();

            builder.RegisterModule<MyWorldClientUIModule>();

            builder.RegisterModule(new MyWorldClientCoreModule(){UseMockServices = false});

            var container = builder.Build();
            //
            #region Sample XML for Autofac Module's configuration
            // AutoFac can also register Modules in XML, like:
            /*
            < module type = "MyWorldClientCoreModule" >
                < properties >
                    < property name = "ObeySpeedLimit" value = "true" />
                </ properties >
            </ module >
            */
            #endregion

            #region Test with single page
            //(TESTING) Single page --> MapPage
            //var page = container.Resolve<MapPage>();
            //MainPage = new NavigationPage(page);
            #endregion

            Xamarin.Forms.OnPlatform<Xamarin.Forms.Color> onPlatWindowBkgColor =
                    (Xamarin.Forms.OnPlatform<Xamarin.Forms.Color>)Application.Current.Resources["WindowBackground"];

            // Using TabbedPage as root of the app 
            var tabs = new TabbedPage();

            tabs.Title = "MyWorld";
            tabs.BackgroundColor = onPlatWindowBkgColor;
            tabs.Children.Add(container.Resolve<VehiclesListPage>());
            tabs.Children.Add(container.Resolve<MapPage>());            
            tabs.Children.Add(container.Resolve<SettingsPage>());

            //Get Bar-Background color from the App's resources
            //Xamarin.Forms.OnPlatform<Xamarin.Forms.Color> onPlatBarBkgColor = 
            //        (Xamarin.Forms.OnPlatform<Xamarin.Forms.Color>) Application.Current.Resources["BarBackgroundColor"];

            //Xamarin.Forms.OnPlatform<Xamarin.Forms.Color> onPlatBarTextColor =
            //        (Xamarin.Forms.OnPlatform<Xamarin.Forms.Color>)Application.Current.Resources["BarBackgroundColor"];

            MainPage = new NavigationPage(tabs)
            {
                //BarBackgroundColor = onPlatBarBkgColor,
                //BarTextColor = onPlatBarTextColor
            };
            
            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

