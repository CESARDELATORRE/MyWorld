using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Vehicles.Domain.Model;

namespace Vehicles.Domain.ServiceContracts
{ 
    public interface IVehiclesStatefulService : IService
    {
        Task<IList<Vehicle>> GetTenantVehiclesAsync(string tenantIdParam);

        Task<IList<Vehicle>> GetVehiclesInAreaAsync(double topLatitude, double leftLongitude, double bottomLatitude, double rightLongitude);
        Task<IList<Vehicle>> GetVehiclesInAreaByTenantAsync(string tenantId, double topLatitude, double leftLongitude, double bottomLatitude, double rightLongitude);

        Task<string> GetCounter();
    }

}
