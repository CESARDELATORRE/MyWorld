using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;

using MyWorld.Client.Core.Services;
using MyWorld.Client.Core.ViewModel;

namespace MyWorld.Client.Core.DependencyInjection
{
    public class MyWorldClientCoreModule : Module
    {
        public bool UseMockServices { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            if (UseMockServices) // MockService implementation classes
            {
                //(TODO:) Confirm if SingleInstance is right in this case
                builder.RegisterType<VehiclesMockService>().As<IVehiclesService>().SingleInstance();
                builder.RegisterType<MyWorldRootMockService>().As<IMyWorldRootService>().SingleInstance();               
            }
            else  // Classes consuming Azure / Azure Service Fabric services
            {
                builder.RegisterType<VehiclesAzureSFService>().As<IVehiclesService>().SingleInstance();
                builder.RegisterType<MyWorldRootAzureSFService>().As<IMyWorldRootService>().SingleInstance();
            }

            //Classes to register no matter what 

            //Single Instance for ViewModels     
            builder.RegisterType<MapViewModel>().SingleInstance();
            builder.RegisterType<MyWorldViewModel>().SingleInstance();
            
            //"n" instances for VehicleDetailsViewModel (When implemented)
            //builder.RegisterType<VehicleDetailsViewModel>();
        }
    }

}
