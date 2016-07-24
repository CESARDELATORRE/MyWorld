using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;

using MyWorld.Client.Core.Model;
using MyWorld.Client.Core.Services;


namespace MyWorld.Client.Windows.CLI
{
    class Program
    {
        private static string _urlPrefix = ConfigurationManager.AppSettings["AzureVehiclesServiceUrlPrefix"];
        private static IDictionary<string, MenuItem> _menuItems = new Dictionary<string, MenuItem>();

        static void Main(string[] args)
        {
            AddMenuItem("Ping WebAPI service in Service Fabric Cluster", Ping);
            AddMenuItem("Add new vehicles", AddVehicles);
            //AddMenuItem("Update vehicles' coordinates Forward", UpdateCoordinatesForward);
            //AddMenuItem("Update vehicles' coordinates Backward", UpdateCoordinatesBackward);
            AddMenuItem("Quit", "q");

            var selection = String.Empty;

            Header("MyWorld CLI v1.0");
            DisplayMenu();
            selection = GetSelection();

            do
            {
                switch (selection)
                {
                    case "q":
                        Console.WriteLine("Good bye");
                        return;
                    case "m":
                        DisplayMenu();
                        selection = GetSelection();
                        break;
                    default:
                        if (_menuItems.ContainsKey(selection))
                            _menuItems[selection].Run();
                        else
                            Console.WriteLine("Invalid selection");

                        selection = GetSelection();
                        break;
                }

            } while (true);

        }

        static void Ping()
        {
            //var vehicleId = PromptTenantId();
            Console.WriteLine("Pinging MyWorld WebAPI Front-End...");
            VehiclesAzureSFService vehiclesService = new VehiclesAzureSFService();
            var reVal = vehiclesService.PingVehiclesService(_urlPrefix);

            Console.WriteLine("\nResponse: {0}", reVal.Result.ToString());
        }


        static void AddVehicles()
        {
            String tenantId = GetTenantId();

            Console.WriteLine("\nCreating and adding vehicles to the Live IoT system");

            //Create Vehicles ServiceAgent
            VehiclesAzureSFService vehiclesService = new VehiclesAzureSFService();

            //ADD VEHICLE 1 /////////////////////////////////////////////
            //Seattle Pike's Place coordinates (47.608875,-122.340098)
            Vehicle vehicle1 = new Vehicle { TenantId = "CDLTLL", Make = "Ford", Model = "Mustang", Latitude = 47.608875, Longitude = -122.340098, Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Ford-Mustang-2015-small.jpg" };

            //Guid createdVehicleGuid = await vehiclesService.CreateVehicle(_urlPrefix, tenantId, vehicle1);
            var retVal = vehiclesService.CreateVehicle(_urlPrefix, tenantId, vehicle1);
            Guid createdVehicleGuid = retVal.Result;

            /////////////////////////////////////////////////////////////////////////

            //ADD VEHICLE 2 /////////////////////////////////////////////

            //Seattle STARBUCKS ORIGINAL coordinates 47.610021, -122.342649
            //tempVehicleData2.GPSCoordinates.Latitude = 47.610021;
            //tempVehicleData2.GPSCoordinates.Longitude = -122.342649;


            /////////////////////////////////////////////////////////////////////////

            //ADD VEHICLE 3 /////////////////////////////////////////////

            //Seattle CONVENTION CENTER coordinates 47.612283, -122.331918
            //tempVehicleData3.GPSCoordinates.Latitude = 47.612283;
            //tempVehicleData3.GPSCoordinates.Longitude = -122.331918;


            /////////////////////////////////////////////////////////////////////////

            Console.WriteLine("##### END OF ADDING VEHICLES PROCESS #####");
            Console.WriteLine("##########################################");

        }

        static string GetTenantId(string tenantId = null)
        {
            Console.Write("\n\nEnter your TenantId: ");
            if (String.IsNullOrEmpty(tenantId))
                return Console.ReadLine().Trim();

            Console.WriteLine(tenantId);

            return tenantId;
        }

        #region Common CLI Utils
        static string Prompt(string message)
        {
            Console.Write(String.Format("\n{0}: ", message));
            return Console.ReadLine();
        }

        static void DisplayMenu()
        {
            Console.WriteLine();
            foreach (var item in _menuItems.Values)
                Console.WriteLine("   {0} - {1}", item.Id, item.Display);
        }

        static void AddMenuItem(string display, Action action)
        {
            var id = (_menuItems.Count + 1).ToString();
            _menuItems.Add(id, new MenuItem(id, display, action));
        }

        static void AddMenuItem(string display, string id)
        {
            _menuItems.Add(id, new MenuItem(id, display));
        }

        static string GetSelection(string selection = null)
        {
            Console.Write("\n\nEnter your selection (m for menu): ");
            if (String.IsNullOrEmpty(selection))
                return Console.ReadLine().Trim().ToLower();

            Console.WriteLine(selection);

            return selection;
        }

        static void Header(string header)
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine(header);
            Console.WriteLine("------------------------------------------------\n");
        }
        #endregion 
    }

    class MenuItem
    {
        public string Id { get; private set; }

        public string Display { get; private set; }

        private Action _action;

        public MenuItem(string id, string display, Action action = null)
        {
            Id = id;
            Display = display;
            _action = action;
        }

        public void Run()
        {
            if (_action != null)
                _action.Invoke();
        }
    }
}
