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
using ReadWriteCsv;


namespace MyWorld.Client.Windows.CLI
{
    class Program
    {
        private static string _urlPrefix = ConfigurationManager.AppSettings["AzureVehiclesServiceUrlPrefix"];
        private static IDictionary<string, MenuItem> _menuItems = new Dictionary<string, MenuItem>();

        private static IDictionary<int, Vehicle> _baseTypeVehicles = new Dictionary<int, Vehicle>();
        private static IDictionary<int, Vehicle> _randomVehicles = new Dictionary<int, Vehicle>();

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
            //String tenantId = GetTenantId();

            Console.WriteLine("\nAdding Thousands of vehicles to the MyWorld system");

            //ADD VEHICLE 1 /////////////////////////////////////////////
            //Seattle Pike's Place coordinates (47.608875,-122.340098)
            Vehicle vehiclePlusCDLTLL = new Vehicle { TenantId = "CDLTLL", Make = "Ford", Model = "Mustang", Latitude = 47.608875, Longitude = -122.340098, Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Ford-Mustang-2015-small.jpg" };


            //Add Type/Generic Vehicles
            

            //(CDLTLL) MOVE THIS TO A SINGLE EXECUTION!!
            Vehicle vehicle1 = new Vehicle { TenantId = "TENANT", Make = "Chevrolet", Model = "Camaro", Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Camaro-RS-2012-small.jpg" };
            _baseTypeVehicles.Add(1, vehicle1);

            Vehicle vehicle2 = new Vehicle { TenantId = "TENANT", Make = "Chevrolet", Model = "Tahoe", Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Tahoe-Z71-2015-small.jpg" };
            _baseTypeVehicles.Add(2, vehicle2);

            Vehicle vehicle3 = new Vehicle { TenantId = "TENANT", Make = "Ford", Model = "Mustang", Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Ford-Mustang-2015-small.jpg" };
            _baseTypeVehicles.Add(3, vehicle3);

            Vehicle vehicle4 = new Vehicle { TenantId = "TENANT", Make = "Ford", Model = "Explorer", Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Ford-Explorer-2016-small.jpg" };
            _baseTypeVehicles.Add(4, vehicle4);

            Vehicle vehicle5 = new Vehicle { TenantId = "TENANT", Make = "BMW", Model = "Z4", Year = "2007", LicensePlate = "M-XXX1234", VIN = "SPDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/BMW-Z4-2007-small.jpg" };
            _baseTypeVehicles.Add(5, vehicle5);


            //AddVehiclesFromCSVFile("cities-us.csv");
            AddVehiclesFromCSVFile("cities-wa.csv");
            

            //ReadTest("cities-us.csv");

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

        
        public static void AddVehiclesFromCSVFile(string fileName)
        {
            //Create Vehicles ServiceAgent
            VehiclesAzureSFService vehiclesService = new VehiclesAzureSFService();

            // Read sample data from CSV file
            using (CsvFileReader reader = new CsvFileReader(fileName))
            {
                int rowCounter = 0;
                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    String latitudeFromCity = string.Empty;
                    String longitudeFromCity = string.Empty;
                    int columnCounter = 1;
                    foreach (string s in row)
                    {
                        switch (columnCounter)
                        {
                            case 4:
                                latitudeFromCity = s;
                                break;
                            case 5:
                                longitudeFromCity = s;
                                break;
                            default:
                                break;
                        }
                        
                        //Add to the line for Console
                        Console.Write(s);
                        Console.Write(" ");
                        columnCounter++;
                    }

                    Console.WriteLine();
                    if (rowCounter > 0)
                    {
                        //Get Ramdon Vehicle from Dict.
                        Random r = new Random();
                        int randomNumber = r.Next(1, 5);
                        Vehicle randomVehicleRef = new Vehicle();

                        var element = _baseTypeVehicles.ElementAt(randomNumber);
                        //_baseTypeVehicles.TryGetValue(randomNumber, out randomVehicleRef);
                        //Vehicle copiedVehicle = randomVehicleRef.ShallowCopy();

                        Vehicle copiedVehicle = element.Value.ShallowCopy();
                        
                        copiedVehicle.TenantId = copiedVehicle.TenantId + "-" + rowCounter.ToString();
                        copiedVehicle.Latitude = Convert.ToDouble(latitudeFromCity);
                        copiedVehicle.Longitude = Convert.ToDouble(longitudeFromCity);
                        Console.WriteLine("This " + copiedVehicle.TenantId + " is being added");

                        var retVal = vehiclesService.CreateVehicle(_urlPrefix, copiedVehicle.TenantId, copiedVehicle);
                        Guid createdVehicleGuid = retVal.Result;
                    }
                    rowCounter++;
                }
            }
        }

        public static void ReadTest(string fileName)
        {
            // Read sample data from CSV file
            using (CsvFileReader reader = new CsvFileReader(fileName))
            {
                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    foreach (string s in row)
                    {
                        Console.Write(s);
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
            }
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
