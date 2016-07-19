using MyWorld.Client.Core.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;

namespace MyWorld.Client.Core.Services
{
    public class VehiclesMockService : IVehiclesService
    {
        public async Task<IList<Vehicle>> GetVehiclesInArea(string urlPrefix, string tenantId, double topLatitude, double leftLongitude, double bottomLatitude, double rightLongitude)
        {
            //Vehicles - Mock data
            List<Vehicle>  vehicles = new List<Vehicle>();
            vehicles.Add(new Vehicle { Id = Guid.NewGuid(), TenantId = "CDLTLL", Make = "Chevrolet", Model = "Camaro", Latitude= 47.644958, Longitude= -122.131077, Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Camaro-RS-2012-small.jpg" });
            vehicles.Add(new Vehicle { Id = Guid.NewGuid(), TenantId = "CDLTLL", Make = "Chevrolet", Model = "Tahoe", Latitude = 47.661542, Longitude = -122.131231, Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Tahoe-Z71-2015-small.jpg" });

            vehicles.Add(new Vehicle { Id = Guid.NewGuid(), TenantId = "TENANT2", Make = "Ford", Model = "Mustang", Latitude = 47.654177, Longitude = -122.132442, Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Camaro-RS-2012-small.jpg" });
            vehicles.Add(new Vehicle { Id = Guid.NewGuid(), TenantId = "TENANT2", Make = "Ford", Model = "Explorer", Latitude = 47.645120, Longitude = -122.138143, Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Tahoe-Z71-2015-small.jpg" });

            return await Task.Run(() => vehicles);
        }

        public async Task<IList<Vehicle>> GetAllVehiclesFromTenant(string urlPrefix, string tenantId)
        {
            //Vehicles - Mock data
            List<Vehicle> vehicles = new List<Vehicle>();
            vehicles.Add(new Vehicle { Id = Guid.NewGuid(), TenantId = "CDLTLL", Make = "Chevrolet", Model = "Camaro", Latitude = 47.644958, Longitude = -122.131077, Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Camaro-RS-2012-small.jpg" });
            vehicles.Add(new Vehicle { Id = Guid.NewGuid(), TenantId = "CDLTLL", Make = "Chevrolet", Model = "Tahoe", Latitude = 47.661542, Longitude = -122.131231, Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Tahoe-Z71-2015-small.jpg" });
            vehicles.Add(new Vehicle { Id = Guid.NewGuid(), TenantId = "CDLTLL", Make = "BMW", Model = "Z4", Latitude = 40.681608, Longitude = -3.620753, Year = "2007", LicensePlate = "M-XXX1234", VIN = "SPDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/BMW-Z4-2007-small.jpg" });

            return await Task.Run(() => vehicles);
        }

        public async Task<Guid> CreateVehicle(string urlPrefix, string tenantId, Vehicle vehicleToCreate)
        {
            return await Task.Run(() => new Guid());
        }


        public async Task<Guid> UpdateVehicle(string urlPrefix, string tenantId, Vehicle vehicleToUpdate)
        {
            return await Task.Run(() => new Guid());
        }

    }
}
