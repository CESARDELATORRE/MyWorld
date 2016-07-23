// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyWorld.Client.Core.Helpers
{
    /// <summary>
    /// This is the Settings class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public class Settings : INotifyPropertyChanged
    {
        private static ISettings AppSettings 
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        static Settings _settings;

        /// <summary>
        /// Gets or sets the current settings. 
        /// *** This static factory method should ONLY be used if NOT using DI ***
        /// </summary>
        /// <value>The current.</value>
        public static Settings Current
        {
            get { return _settings ?? (_settings = new Settings()); }
        }

        //Local Service Fabric cluster in your Dev Machine
        private const string LocalServicelBaseUriKey = "local_service_base_uri";
        private static readonly string LocalServicelBaseUriDefault = "http://192.168.88.214:8740/"; //"http://YOUR-LOCAL-IP:YOUR-PORT/";   //Example http://192.168.88.214:8740/
        public string LocalServicelBaseUri
        {
            get
            {
                return AppSettings.GetValueOrDefault(LocalServicelBaseUriKey, LocalServicelBaseUriDefault);
            }
            set
            {
                if(AppSettings.AddOrUpdateValue(LocalServicelBaseUriKey, value))
                    OnPropertyChanged();
            }
        }

        //Remote/Cloud Service Fabric cluster in Azure's cloud
        private const string CloudServicelBaseUriKey = "cloud_service_base_uri"; 
        private static readonly string CloudServicelBaseUriDefault = "http://myworldcluster.westus.cloudapp.azure.com:8740/";   //Example http://yourclustername.westus.cloudapp.azure.com:9595/
        public string CloudServicelBaseUri
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(CloudServicelBaseUriKey, CloudServicelBaseUriDefault);
            }
            set
            {
                if(AppSettings.AddOrUpdateValue<string>(CloudServicelBaseUriKey, value))
                    OnPropertyChanged();
            }
        }

        const string UseCloudKey = "use_cloud";
        static readonly bool UseCloudDefault = false;

        public bool UseCloud
        {
            get { return AppSettings.GetValueOrDefault<bool>(UseCloudKey, UseCloudDefault); }
            set
            {
                if (AppSettings.AddOrUpdateValue<bool>(UseCloudKey, value))
                    OnPropertyChanged();
            }
        }

        private const string CurrentTenantIdKey = "current_tenant_id";
        private static readonly string CurrentTenantIdDefault = "CDLTLL";
        public string CurrentTenantId
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(CurrentTenantIdKey, CurrentTenantIdDefault);
            }
            set
            {
                if (AppSettings.AddOrUpdateValue<string>(CurrentTenantIdKey, value))
                    OnPropertyChanged();
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string name = "") =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion

    }
}