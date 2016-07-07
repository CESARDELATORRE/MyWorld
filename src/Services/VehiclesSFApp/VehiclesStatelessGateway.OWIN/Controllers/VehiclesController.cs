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

using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

using VehiclesStatelessGateway.OWIN;
using Vehicles.Domain.Model;
using Vehicles.Domain.ServiceContracts;
using GeographicLib.GeoCoordinatePortable;
using GeographicLib.Tiles;

using DummyActor.Interfaces;
using Vehicles.Domain.ActorContracts;

namespace VehiclesStatelessGateway.OWIN.Controllers
{
    public class VehiclesController : ApiController
    {
        private static FabricClient _fabricClient = new FabricClient();
        private static string _vehiclesStatefulServiceUri = "fabric:/VehiclesSFApp/VehiclesStatefulService";

        private Uri _vehiclesStatefulServiceUriInstance = new Uri(_vehiclesStatefulServiceUri);

        [HttpGet]
        [Route("api/vehicles/")]
        public async Task<IEnumerable<Vehicle>> GetVehiclesInArea([FromUri] string tenantId, [FromUri] double topLatitude, [FromUri] double leftLongitude, [FromUri] double bottomLatitude, [FromUri] double rightLongitude)
        {
            ServiceEventSource.Current.Message("Called GetVehiclesInArea in STATELESS GATEWAY service to return collection of Vehicles in a particular Map's area and for Tenant {0}", tenantId);

            long topLeftGeoQuad = GeoTileTool.GeoCoordinateToInt64QuadKey(topLatitude, leftLongitude);
            long bottomDownGeoQuad = GeoTileTool.GeoCoordinateToInt64QuadKey(bottomLatitude, rightLongitude);

            List<Vehicle> aggregatedVehiclesList = new List<Vehicle>();

            ServicePartitionList partitions = await _fabricClient.QueryManager.GetPartitionListAsync(_vehiclesStatefulServiceUriInstance);

            //Do for each partition with partition-key (QuadKey) is greater than top-lef-quadkey or smaller than down-bottom-quadkey
            foreach (Partition p in partitions)
            {
                long lowPartitionKey = (p.PartitionInformation as Int64RangePartitionInformation).LowKey;
                long highPartitionKey = (p.PartitionInformation as Int64RangePartitionInformation).HighKey;

                //If partition-keys are within our boundary, query vehicles in partition
                if(topLeftGeoQuad <= highPartitionKey && bottomDownGeoQuad >= lowPartitionKey)
                {
                    IVehiclesStatefulService vehiclesServiceClient =
                        ServiceProxy.Create<IVehiclesStatefulService>(_vehiclesStatefulServiceUriInstance, new ServicePartitionKey(lowPartitionKey));

                    //Async call aggregating the results
                    IList<Vehicle> currentPartitionResult;
                    if (tenantId == "MASTER")
                        currentPartitionResult = await vehiclesServiceClient.GetVehiclesInAreaAsync(topLatitude, leftLongitude, bottomLatitude, rightLongitude);
                    else
                        currentPartitionResult = await vehiclesServiceClient.GetVehiclesInAreaByTenantAsync(tenantId, topLatitude, leftLongitude, bottomLatitude, rightLongitude);

                    if (currentPartitionResult.Count > 0)
                    {
                        //Aggregate List from current partition to our global result
                        aggregatedVehiclesList.AddRange(currentPartitionResult);
                    }
                }
            }

            //Show List of vehicles to return
            //foreach (Vehicle vehicle in aggregatedVehiclesList)
            //    System.Diagnostics.Debug.WriteLine($"Returning Existing Car: {vehicle.FullTitle} -- Vehicle-Id: {vehicle.Id} -- TenantId: {vehicle.TenantId} -- Orig-Latitude {vehicle.Latitude}, Orig-Longitude {vehicle.Longitude}, GeoQuadKey {vehicle.GeoQuadKey}, Geohash {vehicle.Geohash}");

            //Return the aggregated list from all the partitions
            return aggregatedVehiclesList;
        }

        [HttpGet]
        [Route("api/vehicles/")]
        public async Task<IEnumerable<Vehicle>> GetTenantVehicles([FromUri] string tenantId)
        {
            ServiceEventSource.Current.Message("Called GetVehiclesInArea in STATELESS GATEWAY service to return collection of Vehicles for Tenant {0}", tenantId);

            List<Vehicle> aggregatedVehiclesList = new List<Vehicle>();

            ServicePartitionList partitions = await _fabricClient.QueryManager.GetPartitionListAsync(_vehiclesStatefulServiceUriInstance);

            foreach (Partition p in partitions)
            {
                long minKey = (p.PartitionInformation as Int64RangePartitionInformation).LowKey;
                IVehiclesStatefulService vehiclesServiceClient = 
                    ServiceProxy.Create<IVehiclesStatefulService>(_vehiclesStatefulServiceUriInstance, new ServicePartitionKey(minKey));

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

        [HttpGet]
        [Route("api/vehicles/{vehicleId:Guid}")]
        public async Task<Vehicle> GetVehicle(Guid vehicleId)
        {
            //Guid vehicleId = new Guid("cc164441-9a44-c0c1-208b-08d3a5f9941e");
            ActorId actorId = new ActorId(vehicleId);
            //ActorId actorId = ActorId.CreateRandom();

            // Create actor proxy and send the request
            IVehicleActor actorProxy = ActorProxy.Create<IVehicleActor>(actorId, "VehiclesSFApp", "VehicleActorService");

            //Get current data for this vehicle-actor
            Vehicle vehicle = await actorProxy.GetVehicleAsync();

            //IDummyActor actorProxy = ActorProxy.Create<IDummyActor>(actorId, "VehiclesSFApp", "DummyActorService");
            //int counter = await actorProxy.GetCountAsync();

            return vehicle;
        }

        //// Create actor proxy and send the request
        //IRestockRequestActor restockRequestActor = ActorProxy.Create<IRestockRequestActor>(actorId, this.ApplicationName);
        //await restockRequestActor.AddRestockRequestAsync(request);

            //try
            //{
            //    return customerOrder.GetOrderStatusAsStringAsync();
            //}
            //catch (Exception ex)
            //{
            //    ServiceEventSource.Current.Message("Web Service: Exception {0}: {1}", customerOrder, ex);

            //    throw;
            //}
    }
}
