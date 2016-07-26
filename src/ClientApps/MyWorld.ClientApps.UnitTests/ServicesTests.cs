using System;
using System.Threading.Tasks;
using Xunit;

using MyWorld.Client.Core.Services;
using MyWorld.Client.Core.Model;
using System.Collections;
using System.Collections.Generic;

namespace MyWorld.ClientApps.UnitTests
{
    public class ServicesTests
    {

        [Fact]
        public async Task GettingFakeVehiclesDataTest()
        {
            IVehiclesService vehiclesService = new VehiclesMockService();

            IList<Vehicle> vehicleList = await vehiclesService.GetAllVehiclesFromTenant("http://myworldcluster.westus.cloudapp.azure.com:8740/", "CDLTLL");

            await Task.Run(() => { Assert.Equal(3, vehicleList.Count); });

            await Task.Run(() => { Assert.True(true); });
        }

    }
}


    

    