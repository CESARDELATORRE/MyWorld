using MyWorld.Client.Core.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using Plugin.Geolocator;

using MyWorld.Client.Core.Services;
using MyWorld.Client.Core.Helpers;


namespace MyWorld.Client.Core.ViewModel
{
    public class VehicleDetailsViewModel : INotifyPropertyChanged
    {
        public delegate VehicleDetailsViewModel Factory(IVehiclesService vehiclesService, Vehicle vehicle);
        IVehiclesService _vehiclesService;
        private readonly Plugin.Geolocator.Abstractions.IGeolocator _geolocator;

        //(CDLTLL) Constructor with injected dependencies
        public VehicleDetailsViewModel(IVehiclesService vehiclesService, Vehicle vehicle)
        {
            //Injected dependencies
            _vehiclesService = vehiclesService;
            _vehicle = vehicle;

            _geolocator = Plugin.Geolocator.CrossGeolocator.Current;
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
            set {
                    _vehicle = value;
                    OnPropertyChanged();
                }
        }

        public Double Latitude
        {
            get { return _vehicle.Latitude; }
            set
            {
                _vehicle.Latitude = value;
                OnPropertyChanged();
            }
        }

        public Double Longitude
        {
            get { return _vehicle.Longitude; }
            set
            {
                _vehicle.Longitude = value;
                OnPropertyChanged();
            }
        }

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
                //If Guid is empty, then is a new Vehicle to Create and the GUID will be generated in the microservices
                if (Vehicle.Id == Guid.Empty)  
                {
                    Guid createdVehicleGuid = await _vehiclesService.CreateVehicle(this.UrlPrefix, this.CurrentTenantId, Vehicle);
                }
                else
                {
                    Guid updatedVehicleGuid = await _vehiclesService.UpdateVehicle(this.UrlPrefix, this.CurrentTenantId, Vehicle);
                }
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

        ICommand _getGPSCoordinates;
        public ICommand GetGPSCoordinatesCommand =>
                _getGPSCoordinates ??
                (_getGPSCoordinates = new Command(async () => await ExecuteGetGPSCoordinatesCommand()));

        private async Task ExecuteGetGPSCoordinatesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                var localCoordinates = await _geolocator.GetPositionAsync(timeoutMilliseconds: 10000);
                //weatherRoot = await WeatherService.GetWeather(local.Latitude, local.Longitude
                Latitude = localCoordinates.Latitude;
                Longitude = localCoordinates.Longitude;
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
