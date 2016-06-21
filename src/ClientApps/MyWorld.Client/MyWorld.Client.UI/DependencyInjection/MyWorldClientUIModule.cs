using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;

using MyWorld.Client.UI.Pages;

namespace MyWorld.Client.UI.DependencyInjection
{
    public class MyWorldClientUIModule : Module
    {
        
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MapPage>().SingleInstance();
            builder.RegisterType<VehiclesListPage>().SingleInstance();
            
        }
    }

}
