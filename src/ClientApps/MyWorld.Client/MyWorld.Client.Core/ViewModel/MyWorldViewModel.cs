using MyWorld.Client.Core.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using MyWorld.Client.Core.Services;

namespace MyWorld.Client.Core.ViewModel
{

    public class MyWorldViewModel : INotifyPropertyChanged
    {
        IMyWorldRootService myWorldRootService;

        //(CDLTLL) Constructor - TBD - Still with NO dependencies
        public MyWorldViewModel()
        {
            myWorldRootService = new MyWorldRootMockService();

            myWorld = myWorldRootService.GetMyWorld();
        }

        public string Title { get; set; }
        

        //string tenantID = Settings.TenantID;
        string tenantID = "CDLTLL";
        public string TenantID
        {
            get { return tenantID; }
            set
            {
                tenantID = value;
                OnPropertyChanged();
                //Settings.TenantID = value;
            }
        }

        MyWorldRoot myWorld;
        public MyWorldRoot MyWorld
        {
            get { return myWorld; }
            set { myWorld = value; OnPropertyChanged(); }
        }


        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; OnPropertyChanged(); }
        }

        bool useMockData = true;
        public bool UseMockData
        {
            get { return useMockData; }
            set
            {
                useMockData = value;
                OnPropertyChanged();
            }
        }

        //ICommand getMyWorld;
        //public ICommand GetMyWorldCommand =>
        //        getMyWorld ??
        //        (getMyWorld = new Command(async () => await ExecuteGetMyWorldCommand()));

        //private async Task ExecuteGetMyWorldCommand()
        //{
        //    if (IsBusy)
        //        return;

        //    IsBusy = true;
        //    try
        //    {
        //        MyWorldRoot myWorldRoot = null;
                
        //        if (useMockData)
        //        {
        //            //Get Items from Mock Data
        //            myWorldRoot = await ItemsService.GetWeather(local.Latitude, local.Longitude, units);
        //        }
        //        else
        //        {
        //            //Get Items from real Services

        //            //(CDLTLL) Get GPS position - var local = await CrossGeolocator.Current.GetPositionAsync(10000);

        //            myWorldRoot = await ItemsService.GetWeather(Location.Trim(), units);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //(CDLTLL) Xamarin.Insights.Report(ex);
        //        throw;
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
