using System;
using System.Collections.Generic;
using System.Web.Http;

using Microsoft.ServiceFabric.Services;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Remoting.Client;

using VehiclesStatelessGateway.OWIN;
using Vehicles.Domain.ServiceContracts;

namespace VehiclesStatelessGateway.OWIN.Controllers
{
    public class ValuesController : ApiController
    {
        //private string baseSFStatefulReliableServiceUri;
        private Uri _serviceUriInstance;

        // GET api/values
        //(CDLTLL) -- TODO -- Make Async
        public string Get()
        {
            //(CDLTLL) baseSFStatefulReliableServiceUrl = FabricRuntime.GetActivationContext().ApplicationName + "/VehiclesStatefulService/";

            string serviceUri = "fabric:/VehiclesSFApp/VehiclesStatefulService";
            _serviceUriInstance = new Uri(serviceUri);

            long partitionKeyId = 212300302122303033;  //Any valid GeoQuad would work
            ServicePartitionKey servicePartitionKey = new ServicePartitionKey(partitionKeyId);

            //(CDLTLL - Check this out)
            //When the services lives inside the same app, you can access the service instance something like:
            //public static MyServices.Interfaces.IMyStatefulService GetMyStatefulService()
            //{
            //    var proxyLocation = new ServiceUriBuilder("MyStatefulService");
            //    var partition = new ServicePartitionKey(1); //provide the partitionKey for stateful services. for stateless services, you can just comment this out
            //    return ServiceProxy.Create<MyServices.Interfaces.IMyStatefulService>(proxyLocation.ToUri(), partition);
            //}


            IVehiclesStatefulService serviceClient = 
                                            ServiceProxy.Create<IVehiclesStatefulService>(_serviceUriInstance, servicePartitionKey);

            //(CDLTLL) Find out the Partition ID
            //ResolvedServicePartition resolvedPartition = await this.servicePartitionResolver.ResolveAsync(serviceUriInstance, servicePartitionKey, cancelRequest);
            //ResolvedServiceEndpoint ep = resolvedPartition.GetEndpoint();

            //Calling the method in the Stateful service that will be executing in a particular partition/node
            string counter = serviceClient.GetCounter().Result;

            return "Current Counter is: " + counter;

            //return "TBD";
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}
