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
            InitializeVehicleTemplates();

            AddMenuItem("Ping WebAPI service in Service Fabric Cluster", Ping);
            AddMenuItem("Add a few new vehicles to your tenant", AddFewVehiclesToTenant);
            AddMenuItem("Add hundreds of vehicles to WA state", AddVehiclesToWAState);
            AddMenuItem("Add thousands of vehicles to the U.S.", AddVehiclesToTheUSA);

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
            Console.WriteLine("Pinging MyWorld WebAPI Front-End at: " + _urlPrefix);
            VehiclesAzureSFService vehiclesService = new VehiclesAzureSFService();
            var reVal = vehiclesService.PingVehiclesService(_urlPrefix);

            Console.WriteLine("\nResponse: {0}", reVal.Result.ToString());
        }

        static void InitializeVehicleTemplates()
        {
            //These Vehicle-Templates are used ONLY when generating hundreds or thousands of vehicles from the CITY .CSV files

            //Add Type/Generic Vehicles to re-use
            //There vehicles do NOT have Coordinates (to be added) and the tenant is "TENANT" to be composed like "TENANT-1", "TENANT-2", etc.
            Vehicle vehicle1 = new Vehicle { TenantId = "TENANT", Make = "Chevrolet", Model = "Camaro", Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Camaro-RS-2012-small.jpg" };
            _baseTypeVehicles.Add(1, vehicle1);

            Vehicle vehicle2 = new Vehicle { TenantId = "TENANT", Make = "Chevrolet", Model = "Tahoe Z71", Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Tahoe-Z71-2015-small.jpg" };
            _baseTypeVehicles.Add(2, vehicle2);

            Vehicle vehicle3 = new Vehicle { TenantId = "TENANT", Make = "Ford", Model = "Mustang", Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Ford-Mustang-2015-small.jpg" };
            _baseTypeVehicles.Add(3, vehicle3);

            Vehicle vehicle4 = new Vehicle { TenantId = "TENANT", Make = "Ford", Model = "Explorer", Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Ford-Explorer-2016-small.jpg" };
            _baseTypeVehicles.Add(4, vehicle4);

            Vehicle vehicle5 = new Vehicle { TenantId = "TENANT", Make = "BMW", Model = "Z4 3.0si", Year = "2007", LicensePlate = "M-XXX1234", VIN = "SPDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/BMW-Z4-2007-small.jpg" };
            _baseTypeVehicles.Add(5, vehicle5);
        }

        static void AddFewVehiclesToTenant()
        {
            String tenantId = PromptForTenantId();  //Like CDLTLL, SCOTT, TENANT-1, etc.
            Console.WriteLine("\nAdding a few vehicles to your tenant");

            //Create Vehicles ServiceAgent
            VehiclesAzureSFService vehiclesService = new VehiclesAzureSFService();

            Vehicle vehicle1 = new Vehicle { TenantId = tenantId, Make = "Chevrolet", Model = "Camaro RS", Latitude = 47.644958, Longitude = -122.131077, Year = "2012", LicensePlate = "CAM6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Camaro-RS-2012-small.jpg" };
            var retVal1 = vehiclesService.CreateVehicle(_urlPrefix, vehicle1.TenantId, vehicle1);
            Guid createdVehicleGuid1 = retVal1.Result;

            Vehicle vehicle2 = new Vehicle { TenantId = tenantId, Make = "Chevrolet", Model = "Tahoe Z71", Latitude = 47.661542, Longitude = -122.131231, Year = "2015", LicensePlate = "TAH1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Tahoe-Z71-2015-small.jpg" };
            var retVal2 = vehiclesService.CreateVehicle(_urlPrefix, vehicle2.TenantId, vehicle2);
            Guid createdVehicleGuid2 = retVal2.Result;

            Vehicle vehicle3 = new Vehicle { TenantId = tenantId, Make = "BMW", Model = "Z4 3.0si", Latitude = 40.681608, Longitude = -3.620753, Year = "2007", LicensePlate = "M-XXX1234", VIN = "SPDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/BMW-Z4-2007-small.jpg" };
            var retVal3 = vehiclesService.CreateVehicle(_urlPrefix, vehicle3.TenantId, vehicle3);
            Guid createdVehicleGuid3 = retVal3.Result;

            Vehicle vehicle4 = new Vehicle { TenantId = "CESARDL", Make = "Ford", Model = "Mustang", Latitude = 47.654177, Longitude = -122.132442, Year = "2012", LicensePlate = "ZXC6940", VIN = "FFFERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Ford-Mustang-2015-small.jpg" };
            var retVal4 = vehiclesService.CreateVehicle(_urlPrefix, vehicle4.TenantId, vehicle4);
            Guid createdVehicleGuid4 = retVal4.Result;

            Vehicle vehicle5 = new Vehicle { TenantId = "CESARDL", Make = "Ford", Model = "Explorer", Latitude = 47.645120, Longitude = -122.138143, Year = "2015", LicensePlate = "IOP1234", VIN = "KKKKKKUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Ford-Explorer-2016-small.jpg" };
            var retVal5 = vehiclesService.CreateVehicle(_urlPrefix, vehicle5.TenantId, vehicle5);
            Guid createdVehicleGuid5 = retVal5.Result;

            Vehicle vehicle6 = new Vehicle { TenantId = "CESARDL", Make = "Chevrolet", Model = "Camaro RS", Latitude = 47.608875, Longitude = -122.340098, Year = "2012", LicensePlate = "BNM6940", VIN = "ZZZZZZZUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Camaro-RS-2012-small.jpg" };
            var retVal6 = vehiclesService.CreateVehicle(_urlPrefix, vehicle6.TenantId, vehicle6);
            Guid createdVehicleGuid6 = retVal6.Result;
            //Seattle Pike's Place coordinates (47.608875,-122.340098)
        }

        static void AddVehiclesToWAState()
        {
            Console.WriteLine("\nAdding a few hundreds of vehicles to Washington state, one per city");

            AddVehiclesFromCSVFile("cities-wa.csv");
            //ReadTest("cities-wa.csv");

            Console.WriteLine("##### END OF ADDING VEHICLES PROCESS #####");
            Console.WriteLine("##########################################");
        }

        static void AddVehiclesToTheUSA()
        {

            Console.WriteLine("\nAdding Thousands of vehicles to the U.S. into the MyWorld system");

            AddVehiclesFromCSVFile("cities-us.csv");
            //ReadTest("cities-us.csv");

            Console.WriteLine("##### END OF ADDING VEHICLES PROCESS #####");
            Console.WriteLine("##########################################");
        }

        static string PromptForTenantId(string tenantId = null)
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

                        //Create vehicle in Service Fabric cluster stateful services
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
