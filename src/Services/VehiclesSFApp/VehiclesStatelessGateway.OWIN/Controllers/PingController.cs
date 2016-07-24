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
    public class PingController : ApiController
    {
        [HttpGet]
        [Route("api/ping/")]
        public string Get()
        {
            return "Howdy!";
        }

    }
}
