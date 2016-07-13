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

    public class MyWorldViewModel : INotifyPropertyChanged
    {
        IMyWorldRootService _myWorldRootService;

        //(CDLTLL) Constructor with injected dependencies
        public MyWorldViewModel(IMyWorldRootService injectedVehiclesService)
        {
            //Injected dependencies
            _myWorldRootService = injectedVehiclesService;
        }

        public string Title { get; set; }


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

        MyWorldRoot _myWorld;
        public MyWorldRoot MyWorld
        {
            get { return _myWorld; }
            set { _myWorld = value; OnPropertyChanged(); }
        }

        public void Appearing()
        {
            this.GetMyWorldCommand.Execute(false);

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

        ICommand _getMyWorldData;
        public ICommand GetMyWorldCommand =>
                _getMyWorldData ??
                (_getMyWorldData = new Command(async () => await ExecuteGetMyWorldDataCommand()));

        private async Task ExecuteGetMyWorldDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                this.MyWorld = await _myWorldRootService.GetMyWorldData(this.UrlPrefix, this.CurrentTenantId);
                if (_vehicles != null)
                    VehiclesToShow.Clear();

                VehiclesToShow = new ObservableCollection<Vehicle>(this.MyWorld.Vehicles);
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

        //(CDLTLL) Data in Vehicles is filtered from actual data in-memory in the collection.
        // Data is not reloaded from the original Services/DB/Mock until the method ExecuteGetMyWorldDataCommand() is called
        ObservableCollection<Vehicle> _vehicles;
        public ObservableCollection<Vehicle> VehiclesToShow
        {
            get
            {
                if (!string.IsNullOrEmpty(_searchText))  //There's text to search for
                {
                    ObservableCollection<Vehicle> vehiclesSubset = new ObservableCollection<Vehicle>();
                    if (_vehicles != null)
                    {

                        List<Vehicle> entities = (from p in _vehicles
                                                  where p.FullTitle.ToUpper().Contains(_searchText.ToUpper())
                                                  select p).ToList<Vehicle>();
                        if (entities != null && entities.Any())
                        {
                            vehiclesSubset = new ObservableCollection<Vehicle>(entities);
                        }
                    }
                    return vehiclesSubset;
                }
                else    //if SearchText is empty, return all the vehicles
                {
                    return _vehicles;
                }
            }
            set
            {
                _vehicles = value;
                OnPropertyChanged();
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
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

            OnPropertyChanged("VehiclesToShow");
            //RaisePropertyChanged(() => Vehicles);
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
