using MyWorld.Client.Core.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;

namespace MyWorld.Client.Core.Services
{

    public interface IMyWorldRootService
    {
        MyWorldRoot GetMyWorld();
    }

    public class MyWorldRootMockService : IMyWorldRootService
    {
        public MyWorldRoot GetMyWorld()
        {
            MyWorldRoot myWorldData = new MyWorldRoot();
            myWorldData.TenantID = "CDLTLL";

            myWorldData.Items = new List<Item>();

            myWorldData.Items.Add(new Item { Id = 100, Name = "Marta" });
            myWorldData.Items.Add(new Item { Id = 101, Name = "Erika" });
            myWorldData.Items.Add(new Item { Id = 102, Name = "Adrian" });
            myWorldData.Items.Add(new Item { Id = 103, Name = "Chevy Tahoe" });
            myWorldData.Items.Add(new Item { Id = 104, Name = "Chevy Camaro" });

            return myWorldData;
        }
    }
}
