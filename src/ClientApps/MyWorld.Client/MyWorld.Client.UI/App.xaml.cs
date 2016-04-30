using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using MyWorld.Client.Core.ViewModel;
using MyWorld.Client.UI.Pages;
using MyWorld.Client.UI.Pages.Vehicles;

namespace MyWorld.Client.UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //(CDLTLL) ViewModelBase.Init();  //Check in EVOLVE app Related to Test Cloud and DependencyService.Register for mocking or real, etc...

            //(CDLTLL) Navigation page - Not used now
            //MainPage = new NavigationPage(new MyWorld.Client.UI.Pages.MapPage());

            
            Xamarin.Forms.OnPlatform<Xamarin.Forms.Color> onPlatWindowBkgColor =
                    (Xamarin.Forms.OnPlatform<Xamarin.Forms.Color>)Application.Current.Resources["WindowBackground"];

            //(Using TabbedPage as root of the app)
            var tabs = new TabbedPage
            {
                Title = "MyWorld",
                BackgroundColor = onPlatWindowBkgColor,
                BindingContext = new MyWorldViewModel(),
                Children =
                {
                    new VehiclesListPage(),
                    //new BasicAccordionPage(),
                    //new MyWorldListPage(),
                    //new WeatherAccordionPage(),
                    new MapPage(),
                    //new SimpleListPage()
                }
            };


            //(CDLTLL) Get Bar-Background color from the App's resources
            Xamarin.Forms.OnPlatform<Xamarin.Forms.Color> onPlatBarBkgColor = 
                    (Xamarin.Forms.OnPlatform<Xamarin.Forms.Color>) Application.Current.Resources["BarBackgroundColor"];

            Xamarin.Forms.OnPlatform<Xamarin.Forms.Color> onPlatBarTextColor =
                    (Xamarin.Forms.OnPlatform<Xamarin.Forms.Color>)Application.Current.Resources["BarBackgroundColor"];

            MainPage = new NavigationPage(tabs)
            {
                BarBackgroundColor = onPlatBarBkgColor,
                BarTextColor = onPlatBarTextColor
                //BarBackgroundColor = Color.Blue,
                //BarTextColor = Color.Yellow
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

