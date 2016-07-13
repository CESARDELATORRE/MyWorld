using System;
using System.Threading.Tasks;
using Xunit;

using MyWorld.Client.Core.Services;
using MyWorld.Client.Core.Model;

namespace MyWorld.ClientApps.UnitTests
{
    public class ServicesTests
    {

        [Fact]
        public async Task GettingFakeVehiclesDataTest()
        {
            IMyWorldRootService myWorldRootService;
            IVehiclesService vehiclesService = new VehiclesMockService(); 
            myWorldRootService = new MyWorldRootMockService(vehiclesService);
            MyWorldRoot myWorld = await myWorldRootService.GetMyWorldData("CDLTLL");

            await Task.Run(() => { Assert.Equal(3, myWorld.Vehicles.Count); });
        }

    }
}


    

    