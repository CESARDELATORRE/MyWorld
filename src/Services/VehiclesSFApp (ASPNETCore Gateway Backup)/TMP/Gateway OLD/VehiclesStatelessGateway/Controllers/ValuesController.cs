using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.ServiceFabric.Services;
using Microsoft.ServiceFabric;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Remoting.Client;

//using VehiclesStatefulService.Contracts;

namespace VehiclesStatelessGateway.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private Uri vehiclesStatefulServiceInstance = new Uri(FabricRuntime.GetActivationContext().ApplicationName + "/VehiclesStatefulService");

        private string defaultPartitionID = "8d8f5fc8-8339-4e28-a3c3-57075f8478bb";

        [HttpGet]
        public string Get()
        {
            //////param this.defaultPartitionID ??
            //IVehiclesStatefulService serviceClient = ServiceProxy.Create<IVehiclesStatefulService>(this.vehiclesStatefulServiceInstance);

            ////Calling Stateless service
            ////IMyService helloWorldClient = ServiceProxy.Create<IMyService>(new Uri("fabric:/MyApplication/MyHelloWorldService"));

            //string counter = serviceClient.GetCounter().Result;
            //return counter;

            return "TBD";
        }

        // GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

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
