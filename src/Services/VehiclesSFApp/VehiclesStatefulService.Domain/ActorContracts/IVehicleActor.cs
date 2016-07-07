using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

using Vehicles.Domain.Model;

namespace Vehicles.Domain.ActorContracts
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IVehicleActor : IActor
    {
        Task<Vehicle> GetVehicleAsync();
        Task SetVehicleAsync(Vehicle vehicle);

        Task<int> GetCountAsync();
        Task SetCountAsync(int count);
    }
}
