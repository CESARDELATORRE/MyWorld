using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MyWorld.Client.Core.Model
{

    public class MyWorldRoot
    {
        [JsonProperty("tenantID")]
        public string TenantID { get; set; }
        
        public List<Item> Items { get; set; }

    }
}
