using MyWorld.Client.Core.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;

namespace MyWorld.Client.Core.Services
{

    public interface IVehiclesService
    {
        Task<IList<Vehicle>> GetVehiclesNearby(string tenantId, double latitude, double longitude, double distanceRadians = 0);
    }

    public class VehiclesMockService : IVehiclesService
    {
        public async Task<IList<Vehicle>> GetVehiclesNearby(string tenantId, double latitude, double longitude, double distanceRadians = 0)
        {
            //Vehicles - Mock data
            List<Vehicle>  vehicles = new List<Vehicle>();
            vehicles.Add(new Vehicle { Id = 100, TenantId = "CDLTLL", Make = "Chevrolet", Model = "Camaro", Latitude= 47.644958, Longitude= -122.131077, Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Camaro-RS-2012-small.jpg" });
            vehicles.Add(new Vehicle { Id = 101, TenantId = "CDLTLL", Make = "Chevrolet", Model = "Tahoe", Latitude = 47.661542, Longitude = -122.131231, Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Tahoe-Z71-2015-small.jpg" });

            return await Task.Run(() => vehicles);
            
        }
        
    }
}
