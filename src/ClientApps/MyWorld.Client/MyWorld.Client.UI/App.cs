using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using MyWorld.Client.Core.ViewModel;
using MyWorld.Client.UI.Pages;

namespace MyWorld.Client.UI
{
    public class App : Application
    {
        public App()
        {
            //(CDLTLL) Navigation page - Not used now
            // MainPage = new NavigationPage(new MyWorld.Client.UI.Pages.MainPage());

            //(Using TabbedPage as root of the app)
            var tabs = new TabbedPage
            {
                Title = "MyWorld",
                BindingContext = new MyWorldViewModel(),
                Children =
                {
                    new MyWorldListPage(),
                    new MapPage()
                }
            };

            MainPage = new NavigationPage(tabs)
            {
                BarBackgroundColor = Color.FromHex("3498db"),
                BarTextColor = Color.White
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
