using System.Windows.Input;

namespace MyWorld.Client.Core.Maps
{
    public class LocationViewModel : ILocationViewModel
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICommand Command { get; set; }
    }
}