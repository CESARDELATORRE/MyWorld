using MyWorld.Client.Core.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

using MyWorld.Client.Core.Services;
using MyWorld.Client.Core.Helpers;


namespace MyWorld.Client.Core.ViewModel
{
    public class VehicleDetailsViewModel : INotifyPropertyChanged
    {
        public delegate VehicleDetailsViewModel Factory(IVehiclesService vehiclesService, Vehicle vehicle);
        IVehiclesService _vehiclesService;

        //(CDLTLL) Constructor with injected dependencies
        public VehicleDetailsViewModel(IVehiclesService vehiclesService, Vehicle vehicle)
        {
            //Injected dependencies
            _vehiclesService = vehiclesService;
            _vehicle = vehicle;
        }

        //TenantId 
        public string CurrentTenantId
        {
            get { return Settings.Current.CurrentTenantId; }
        }

        public string UrlPrefix
        {
            get
            {
                if (Settings.Current.UseCloud)
                    return Settings.Current.CloudServicelBaseUri;
                else
                    return Settings.Current.LocalServicelBaseUri;
            }
        }

        Vehicle _vehicle;
        public Vehicle Vehicle
        {
            get { return _vehicle; }
            set { _vehicle = value; OnPropertyChanged(); }
        }

        public void Appearing()
        {
            //this.GetMyWorldCommand.Execute(false);

            //Method 2 to refresh
            //Device.BeginInvokeOnMainThread(ReloadMyWorldRoot);
        }

        //Method 2 to refresh
        //private async void ReloadMyWorldRoot()
        //{
        //    this.MyWorld = await _myWorldRootService.GetMyWorldData(this.UrlPrefix, this.CurrentTenantId);
        //}

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; OnPropertyChanged(); }
        }

        ICommand _saveVehicle;
        public ICommand SaveVehicleCommand =>
                _saveVehicle ??
                (_saveVehicle = new Command(async () => await ExecuteSaveVehicleCommand()));

        private async Task ExecuteSaveVehicleCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Guid createdVehicleGuid = await _vehiclesService.CreateVehicle(this.UrlPrefix, this.CurrentTenantId, _vehicle);
            }
            catch (Exception ex)
            {
                //(CDLTLL) Xamarin.Insights.Report(ex);  // --> Add here HockeyApp telemetry for crash/exception, etc.
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
