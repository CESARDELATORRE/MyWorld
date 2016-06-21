using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MyWorld.Client.Core.Model
{

    public class MyWorldRoot
    {
        [JsonProperty("tenantid")]
        public string TenantID { get; set; }
        [JsonProperty("people")]
        public List<Person> People { get; set; }
        [JsonProperty("vehicles")]
        public IList<Vehicle> Vehicles { get; set; }
        [JsonProperty("techitems")]
        public List<TechItem> TechItems { get; set; }
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }
}
