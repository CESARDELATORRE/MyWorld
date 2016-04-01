using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MyWorld.Client.UI
{
    public class App : Application
    {
        public App()
        {
            MainPage = new MyWorld.Client.UI.Pages.MainPage();

            //(CDLTLL) Original main page created by code
            // The root page of your application
            //MainPage = new ContentPage
            //{
            //    Content = new StackLayout
            //    {
            //        VerticalOptions = LayoutOptions.Center,
            //        Children = {
            //            new Label {
            //                HorizontalTextAlignment = TextAlignment.Center,
            //                //(CDLTLL-Deprecated)XAlign = TextAlignment.Center,
            //                Text = "Hardcoded content.."
            //            }
            //        }
            //    }
            //};
        }


        //public static Page GetMainPage()
        //{
        //    //(CDLTLL)The root page of my app
        //    return new NavigationPage(new MyWorld.Client.UI.Pages.MainPage());
        //}

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
