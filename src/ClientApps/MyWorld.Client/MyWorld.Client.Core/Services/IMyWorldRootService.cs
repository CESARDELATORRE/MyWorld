using MyWorld.Client.Core.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;

namespace MyWorld.Client.Core.Services
{

    public interface IMyWorldRootService
    {
        Task<MyWorldRoot> GetMyWorldData(string urlPrefix, string tenantId);
    }

}
