using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;

using MyWorld.Client.Core.Services;
using MyWorld.Client.Core.ViewModel;

using MyWorld.Client.Core.Helpers;

namespace MyWorld.Client.Core.DependencyInjection
{
    public class MyWorldClientCoreModule : Module
    {
        public bool UseMockServices { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            if (UseMockServices) // MockService implementation classes
            {
                builder.RegisterType<VehiclesMockService>().As<IVehiclesService>().InstancePerDependency();
                builder.RegisterType<MyWorldRootMockService>().As<IMyWorldRootService>().InstancePerDependency();               
            }
            else  // Classes consuming Azure / Azure Service Fabric services
            {
                builder.RegisterType<VehiclesAzureSFService>().As<IVehiclesService>().InstancePerDependency();
                builder.RegisterType<MyWorldRootAzureSFService>().As<IMyWorldRootService>().InstancePerDependency();
            }

            //Classes to register no matter what 

            //Single Instance for ViewModels     
            builder.RegisterType<MapViewModel>().SingleInstance();
            builder.RegisterType<MyWorldViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();

            //"n" instances for VehicleDetailsViewModel (When implemented)
            //builder.RegisterType<VehicleDetailsViewModel>();
        }
    }

}
