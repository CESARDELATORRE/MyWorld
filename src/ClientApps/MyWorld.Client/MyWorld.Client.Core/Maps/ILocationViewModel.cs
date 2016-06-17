using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyWorld.Client.Core.Maps
{
    public interface ILocationViewModel
    {
        string Key { get; set; }
        string Title { get; set; }
        string Description { get; }
        double Latitude { get; }
        double Longitude { get; }
        ICommand Command { get; }
    }
}
