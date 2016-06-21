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
        IMyWorldRootService _myWorldRootService;

        //(CDLTLL) Constructor - TBD - Still with NO dependencies
        public MyWorldViewModel(IMyWorldRootService injectedVehiclesService)
        {
            //myWorldRootService = new MyWorldRootMockService();
            _myWorldRootService = injectedVehiclesService;
            
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

        MyWorldRoot _myWorld;
        public MyWorldRoot MyWorld
        {
            get { return _myWorld; }
            set { _myWorld = value; OnPropertyChanged(); }
        }

        public void Appearing()
        {
            Device.BeginInvokeOnMainThread(ReloadMyWorldRoot);
        }

        private async void ReloadMyWorldRoot()
        {
            MyWorld = await _myWorldRootService.GetMyWorldData();
        }

            bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; OnPropertyChanged(); }
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
