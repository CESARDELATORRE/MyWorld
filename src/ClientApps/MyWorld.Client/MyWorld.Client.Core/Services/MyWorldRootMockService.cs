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
        MyWorldRoot GetMyWorldData();
    }

    public class MyWorldRootMockService : IMyWorldRootService
    {
        public MyWorldRoot GetMyWorldData()
        {
            MyWorldRoot myWorldData = new MyWorldRoot();
            myWorldData.TenantID = "CDLTLL";

            //People - Mock data
            myWorldData.People = new List<Person>();
            myWorldData.People.Add(new Person { Id = 1, FirstName = "Marta", LastName = "Blazquez" });
            myWorldData.People.Add(new Person { Id = 2, FirstName = "Erika", LastName = "De la Torre" });
            myWorldData.People.Add(new Person { Id = 3, FirstName = "Adrian", LastName = "De la Torre" });
            myWorldData.People.Add(new Person { Id = 4, FirstName = "David", LastName = "Rodriguez" });
            myWorldData.People.Add(new Person { Id = 5, FirstName = "Jaime", LastName = "Perena" });
            myWorldData.People.Add(new Person { Id = 6, FirstName = "Vicente", LastName = "Vazquez" });

            //Vehicles - Mock data
            myWorldData.Vehicles = new List<Vehicle>();
            myWorldData.Vehicles.Add(new Vehicle { Id = 100, Make = "Chevrolet", Model = "Camaro", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17" });
            myWorldData.Vehicles.Add(new Vehicle { Id = 101, Make = "Chevrolet", Model = "Tahoe",  LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17" });

            //Tech Items - Mock Data
            myWorldData.TechItems = new List<TechItem>();
            myWorldData.TechItems.Add(new TechItem { Id = 1001, Name = "Surface Book", PurchaseDate = new DateTime(2015, 10, 30) });
            myWorldData.TechItems.Add(new TechItem { Id = 1002, Name = "HP MicroServer", PurchaseDate = new DateTime(2012, 03, 15) });
            myWorldData.TechItems.Add(new TechItem { Id = 1001, Name = "NetDuma Router", PurchaseDate = new DateTime(2016, 03, 25) });

            //Generic Items
            myWorldData.Items = new List<Item>();
            myWorldData.Items.Add(new Item { Id = 10000, Name = "Thing" });
            myWorldData.Items.Add(new Item { Id = 10001, Name = "Unrelated thing" });
            myWorldData.Items.Add(new Item { Id = 10002, Name = "Yet another thing" });
            myWorldData.Items.Add(new Item { Id = 10003, Name = "Pants" });
            myWorldData.Items.Add(new Item { Id = 10004, Name = "Whatever" });

            return myWorldData;
        }
    }
}
