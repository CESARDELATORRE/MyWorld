using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using MyWorld.Client.Core.ViewModel;

namespace MyWorld.Client.UI.Pages
{
    public partial class SimpleListPage : ContentPage
    {
        public SimpleListPage()
        {
            InitializeComponent();

            //(CDLTLL - TBD - Still NO Dependency Injection of the ViewModel)
            MyWorldViewModel viewModel = new MyWorldViewModel();

            BindingContext = viewModel;
        }
    }
}
