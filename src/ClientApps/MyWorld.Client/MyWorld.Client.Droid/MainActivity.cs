
using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


namespace MyWorld.Client.Droid
{
    //(CDLTLL) Changed from FormsApplicationActivity to FormsAppCompatActivity so changes in tabs and toolbar work
    //(CDLTLL) https://blog.xamarin.com/material-design-for-your-xamarin-forms-android-apps/

    [Activity(Label = "MyWorld", Icon = "@drawable/icon", MainLauncher = true)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //(CDLTLL) Config for Tab colors
            //FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new MyWorld.Client.UI.App());
        }
    }
}

