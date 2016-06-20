﻿
using System;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;
using Plugin.Geolocator.Abstractions;

using MyWorld.Client.Core.Maps;
using MyWorld.Client.Core.Model;
using MyWorld.Client.Core.Services;
using System.Linq;

namespace MyWorld.Client.Core.ViewModel
{
    public class MapViewModel : INotifyPropertyChanged
    {
        IVehiclesService _vehiclesService;

        private Command _doneCommand;
        private Command _refreshCommand;
        private Command _cancelCommand;

        private Func<Task> doneAction;
        public async Task DoneAction()
        {
            if (doneAction != null)
            {
                await doneAction();
            }
        }

        private Func<Task> cancelAction;
        public async Task CancelAction()
        {
            if (cancelAction != null)
            {
                await cancelAction();
            }
        }

        private Plugin.Geolocator.Abstractions.Position _firstLocation = null;
        private readonly Plugin.Geolocator.Abstractions.IGeolocator _geolocator;

        //(CDLTLL) Constructor - TBD - Still with NO dependencies (DI)
        public MapViewModel()
        {
            _geolocator = Plugin.Geolocator.CrossGeolocator.Current; //Needs to be injected/DI with Constructor dependency

            _vehiclesService = new VehiclesMockService();   //Needs to be injected/DI

            Title = "Map";

            //(Redmond's location in case the GPS plugin doesn't work)
            VisibleRegion = MapSpan.FromCenterAndRadius(new Position(47.661407, -122.131213), new Distance(3000));

        }

        // (Reload data from repos everytime the page is appearing, like when hitting the page-tab)
        public void Appearing()
        {
            Device.BeginInvokeOnMainThread(ReloadMapInfo);
        }

        public void Disappearing()
        {
            //TBD
        }

        public string Title { get; set; }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set {
                if (_searchText != value)
                {
                    _searchText = value ?? string.Empty;
                    OnPropertyChanged();

                    // Perform the search
                    if (SearchCommand.CanExecute(null))
                    {
                        SearchCommand.Execute(null);
                    }

                }

            }
        }

        //string tenantID = Settings.TenantID;
        string _tenantID = "CDLTLL";
        public string TenantID
        {
            get { return _tenantID; }
            set
            {
                _tenantID = value;
                OnPropertyChanged();
                //Settings.TenantID = value;
            }
        }

