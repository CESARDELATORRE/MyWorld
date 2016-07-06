using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

//using Microsoft.ServiceFabric.Services;
//using Microsoft.ServiceFabric.Services.Client;
//using Microsoft.ServiceFabric;
//using System.Fabric;
//using Microsoft.ServiceFabric.Services.Remoting.Client;

//using VehiclesStatefulService.Contracts;

namespace VehiclesStatelessGateway.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        //private long partitionKeyId = 1;
        //private string baseSFStatefulReliableServiceUri;
        //private Uri vehiclesStatefulServiceInstance;

        // GET api/values
        [HttpGet]
        public string Get()
        {
            ////(CDLTLL) baseSFStatefulReliableServiceUrl = FabricRuntime.GetActivationContext().ApplicationName + "/VehiclesStatefulService/";
            //baseSFStatefulReliableServiceUri = "fabric:/VehiclesSFApp/VehiclesStatefulService/";

            //string serviceUri = baseSFStatefulReliableServiceUri;
            //vehiclesStatefulServiceInstance = new Uri(serviceUri);

            //ServicePartitionKey servicePartitionKey = new ServicePartitionKey();
            //IVehiclesStatefulService serviceClient =
            //                ServiceProxy.Create<IVehiclesStatefulService>(this.vehiclesStatefulServiceInstance, servicePartitionKey);

            ////Calling the method in the Stateful service that will be executing in a particular partition/node
            //string counter = serviceClient.GetCounter().Result;
            //return "Current Counter is: " + counter;

            return "TBD";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
