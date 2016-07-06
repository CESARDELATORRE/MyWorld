using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

using System.Fabric;
using System.Fabric.Query;
using Microsoft.ServiceFabric;
using Microsoft.ServiceFabric.Services;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

using VehiclesStatelessGateway.OWIN;
using Vehicles.Domain.Model;
using Vehicles.Domain.ServiceContracts;
using GeographicLib.GeoCoordinatePortable;
using GeographicLib.Tiles;

namespace VehiclesStatelessGateway.OWIN.Controllers
{
    public class VehiclesController : ApiController
    {
        private static FabricClient _fabricClient = new FabricClient();
        private Uri _serviceUriInstance;

        [HttpGet]
        [Route("api/vehicles/")]
        public async Task<IEnumerable<Vehicle>> GetVehiclesInArea(string tenantId, double topLatitude, double leftLongitude, double bottomLatitude, double rightLongitude)
        {
            string serviceUri = "fabric:/VehiclesSFApp/VehiclesStatefulService";
            _serviceUriInstance = new Uri(serviceUri);


            long topLeftGeoQuad = GeoTileTool.GeoCoordinateToInt64QuadKey(topLatitude, leftLongitude);
            long bottomDownGeoQuad = GeoTileTool.GeoCoordinateToInt64QuadKey(bottomLatitude, rightLongitude);

            List<Vehicle> aggregatedVehiclesList = new List<Vehicle>();

            ServicePartitionList partitions = await _fabricClient.QueryManager.GetPartitionListAsync(_serviceUriInstance);

            //Do for each partition with partition-key (QuadKey) is greater than top-lef-quadkey or smaller than down-bottom-quadkey
            foreach (Partition p in partitions)
            {
                long lowPartitionKey = (p.PartitionInformation as Int64RangePartitionInformation).LowKey;
                long highPartitionKey = (p.PartitionInformation as Int64RangePartitionInformation).HighKey;

                //If partition-keys are within our boundary, query vehicles in partition
                if(topLeftGeoQuad <= highPartitionKey && bottomDownGeoQuad >= lowPartitionKey)
                {
                    IVehiclesStatefulService vehiclesServiceClient =
                        ServiceProxy.Create<IVehiclesStatefulService>(_serviceUriInstance, new ServicePartitionKey(lowPartitionKey));
                    
                    //Async call aggregating the results
                    IList<Vehicle> currentPartitionResult = await vehiclesServiceClient.GetVehiclesInAreaAsync(tenantId, topLatitude, leftLongitude, bottomLatitude, rightLongitude);
                    if (currentPartitionResult.Count > 0)
                    {
                        //Aggregate List from current partition to our global result
                        aggregatedVehiclesList.AddRange(currentPartitionResult);
                    }
                }
            }

            //Show List of vehicles to return
            foreach (Vehicle vehicle in aggregatedVehiclesList)
                System.Diagnostics.Debug.WriteLine($"Returning Existing Car: {vehicle.FullTitle} -- Vehicle-Id: {vehicle.Id} -- TenantId: {vehicle.TenantId} -- Orig-Latitude {vehicle.Latitude}, Orig-Longitude {vehicle.Longitude}, GeoQuadKey {vehicle.GeoQuadKey}, Geohash {vehicle.Geohash}");

            //Return the aggregated list from all the partitions
            return aggregatedVehiclesList;
        }

        [HttpGet]
        [Route("api/vehicles/")]
        public async Task<IEnumerable<Vehicle>> GetTenantVehicles(string tenantId)
        {
            //TO DO: Move Uri construction to the Constructor
            string serviceUri = "fabric:/VehiclesSFApp/VehiclesStatefulService";
            _serviceUriInstance = new Uri(serviceUri);

            List<Vehicle> aggregatedVehiclesList = new List<Vehicle>();

            ServicePartitionList partitions = await _fabricClient.QueryManager.GetPartitionListAsync(_serviceUriInstance);

            foreach (Partition p in partitions)
            {
                long minKey = (p.PartitionInformation as Int64RangePartitionInformation).LowKey;
                IVehiclesStatefulService vehiclesServiceClient = 
                    ServiceProxy.Create<IVehiclesStatefulService>(_serviceUriInstance, new ServicePartitionKey(minKey));

                //Async call aggregating the results
                IList<Vehicle> currentPartitionResult = await vehiclesServiceClient.GetTenantVehiclesAsync(tenantId);
                if (currentPartitionResult.Count > 0)
                {
                    //Aggregate List from current partition to our global result
                    aggregatedVehiclesList.AddRange(currentPartitionResult);
                }
            }
            //Return the aggregated list from all the partitions
            return aggregatedVehiclesList;

        }

        
    }
}