        bool _useMockData = true;
        public bool UseMockData
        {
            get { return _useMockData; }
            set
            {
                _useMockData = value;
                OnPropertyChanged();
            }
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; OnPropertyChanged(); }
        }

        //////////

        Position _mapCenter;
        public Position MapCenter
        {
            get { return _mapCenter; }
            set
            {
                var oldCenter = _mapCenter;
                var distance = MapHelper.CalculateDistance(oldCenter.Latitude, oldCenter.Longitude, value.Latitude, value.Longitude, 'K');

                if (distance > 1)
                {
                    _mapCenter = value;
                    Device.BeginInvokeOnMainThread(ReloadMapInfo);
                }
            }
        }

        Distance _radius;
        public Distance Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                OnPropertyChanged();
                Device.BeginInvokeOnMainThread(ReloadMapInfo);
            }
        }

        MapSpan _visibleRegion;
        public MapSpan VisibleRegion
        {
            get { return _visibleRegion; }
            set
            {
                _visibleRegion = value;
                if (value != null)
                {
                    MapCenter = value.Center;
                    Radius = value.Radius;
                }
                OnPropertyChanged();
            }
        }

        //(CDLTLL) Data in Pins is filtered from actual data in-memory in the collection.
        // Data is not reloaded from the original Services/DB/Mock until the method ReloadMapInfo() is called
        ObservableCollection<ILocationViewModel> _pins;
        public ObservableCollection<ILocationViewModel> Pins
        {
            get
            {
                if (!string.IsNullOrEmpty(_searchText))  //There's text to search for
                {
                    ObservableCollection<ILocationViewModel> pinsSubset = new ObservableCollection<ILocationViewModel>();
                    if (_pins != null)
                    {
                        
                        List<ILocationViewModel> entities = (from p in _pins
                                                            where p.Title.ToUpper().Contains(_searchText.ToUpper())
                                                             select p).ToList<ILocationViewModel>();
                        if (entities != null && entities.Any())
                        {
                            pinsSubset = new ObservableCollection<ILocationViewModel>(entities);
                        }
                    }
                    return pinsSubset;
                }
                else    //if SearchText is empty, return all the pins
                {
                    return _pins;
                }
            }
            set
            {
                _pins = value;
                OnPropertyChanged();
            }
        }


        Vehicle _selectedVehicle;
        public Vehicle SelectedVehicle
        {
            get { return _selectedVehicle; }
            set
            {
                _selectedVehicle = value;
                OnPropertyChanged();
            }
        }

        IList<Vehicle> _vehicles;
        public IList<Vehicle> Vehicles //(CDLTLL - Check this, not being used)
        {
            get { return _vehicles; }
            set
            {
                _vehicles = value;
                OnPropertyChanged();
            }
        }

        private Plugin.Geolocator.Abstractions.Position _lastPosition;
        public async Task<Plugin.Geolocator.Abstractions.Position> GetLocation(bool fetch = false)
        {
            if (_lastPosition == null || fetch)
            {
                return await FetchLocation();
            }

            return _lastPosition;
        }

        private async Task<Plugin.Geolocator.Abstractions.Position> FetchLocation()
        {
            //try
            //{
                _lastPosition = await _geolocator.GetPositionAsync(timeoutMilliseconds: 10000);
            //}
            //catch (Exception e)
            //{
            //}

            return _lastPosition;
        }

        //(CDLTLL) RefreshMap() with real data loading from services/mock is called only if VisibleRegion or Map-Zoom has changed
        private async void ReloadMapInfo()
        {
            if (IsBusy)
               return;
            
            IsBusy = true;

            if (_firstLocation == null)
            {
                _firstLocation = await GetLocation();
                if (_firstLocation != null)
                {
                    VisibleRegion = MapSpan.FromCenterAndRadius(new Position(_firstLocation.Latitude, _firstLocation.Longitude), VisibleRegion.Radius);
                    IsBusy = false;
                    return;
                }
            }

            try
            {
                var distanceRadians =
                    DegreesToRadians(Math.Max(VisibleRegion.LatitudeDegrees, VisibleRegion.LongitudeDegrees) / 2);

                // (CDLTLL) Query and obtain data from a real service or from the mock service depending on the injected implementation
                var vehiclesInCurrentMapArea = await _vehiclesService.GetVehiclesNearby(_tenantID, MapCenter.Latitude, MapCenter.Longitude, distanceRadians);

                //(CDLTLL - Check this, not being used)
                Vehicles = new ObservableCollection<Vehicle>(vehiclesInCurrentMapArea);

                var pinsList = new List<ILocationViewModel>();
                foreach (Vehicle vehicle in vehiclesInCurrentMapArea)
                {
                    Vehicle localVehicle = vehicle;
                    pinsList.Add(new LocationViewModel
                    {
                        Key = vehicle.Id.ToString(),
                        Title = vehicle.Make + " " + vehicle.Model,
                        Latitude = vehicle.Latitude,
                        Longitude = vehicle.Longitude,
                        Description = "LicensePlate: "+ vehicle.LicensePlate,
                        Command = new Command(() => DoneCommand.Execute(localVehicle))
                    });
                }

                //UpdatePins(pins);
                Pins = new ObservableCollection<ILocationViewModel>(pinsList);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command DoneCommand
        {
            get
            {
                return _doneCommand ?? (_doneCommand = new Command(async (t) =>
                {
                    var vehicle = t as Vehicle;
                    if (vehicle != null)
                    {
                        SelectedVehicle = vehicle;
                    }

                    await DoneAction();
                }));
            }
        }

        public Command RefreshCommand
        {
            get { return _refreshCommand ?? (_refreshCommand = new Command(ReloadMapInfo, () => !IsBusy)); }
        }

        public Command CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new Command(async obj =>
                {
                    await CancelAction();
                }));
            }
        }



        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        #region Command and associated methods for SearchCommand
        private Xamarin.Forms.Command _searchCommand;

        public System.Windows.Input.ICommand SearchCommand
        {
            get
            {
                _searchCommand = _searchCommand ?? new Xamarin.Forms.Command(DoSearchCommand, CanExecuteSearchCommand);
                return _searchCommand;
            }
        }

        private void DoSearchCommand()
        {
            // Refresh the list, which will automatically apply the search text

            OnPropertyChanged("Pins");
            //RaisePropertyChanged(() => Pins);
        }

        private bool CanExecuteSearchCommand()
        {
            return true;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    }
}
