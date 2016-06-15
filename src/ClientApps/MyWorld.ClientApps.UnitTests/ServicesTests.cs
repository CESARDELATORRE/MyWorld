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
        public void GettingFakeVehiclesDataTest()
        {
            IMyWorldRootService myWorldRootService;
            myWorldRootService = new MyWorldRootMockService();
            MyWorldRoot myWorld = myWorldRootService.GetMyWorldData();

            Assert.Equal(2, myWorld.Vehicles.Count);
        }

    }
}


    

    