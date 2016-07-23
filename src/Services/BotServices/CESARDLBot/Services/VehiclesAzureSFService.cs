using MyWorld.Client.Core.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;
using System.Net.Http.Headers;

//using MyWorld.Client.Core.Helpers;


namespace MyWorld.Client.Core.Services
{
    public class VehiclesAzureSFService : BaseRequest, IVehiclesService
    {
        public VehiclesAzureSFService()
            : base()
        {
            
        }

        public async Task<IList<Vehicle>> GetVehiclesInArea(string urlPrefix, string tenantId, double topLatitude, double leftLongitude, double bottomLatitude, double rightLongitude)
        {
            //Sample: http://localhost:8740/api/vehicles/?tenantId=CDLTLL&topLatitude=47.6670476481776&leftLongitude=-122.169899876643&bottomLatitude=47.6130883518224&rightLongitude=-122.089816123357

            string url = $"{urlPrefix}api/vehicles/?tenantid={tenantId}&topLatitude={topLatitude}&leftLongitude={leftLongitude}&bottomLatitude={bottomLatitude}&rightLongitude={rightLongitude}";
            List<Vehicle> vehicles = await GetAsync<List<Vehicle>>(url);
            return vehicles;

        }

        public async Task<IList<Vehicle>> GetAllVehiclesFromTenant(string urlPrefix, string tenantId)
        {
            string url;
            if (tenantId != "MASTER")
                url = $"{urlPrefix}api/vehicles/?tenantid={tenantId}";
            else
                url = $"{urlPrefix}api/vehicles/";

            List<Vehicle> vehicles = await GetAsync<List<Vehicle>>(url);
            //return vehicles;

            //TEST - CREATE & UPDATE a Vehicle
            //Vehicle vehicle = new Vehicle { TenantId = tenantId, Make = "BMW", Model = "X6", Latitude = 47.612040, Longitude = -122.332359, Year = "2015", LicensePlate = "BXW6940", VIN = "QWERTYDGRT$^HTFSE#", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/BMW-X6-2015-small.jpg" };

            //Guid vehicleIdCreated = await CreateVehicle(urlPrefix, tenantId, vehicle);
            //vehicle.Model = "X5";
            //vehicle.Id = vehicleIdCreated;
            //Guid vehicleIdUpdated = await UpdateVehicle(urlPrefix, tenantId, vehicle);

            return vehicles;
        }

        public async Task<Guid> CreateVehicle(string urlPrefix, string tenantId, Vehicle vehicleToCreate)
        {
            string url = $"{urlPrefix}api/vehicles/create";

            Guid createdVehicleId = await PostAsync<Guid, Vehicle>(url, vehicleToCreate);
            return createdVehicleId;
        }


        public async Task<Guid> UpdateVehicle(string urlPrefix, string tenantId, Vehicle vehicleToUpdate)
        {
            string url = $"{urlPrefix}api/vehicles/update";

            Guid updatedVehicleId = await PutAsync<Guid, Vehicle>(url, vehicleToUpdate);
            return updatedVehicleId;
        }

    }
}
