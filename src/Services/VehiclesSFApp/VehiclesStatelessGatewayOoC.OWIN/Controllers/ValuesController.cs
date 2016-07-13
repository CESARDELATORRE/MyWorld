using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
//using Vehicles.Domain.ActorContracts;
//using Vehicles.Domain.Model;

namespace VehiclesStatelessGatewayOoC.OWIN.Controllers
{
    public class ValuesController : ApiController
    {

        //[HttpGet]
        //[Route("api/values/{vehicleId:Guid}")]
        //public async Task<Vehicle> GetVehicle(Guid vehicleId)
        //{
        //    //Guid vehicleId = new Guid("cc164441-9a44-c0c1-208b-08d3a5f9941e");
        //    ActorId actorId = new ActorId(vehicleId);
        //    //ActorId actorId = ActorId.CreateRandom();

        //    // Create actor proxy and send the request
        //    IVehicleActor actorProxy = ActorProxy.Create<IVehicleActor>(actorId, "VehiclesSFApp", "VehicleActorService");

        //    //Get current data for this vehicle-actor
        //    Vehicle vehicle = await actorProxy.GetVehicleAsync();

        //    return null;
        //}

        [HttpGet]
        [Route("api/values/")]
        public string GetCheers()
        {
            return "Howdy";
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
