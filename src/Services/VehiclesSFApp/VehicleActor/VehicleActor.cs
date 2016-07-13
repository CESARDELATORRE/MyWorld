using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;

using Vehicles.Domain.Model;
using Vehicles.Domain.ActorContracts;
using System.Fabric;
using Vehicles.Domain.ServiceContracts;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;

namespace VehicleActor
{
    /// <remarks>
    /// This class represents the Vehicle Actor (Domain Logic plus Data).
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class VehicleActor : Actor, IVehicleActor
    {
        private const string VehicleEntityPropertyName = "VehicleEntity";

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Vehicle-Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see http://aka.ms/servicefabricactorsstateserialization

            return this.StateManager.TryAddStateAsync("count", 25);
        }

        Task<Vehicle> IVehicleActor.GetVehicleAsync()
        {
            return this.StateManager.GetStateAsync<Vehicle>(VehicleEntityPropertyName);
        }

        async Task IVehicleActor.SetVehicleAsync(Vehicle vehicle)
        {
            //Create or Update a Vehicle in its related VehicleActor object
            var resultUpdatingActor = this.StateManager.TryAddStateAsync(VehicleEntityPropertyName, vehicle);

            //Create or Update the same Vehicle into Stateful Services
            //like multiple possible "QueryViews" on Stateful Services using Reliable Collections under the covers.
            //These Stateful services could be multiple "Read/query views" like in CQRS architectures with several "Read-sides"
            FabricClient fabricClient = new FabricClient();
            string vehiclesStatefulServiceUri = "fabric:/VehiclesSFApp/VehiclesStatefulService";
            Uri vehiclesStatefulServiceUriInstance = new Uri(vehiclesStatefulServiceUri);

            long vehiclePartitionKey = vehicle.GetPartitionKey();
            IVehiclesStatefulService vehiclesServiceClient = 
                                    ServiceProxy.Create<IVehiclesStatefulService>(vehiclesStatefulServiceUriInstance, 
                                                                                  new ServicePartitionKey(vehiclePartitionKey));

            bool result = await vehiclesServiceClient.AddOrUpdateVehicleAsync(vehicle);

            return;            
        }


        Task<int> IVehicleActor.GetCountAsync()
        {
            return this.StateManager.GetStateAsync<int>("count");
        }

        Task IVehicleActor.SetCountAsync(int count)
        {
            // Requests are not guaranteed to be processed in order nor at most once.
            // The update function here verifies that the incoming count is greater than the current count to preserve order.
            return this.StateManager.AddOrUpdateStateAsync("count", count, (key, value) => count > value ? count : value);
        }
    }
}
