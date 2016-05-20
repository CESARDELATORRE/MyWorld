using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MyWorld.Client.Core.Model
{

    public class Vehicle
    {
        [JsonProperty("id")]
        public int Id { get; set; } = 0;

        [JsonProperty("make")]
        public string Make { get; set; } = string.Empty;

        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        [JsonProperty("year")]
        public string Year { get; set; } = string.Empty;

        [JsonProperty("licenseplate")]
        public string LicensePlate { get; set; } = string.Empty;

        [JsonProperty("vin")]
        public string VIN { get; set; } = string.Empty;  //Vehicle Identification Number

        
        [JsonProperty("frontviewphoto")]
        public string FrontViewPhoto { get; set; } = string.Empty;

        [JsonProperty("backviewphoto")]
        public string BackViewPhoto { get; set; } = string.Empty;

        [JsonProperty("sideviewphoto")]
        public string SideViewPhoto { get; set; } = string.Empty;


        //(CDLTLL - TBD)
        //ICommand imageCommand;
        //public ICommand iCommand =>
        //    imageCommand ?? (imageCommand = new Command(ExecuteImageCommand));

        //void ExecuteImageCommand()
        //{
        //    if (string.IsNullOrWhiteSpace(image))
        //        return;
        //    //(CDLTLL - TBD)
        //    //MessagingService.Current.SendMessage(MessageKeys.NavigateToImage, image);
        //}

    }
}
