using MyWorld.Client.Core.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;
using System.Net.Http.Headers;


namespace MyWorld.Client.Core.Services
{
    public class VehiclesAzureSFService : BaseRequest, IVehiclesService
    {
        public VehiclesAzureSFService()
            : base()
        {
            this.UrlPrefix = AppSettings.ServerlUrl;
        }

        //TBD Check if using TenantId in headers
        //public ClinicAppointmentsService(string urlPrefix, int tenantId)
        //    : base(urlPrefix, tenantId)
        //{

        //}


        public async Task<IList<Vehicle>> GetVehiclesInArea(string tenantId, double topLatitude, double leftLongitude, double bottomLatitude, double rightLongitude)
        {
            //Sample: http://localhost:8740/api/vehicles/?tenantId=CDLTLL&topLatitude=47.6670476481776&leftLongitude=-122.169899876643&bottomLatitude=47.6130883518224&rightLongitude=-122.089816123357

            string url = $"{_UrlPrefix}api/vehicles/?tenantid={tenantId}&topLatitude={topLatitude}&leftLongitude={leftLongitude}&bottomLatitude={bottomLatitude}&rightLongitude={rightLongitude}";
            List<Vehicle> vehicles = await GetAsync<List<Vehicle>>(url);
            return vehicles;

            //using (var client = new HttpClient())
            //{
            //    //TO DO: Change from Base URI from Config..
            //    client.BaseAddress = new Uri("http://localhost:8740/api/vehicles/");
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    // HTTP GET
            //    //Sample: http://localhost:8740/api/vehicles/?tenantId=CDLTLL&topLatitude=47.6670476481776&leftLongitude=-122.169899876643&bottomLatitude=47.6130883518224&rightLongitude=-122.089816123357

            //    string fullUri = string.Format("api/vehicles/?tenantId={0}&topLatitude={1}&leftLongitude={2}&bottomLatitude={3}&rightLongitude={4}", tenantId, topLatitude, leftLongitude, bottomLatitude, rightLongitude);
            //    HttpResponseMessage response = await client.GetAsync(fullUri);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        vehicles = await response.Content.ReadAsAsync<List<Vehicle>>();
            //        //Console.WriteLine("data{0}\t${1}\t{2}", val0, val1, val2);
            //    }
            //}

            //using (var httpClient = new HttpClient())
            //{
            //    httpClient.BaseAddress = new Uri("http://localhost:8740/api/vehicles");

            //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var sentimentRequest = "data.ashx/amla/text-analytics/v1/GetSentiment?Text=" + inputTextEncoded;
            //    var responseTask = httpClient.GetAsync(sentimentRequest);
            //    responseTask.Wait();
            //    var response = responseTask.Result;
            //    var contentTask = response.Content.ReadAsStringAsync();
            //    var content = contentTask.Result;
            //    if (!response.IsSuccessStatusCode)
            //    {
            //        return -1;
            //    }

            //    dynamic sentimentResult = JsonConvert.DeserializeObject<dynamic>(content);
            //    score = (decimal)sentimentResult.Score;
            //}

            //Vehicles - Mock data
            //List<Vehicle>  vehicles = new List<Vehicle>();
            //vehicles.Add(new Vehicle { Id = 100, TenantId = "CDLTLL", Make = "Chevrolet", Model = "Camaro", Latitude= 47.644958, Longitude= -122.131077, Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Camaro-RS-2012-small.jpg" });
            //vehicles.Add(new Vehicle { Id = 101, TenantId = "CDLTLL", Make = "Chevrolet", Model = "Tahoe", Latitude = 47.661542, Longitude = -122.131231, Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Tahoe-Z71-2015-small.jpg" });

            //return await Task.Run(() => vehicles);
        }

        public async Task<IList<Vehicle>> GetAllVehiclesFromTenant(string tenantId)
        {
            //Vehicles - Mock data
            List<Vehicle> vehicles = new List<Vehicle>();
            vehicles.Add(new Vehicle { Id = Guid.NewGuid(), TenantId = "CDLTLL", Make = "Chevrolet", Model = "Camaro", Latitude = 47.644958, Longitude = -122.131077, Year = "2012", LicensePlate = "AJX6940", VIN = "QWERTYUIOPASDFG17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Camaro-RS-2012-small.jpg" });
            vehicles.Add(new Vehicle { Id = Guid.NewGuid(), TenantId = "CDLTLL", Make = "Chevrolet", Model = "Tahoe", Latitude = 47.661542, Longitude = -122.131231, Year = "2015", LicensePlate = "XXX1234", VIN = "ASDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/Chevy-Tahoe-Z71-2015-small.jpg" });
            vehicles.Add(new Vehicle { Id = Guid.NewGuid(), TenantId = "CDLTLL", Make = "BMW", Model = "Z4", Latitude = 40.681608, Longitude = -3.620753, Year = "2007", LicensePlate = "M-XXX1234", VIN = "SPDFGUIOPASDFGX17", FrontViewPhoto = "http://myworldfiles.blob.core.windows.net/vehicles/BMW-Z4-2007-small.jpg" });

            return await Task.Run(() => vehicles);
        }
        
    }
}
