using System;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using MyWorld.Client.Core.Helpers;

namespace MyWorld.Client.Core.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        Settings _settings;

        //(CDLTLL) Constructor - DI: Dependencies injected in the Constructor
        public SettingsViewModel()
        {
            
        }

        public string CurrentTenantId
        {
            get { return Settings.Current.CurrentTenantId; }
            set
            {
                if (Settings.Current.CurrentTenantId == value)
                    return;

                Settings.Current.CurrentTenantId = value;
                OnPropertyChanged();
            }
        }

        public bool UseCloud
        {
            get { return Settings.Current.UseCloud; }
            set
            {
                if (Settings.Current.UseCloud == value)
                    return;

                Settings.Current.UseCloud = value;
                OnPropertyChanged();
            }
        }

        public string LocalServiceBaseUri
        {
            get { return Settings.Current.LocalServicelBaseUri; }
            set
            {
                if (Settings.Current.LocalServicelBaseUri == value)
                    return;

                Settings.Current.LocalServicelBaseUri = value;
                OnPropertyChanged();
            }
        }

        public string CloudServiceBaseUri
        {
            get { return Settings.Current.CloudServicelBaseUri; }
            set
            {
                if (Settings.Current.CloudServicelBaseUri == value)
                    return;

                Settings.Current.CloudServicelBaseUri = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    }
}

