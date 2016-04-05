using System;
using System.Windows.Input;

namespace MyWorld.Client.UI.Interfaces
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }

        ICommand SelectCommand { get; set; }
    }
}
