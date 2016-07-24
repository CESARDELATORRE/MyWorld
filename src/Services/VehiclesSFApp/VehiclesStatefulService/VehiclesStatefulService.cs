using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Actors;

using Vehicles.Domain.Model;
using Vehicles.Domain.ServiceContracts;
using Vehicles.Domain.ActorContracts;

using GeographicLib.GeoCoordinatePortable;
//using System.Device.Location;
using GeographicLib;
using GeographicLib.Tiles;
using Microsoft.ServiceFabric.Actors.Client;

namespace VehiclesStatefulService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class VehiclesStatefulService : StatefulService, IVehiclesStatefulService
    {
        ServicePartitionKind _ServicePartitionKind;

        //Partition High/Low Keys 
        long _currentInt64RangePartitionHighKey;
        long _currentInt64RangePartitionLowKey;

        public VehiclesStatefulService(StatefulServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, ServiceFabric.Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see http://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            //(CDLTLL) Return my service listeners
            return new List<ServiceReplicaListener>()
            {
                //ServiceFabric.Remoting Listener
                new ServiceReplicaListener(
                    (context) =>
                        this.CreateServiceRemotingListener(context))
            };

        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {

            //(CDLTLL) Find out the HighKey and LowKey values for the this service's partition (current)
            //var fabricClient = new FabricClient(new FabricClientSettings() { HealthReportSendInterval = TimeSpan.FromSeconds(0) });
            var fabricClient = new FabricClient();
            string serviceUri = "fabric:/VehiclesSFApp/VehiclesStatefulService";
            Uri serviceUriInstance = new Uri(serviceUri);

            System.Fabric.Query.ServicePartitionList partitionList =
                                            await fabricClient.QueryManager.GetPartitionListAsync(serviceUriInstance);

            foreach (var partition in partitionList)
            {
                if (partition.PartitionInformation.Id == this.Context.PartitionId)
                {
                    //ServiceEventSource.Current.ServiceMessage(this, "************************* PARTITION INFO *********************************");

                    if (partition.PartitionInformation.Kind == ServicePartitionKind.Int64Range)
                    {
                        Int64RangePartitionInformation currentInt64RangePartition =
                                                    (Int64RangePartitionInformation)partition.PartitionInformation;

                        //Saving Partition's kind (Int64Range/Named/Singleton/Invalid )
                        _ServicePartitionKind = partition.PartitionInformation.Kind;

                        //Saving High/Low Keys and related GeoLocation info
                        //PartitionKeys are used as GeoQuads
                        _currentInt64RangePartitionHighKey = currentInt64RangePartition.HighKey;
                        _currentInt64RangePartitionLowKey = currentInt64RangePartition.LowKey;

                        //ServiceEventSource.Current.ServiceMessage(this, "Partition ID: {0} -- Serv-Partition Kind: {1} -- HighKey: {2} -- LowKey: {3}",
                        //                                                this.Context.PartitionId,
                        //                                                _ServicePartitionKind, 
                        //                                                _currentInt64RangePartitionHighKey,
                        //                                                _currentInt64RangePartitionLowKey
                        //                                                );
                    }
                    else
                    {
                        ServiceEventSource.Current.ServiceMessage(this, "Partition is not Int64Range");
                    }
                }
            }

            //Create Vehicles through VehicleActors which will update the Stateful services and it Reliable Dictionaries afterwards 

            Vehicle vehicle1 = new Vehicle { Alias = "Red Daemon", TenantId = "CDLTLL", Make = "Chevrolet", Model = "Camaro", Latitude = 47.644958, Longitude = -122.131077, Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Camaro-RS-2012-small.jpg" };
            vehicle1.GenerateNewIdentity();
            long vehicle1PartitionKey = vehicle1.GetPartitionKey();
            if ((vehicle1PartitionKey > _currentInt64RangePartitionLowKey) && (vehicle1PartitionKey < _currentInt64RangePartitionHighKey))
            {
                IVehicleActor actorProxy1 = ActorProxy.Create<IVehicleActor>(new ActorId(vehicle1.Id), "VehiclesSFApp", "VehicleActorService");
                await actorProxy1.SetVehicleAsync(vehicle1);

                ServiceEventSource.Current.ServiceMessage(this, "Added Car: {0} with Id: {1} TenantId: {2}", vehicle1.FullTitle, vehicle1.Id, vehicle1.TenantId);
            }

            Vehicle vehicle2 = new Vehicle { Alias = "The Beast", TenantId = "CDLTLL", Make = "Chevrolet", Model = "Tahoe", Latitude = 47.661542, Longitude = -122.131231, Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Tahoe-Z71-2015-small.jpg" };
            vehicle2.GenerateNewIdentity();
            long vehicle2PartitionKey = vehicle2.GetPartitionKey();
            if ((vehicle2PartitionKey > _currentInt64RangePartitionLowKey) && (vehicle2PartitionKey < _currentInt64RangePartitionHighKey))
            {
                IVehicleActor actorProxy2 = ActorProxy.Create<IVehicleActor>(new ActorId(vehicle2.Id), "VehiclesSFApp", "VehicleActorService");
                await actorProxy2.SetVehicleAsync(vehicle2);

                ServiceEventSource.Current.ServiceMessage(this, "Added Car: {0} with Id: {1} TenantId: {2}", vehicle2.FullTitle, vehicle2.Id, vehicle2.TenantId);
            }

            Vehicle vehicle3 = new Vehicle { Alias = "Kicking Horse", TenantId = "TENANT2", Make = "Ford", Model = "Mustang", Latitude = 47.654177, Longitude = -122.132442, Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Ford-Mustang-2015-small.jpg" };
            vehicle3.GenerateNewIdentity();
            long vehicle3PartitionKey = vehicle3.GetPartitionKey();
            if ((vehicle3PartitionKey > _currentInt64RangePartitionLowKey) && (vehicle3PartitionKey < _currentInt64RangePartitionHighKey))
            {
                IVehicleActor actorProxy3 = ActorProxy.Create<IVehicleActor>(new ActorId(vehicle3.Id), "VehiclesSFApp", "VehicleActorService");
                await actorProxy3.SetVehicleAsync(vehicle3);

                ServiceEventSource.Current.ServiceMessage(this, "Added Car: {0} with Id: {1} TenantId: {2}", vehicle3.FullTitle, vehicle3.Id, vehicle3.TenantId);
            }

            Vehicle vehicle4 = new Vehicle { Alias = "The Explorer", TenantId = "TENANT2", Make = "Ford", Model = "Explorer", Latitude = 47.645120, Longitude = -122.138143, Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Ford-Explorer-2016-small.jpg" };
            vehicle4.GenerateNewIdentity();
            long vehicle4PartitionKey = vehicle4.GetPartitionKey();
            if ((vehicle4PartitionKey > _currentInt64RangePartitionLowKey) && (vehicle4PartitionKey < _currentInt64RangePartitionHighKey))
            {
                IVehicleActor actorProxy4 = ActorProxy.Create<IVehicleActor>(new ActorId(vehicle4.Id), "VehiclesSFApp", "VehicleActorService");
                await actorProxy4.SetVehicleAsync(vehicle4);

                ServiceEventSource.Current.ServiceMessage(this, "Added Car: {0} with Id: {1} TenantId: {2}", vehicle4.FullTitle, vehicle4.Id, vehicle4.TenantId);
            }

            Vehicle vehicle5 = new Vehicle { Alias = "The Macchina", TenantId = "CDLTLL", Make = "BMW", Model = "Z4", Latitude = 40.681608, Longitude = -3.620753, Year = "2007", LicensePlate = "M-XXX1234", VIN = "SPDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/BMW-Z4-2007-small.jpg" };
            vehicle5.GenerateNewIdentity();
            long vehicle5PartitionKey = vehicle5.GetPartitionKey();
            if ((vehicle5PartitionKey > _currentInt64RangePartitionLowKey) && (vehicle5PartitionKey < _currentInt64RangePartitionHighKey))
            {
                IVehicleActor actorProxy5 = ActorProxy.Create<IVehicleActor>(new ActorId(vehicle5.Id), "VehiclesSFApp", "VehicleActorService");
                await actorProxy5.SetVehicleAsync(vehicle5);

                ServiceEventSource.Current.ServiceMessage(this, "Added Car: {0} with Id: {1} TenantId: {2}", vehicle5.FullTitle, vehicle5.Id, vehicle5.TenantId);
            }

            ////Single search
            ////var result = await vehiclesPartitionDictionary.TryGetValueAsync(tx, "GUID1");


            ////********************************************************************************************************
            ////Vehicles Reliable-Dictionary Creation and Initialization with sample data depending on the Partition Key-Ranges
            //IReliableDictionary<Guid, Vehicle> vehiclesPartitionDictionary = 
            //                    await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Vehicle>>("VehiclesDictionary");

            //// Sample Data Initialization for this partition
            //using (var tx = this.StateManager.CreateTransaction())
            //{

            //    Vehicle vehicle1 = new Vehicle { Alias = "Red Daemon", TenantId = "CDLTLL", Make = "Chevrolet", Model = "Camaro", Latitude = 47.644958, Longitude = -122.131077, Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Camaro-RS-2012-small.jpg" };
            //    vehicle1.GenerateNewIdentity();
            //    long vehicle1PartitionKey = vehicle1.GetPartitionKey();
            //    if ((vehicle1PartitionKey > _currentInt64RangePartitionLowKey) && (vehicle1PartitionKey < _currentInt64RangePartitionHighKey))
            //    {
            //        await vehiclesPartitionDictionary.AddOrUpdateAsync(tx, vehicle1.Id, vehicle1, (key, value) => vehicle1);

            //        ServiceEventSource.Current.ServiceMessage(this, "Added Car: {0} with Id: {1} TenantId: {2}", vehicle1.FullTitle, vehicle1.Id, vehicle1.TenantId);
            //    }

            //    Vehicle vehicle2 = new Vehicle { Alias = "The Beast", TenantId = "CDLTLL", Make = "Chevrolet", Model = "Tahoe", Latitude = 47.661542, Longitude = -122.131231, Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Tahoe-Z71-2015-small.jpg" };
            //    vehicle2.GenerateNewIdentity();
            //    long vehicle2PartitionKey = vehicle2.GetPartitionKey();
            //    if ((vehicle2PartitionKey > _currentInt64RangePartitionLowKey) && (vehicle2PartitionKey < _currentInt64RangePartitionHighKey))
            //    {
            //        await vehiclesPartitionDictionary.AddOrUpdateAsync(tx, vehicle2.Id, vehicle2, (key, value) => vehicle2);

            //        ServiceEventSource.Current.ServiceMessage(this, "Added Car: {0} with Id: {1} TenantId: {2}", vehicle2.FullTitle, vehicle2.Id, vehicle2.TenantId);
            //    }

            //    Vehicle vehicle3 = new Vehicle { Alias = "Kicking Horse", TenantId = "TENANT2", Make = "Ford", Model = "Mustang", Latitude = 47.654177, Longitude = -122.132442, Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Ford-Mustang-2015-small.jpg" };
            //    vehicle3.GenerateNewIdentity();
            //    long vehicle3PartitionKey = vehicle3.GetPartitionKey();
            //    if ((vehicle3PartitionKey > _currentInt64RangePartitionLowKey) && (vehicle3PartitionKey < _currentInt64RangePartitionHighKey))
            //    {
            //        await vehiclesPartitionDictionary.AddOrUpdateAsync(tx, vehicle3.Id, vehicle3, (key, value) => vehicle3);

            //        ServiceEventSource.Current.ServiceMessage(this, "Added Car: {0} with Id: {1} TenantId: {2}", vehicle3.FullTitle, vehicle3.Id, vehicle3.TenantId);
            //    }

            //    Vehicle vehicle4 = new Vehicle { Alias = "The Explorer", TenantId = "TENANT2", Make = "Ford", Model = "Explorer", Latitude = 47.645120, Longitude = -122.138143, Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Ford-Explorer-2016-small.jpg" };
            //    vehicle4.GenerateNewIdentity();
            //    long vehicle4PartitionKey = vehicle4.GetPartitionKey();
            //    if ((vehicle4PartitionKey > _currentInt64RangePartitionLowKey) && (vehicle4PartitionKey < _currentInt64RangePartitionHighKey))
            //    {
            //        await vehiclesPartitionDictionary.AddOrUpdateAsync(tx, vehicle4.Id, vehicle4, (key, value) => vehicle4);

            //        ServiceEventSource.Current.ServiceMessage(this, "Added Car: {0} with Id: {1} TenantId: {2}", vehicle4.FullTitle, vehicle4.Id, vehicle4.TenantId);
            //    }

            //    Vehicle vehicle5 = new Vehicle { Alias = "The Macchina", TenantId = "CDLTLL", Make = "BMW", Model = "Z4", Latitude = 40.681608, Longitude = -3.620753, Year = "2007", LicensePlate = "M-XXX1234", VIN = "SPDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/BMW-Z4-2007-small.jpg" };
            //    vehicle5.GenerateNewIdentity();
            //    long vehicle5PartitionKey = vehicle5.GetPartitionKey();
            //    if ((vehicle5PartitionKey > _currentInt64RangePartitionLowKey) && (vehicle5PartitionKey < _currentInt64RangePartitionHighKey))
            //    {
            //        await vehiclesPartitionDictionary.AddOrUpdateAsync(tx, vehicle5.Id, vehicle5, (key, value) => vehicle5);

            //        ServiceEventSource.Current.ServiceMessage(this, "Added Car: {0} with Id: {1} TenantId: {2}", vehicle5.FullTitle, vehicle5.Id, vehicle5.TenantId);
            //    } 

            //    ////Single search
            //    ////var result = await vehiclesPartitionDictionary.TryGetValueAsync(tx, "GUID1");

            //    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
            //    // discarded, and nothing is saved to the secondary replicas.
            //    await tx.CommitAsync();
            //}
            //// Query Vehicles in this partition
            //using (var tx = this.StateManager.CreateTransaction())
            //{
            //    //Query all vehicles in partition, if any
            //    long numVehicles = await vehiclesPartitionDictionary.GetCountAsync(tx);
            //    if(numVehicles > 0)
            //    {
            //        //Get IEnumerable and list what's in the Vehicles Dictionary
            //        Microsoft.ServiceFabric.Data.IAsyncEnumerable<KeyValuePair<Guid, Vehicle>> vehiclesPartitionEnumerable =
            //                                    await vehiclesPartitionDictionary.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

            //        using (var sequenceEnum = vehiclesPartitionEnumerable.GetAsyncEnumerator())
            //        {
            //            while (await sequenceEnum.MoveNextAsync(cancellationToken))
            //            {
            //                Vehicle vehicle = sequenceEnum.Current.Value;

            //                Guid currentPartition = this.Context.PartitionId;
            //                double quadKeyLatitude;
            //                double quadKeyLongitude;
            //                int quadKeyLevelOfDetail;

            //                GeoTileTool.Int64QuadKeyToGeoCoordinate(vehicle.GeoQuadKey, out quadKeyLatitude, out quadKeyLongitude, out quadKeyLevelOfDetail);

            //                double[] GeohashCoordinate;
            //                GeohashCoordinate = GeohashTool.Decode(vehicle.Geohash);

            //                ServiceEventSource.Current.ServiceMessage(this, "Existing Car: {0} -- Partition {1} -- Vehicle-Id: {2} -- TenantId: {3} -- Orig-Latitude {4}, Orig-Longitude {5}, GeoQuadKey {6}, Geohash {7} -- QuadKey-Latitude {8}, QuadKey-Longitude {9} -- Geohash-Latitude {10}, Geohash-Longitude {11}", vehicle.FullTitle, currentPartition.ToString(), vehicle.Id, vehicle.TenantId, vehicle.Latitude, vehicle.Longitude, vehicle.GeoQuadKey, vehicle.Geohash, quadKeyLatitude, quadKeyLongitude, GeohashCoordinate[0], GeohashCoordinate[1]);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //ServiceEventSource.Current.ServiceMessage(this, "No Vehicles in Partition {0}) ", this.Context.PartitionId);
            //        //With Max/Min PArtition Coordinates
            //        //ServiceEventSource.Current.ServiceMessage(this, "No Vehicles in Partition {1} -- PartitionCoordinateHigh ({2},{3}) -- PartitionCoordinateLow ({4},{5}) ", this.Context.PartitionId.ToString(), _currentPartitionHighGeoCoordinate.Latitude, _currentPartitionHighGeoCoordinate.Longitude, _currentPartitionLowGeoCoordinate.Latitude, _currentPartitionLowGeoCoordinate.Longitude);
            //    }

            //    //Single search
            //    //var result = await vehiclesPartitionDictionary.TryGetValueAsync(tx, "GUID1");

            //    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
            //    // discarded, and nothing is saved to the secondary replicas.
            //    await tx.CommitAsync();

            //}

            //(CDLTLL) Test Query of vehicles by TenantID
            //var resultList = await GetTenantVehiclesAsync("CDLTLL");
            //IList<Vehicle> VehiclesListTest = resultList;

            // *********************
            // *********************
            // COUNTER stuff - To be deleted ---
            //Test code with a simple Counter in a Dictionary

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");
            using (var tx = this.StateManager.CreateTransaction())
            {
                var result = await myDictionary.TryGetValueAsync(tx, "Counter");
                await myDictionary.AddOrUpdateAsync(tx, "Counter", 12, (key, value) => 121212);
                await tx.CommitAsync();
            }
        }

        public async Task<string> GetCounter()
        {
            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");
            string retCounter;
            
            using (var tx = this.StateManager.CreateTransaction())
            {
                var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                //await tx.CommitAsync();

                retCounter = result.Value.ToString();
            }           
            return retCounter;
        }
        
        public async Task<IList<Vehicle>> GetTenantVehiclesAsync(string tenantIdParam)
        {
            ServiceEventSource.Current.ServiceMessage(this, "Called GetTenantVehiclesAsync in STATEFUL SERVICE to return collection of Vehicles in partition {0} from TetantId {1}", this.Context.PartitionId, tenantIdParam);

            var queryResult = await QueryReliableDictionary<Vehicle>(this.StateManager, "VehiclesDictionary", vehicle => !string.IsNullOrWhiteSpace(vehicle.TenantId) && (vehicle.TenantId == tenantIdParam));
                                                                                                                                                                        //vehicle.TenantId.IndexOf(tenantIdParam, StringComparison.OrdinalIgnoreCase) >= 0)

            return queryResult;
        }

        //(CDLTLL) This method should be substituted by a paginated method (20 per page or so). 
        //No method should return ALL the vehicles as potentially there could be thousands or millions...
        public async Task<IList<Vehicle>> GetAllVehiclesAsync()
        {
            ServiceEventSource.Current.ServiceMessage(this, "Called GetAllVehiclesAsync in STATEFUL SERVICE to return collection of Vehicles in partition {0}", this.Context.PartitionId);

            var queryResult = await QueryReliableDictionary<Vehicle>(this.StateManager, "VehiclesDictionary", null);

            return queryResult;
        }

        public async Task<IList<Vehicle>> GetVehiclesInAreaAsync(double topLatitude, double leftLongitude, double bottomLatitude, double rightLongitude)
        {
            ServiceEventSource.Current.ServiceMessage(this, "Called GetVehiclesInAreaAsync in STATEFUL SERVICE to return collection of Vehicles in partition {0} for all Tenants", this.Context.PartitionId);

            var queryResult = await QueryReliableDictionary<Vehicle>(this.StateManager, "VehiclesDictionary",
                                                                     vehicle => vehicle.Latitude < topLatitude &&
                                                                                vehicle.Latitude > bottomLatitude &&
                                                                                vehicle.Longitude > leftLongitude &&
                                                                                vehicle.Longitude < rightLongitude
                                                                    );

            return queryResult;
        }

        public async Task<IList<Vehicle>> GetVehiclesInAreaByTenantAsync(string tenantIdParam, double topLatitude, double leftLongitude, double bottomLatitude, double rightLongitude)
        {
            ServiceEventSource.Current.ServiceMessage(this, "Called GetVehiclesInAreaByTenantAsync in STATEFUL SERVICE to return collection of Vehicles in partition {0} from TetantId {1}", this.Context.PartitionId, tenantIdParam);

            var queryResult = await QueryReliableDictionary<Vehicle>(this.StateManager, "VehiclesDictionary",
                                                                     vehicle => !string.IsNullOrWhiteSpace(vehicle.TenantId) &&
                                                                                vehicle.TenantId == tenantIdParam &&  //(vehicle.TenantId.IndexOf(tenantIdParam, StringComparison.OrdinalIgnoreCase) >= 0)
                                                                                vehicle.Latitude < topLatitude &&
                                                                                vehicle.Latitude > bottomLatitude &&
                                                                                vehicle.Longitude > leftLongitude &&
                                                                                vehicle.Longitude < rightLongitude
                                                                    );
            //Just for checking/Debug queried values
            //foreach (Vehicle vehicle in queryResult)
            //{
            //    ServiceEventSource.Current.ServiceMessage(this, "Returning Existing Car: {0} -- Vehicle-Id: {1} -- TenantId: {2} -- Orig-Latitude {3}, Orig-Longitude {4}, GeoQuadKey {5}, Geohash {6}", vehicle.FullTitle, vehicle.Id, vehicle.TenantId, vehicle.Latitude, vehicle.Longitude, vehicle.GeoQuadKey, vehicle.Geohash);
            //}

            return queryResult;
        }

        //Generic query method based on a Filter, returning a IList<T>
        public static async Task<IList<T>> QueryReliableDictionary<T>(Microsoft.ServiceFabric.Data.IReliableStateManager stateManager,
                                                                                          string reliableDictionaryName,
                                                                                          Func<T, bool> filter)
        {
            var result = new List<T>();

            IReliableDictionary<Guid, T> reliableDictionary =
                await stateManager.GetOrAddAsync<IReliableDictionary<Guid, T>>(reliableDictionaryName);

            using (var tx = stateManager.CreateTransaction())
            {
                Microsoft.ServiceFabric.Data.IAsyncEnumerable<KeyValuePair<Guid, T>> asyncEnumerable =
                            await reliableDictionary.CreateEnumerableAsync(tx);

                using (Microsoft.ServiceFabric.Data.IAsyncEnumerator<KeyValuePair<Guid, T>> asyncEnumerator = asyncEnumerable.GetAsyncEnumerator())
                {
                    while (await asyncEnumerator.MoveNextAsync(CancellationToken.None))
                    {
                        bool addToResult = false;
                        if (filter == null)
                        {
                            addToResult = true;
                        }
                        else
                        {
                            if (filter(asyncEnumerator.Current.Value))
                                addToResult = true;
                        }
                        if(addToResult)
                            result.Add(asyncEnumerator.Current.Value);
                    }
                }
            }
            return result;
        }

        //Generic query method returning a IList<KeyValuePair<Guid, T>>
        public static async Task<IList<KeyValuePair<Guid, T>>> QueryReliableDictionaryKeyValuePairList<T>(Microsoft.ServiceFabric.Data.IReliableStateManager stateManager, 
                                                                                                          string reliableDictionaryName, 
                                                                                                          Func<T, bool> filter)
        {
            var result = new List<KeyValuePair<Guid, T>>();

            IReliableDictionary<Guid, T> reliableDictionary =
                await stateManager.GetOrAddAsync<IReliableDictionary<Guid, T>>(reliableDictionaryName);

            using (var tx = stateManager.CreateTransaction())
            {
                Microsoft.ServiceFabric.Data.IAsyncEnumerable<KeyValuePair<Guid, T>> asyncEnumerable = 
                            await reliableDictionary.CreateEnumerableAsync(tx);

                using (Microsoft.ServiceFabric.Data.IAsyncEnumerator<KeyValuePair<Guid, T>> asyncEnumerator = asyncEnumerable.GetAsyncEnumerator())
                {
                    while (await asyncEnumerator.MoveNextAsync(CancellationToken.None))
                    {
                        if (filter(asyncEnumerator.Current.Value))
                            result.Add(asyncEnumerator.Current);
                    }
                }
            }
            return result;
        }

        public async Task<bool> AddOrUpdateVehicleAsync(Vehicle vehicle)
        {
            //Get the Vehicles Dictionaryin the current Partition
            IReliableDictionary<Guid, Vehicle> vehiclesPartitionDictionary =
                                await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Vehicle>>("VehiclesDictionary");

            // Sample Data Initialization for this partition
            using (var tx = this.StateManager.CreateTransaction())
            {
                await vehiclesPartitionDictionary.AddOrUpdateAsync(tx, vehicle.Id, vehicle, (key, value) => vehicle);
                await tx.CommitAsync();

                long vehiclePartitionKey = vehicle.GetPartitionKey();
                ServiceEventSource.Current.ServiceMessage(this, "Added/Updated Car: {0} with Id: {1} -- TenantId: {2} -- PartitionID: {3}", vehicle.FullTitle, vehicle.Id, vehicle.TenantId, vehiclePartitionKey);
            }

            return true;
        }


        //(CDLTLL) Alternative explicit code for querying:
        //public async Task<IList<Vehicle>> GetTenantVehiclesAsync(string tenantIdParam)
        //{
        //    //Get the Vehicles Dictionary
        //    IReliableDictionary<Guid, Vehicle> vehiclesPartitionDictionary =
        //        await this.StateManager.GetOrAddAsync<IReliableDictionary<Guid, Vehicle>>("VehiclesDictionary");
        //    IList<Vehicle> tenantVehiclesList = new List<Vehicle>();

        //    using (var tx = this.StateManager.CreateTransaction())
        //    {
        //        ServiceEventSource.Current.ServiceMessage(this, "Generating vehicle list for {0} items", await vehiclesPartitionDictionary.GetCountAsync(tx));

        //        Microsoft.ServiceFabric.Data.IAsyncEnumerable<KeyValuePair<Guid, Vehicle>> vehiclesPartitionEnumerable =
        //                    await vehiclesPartitionDictionary.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

        //        using (var sequenceEnum = vehiclesPartitionEnumerable.GetAsyncEnumerator())
        //        {
        //            while (await sequenceEnum.MoveNextAsync(CancellationToken.None))
        //            {
        //                Vehicle vehicle = sequenceEnum.Current.Value;
        //                Guid currentPartition = this.Context.PartitionId;
        //                if (sequenceEnum.Current.Value.TenantId == tenantId)
        //                {
        //                    tenantVehiclesList.Add(sequenceEnum.Current.Value);
        //                }
        //            }
        //        }
        //    }
        //    return tenantVehiclesList;
        //}

        //--- LINQ Samples (Still not supported by Reliable Dictioneries) ---

        //var vehicles = from vehicle in vehiclesPartitionEnumerable
        //               where vehicle.Value.TenantId == tenantId
        //               select vehicle;

        //var query = from word in words
        //            group word.ToUpper() by word.Length into gr
        //            orderby gr.Key
        //            select new { Length = gr.Key, Words = gr };


        //IReliableDictionary<int, Customer> myDictionary =
        //   await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Customer>>("mydictionary");
        //return from item in myDictionary
        //       where item.Value.Name.StartsWith("A")
        //       orderby item.Key
        //       select new ResultView(...);


    }
}
