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
        Task<IList<Vehicle>> GetAllVehiclesFromTenant(string tenantId);
    }

}
