using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using GeographicLib.GeoCoordinatePortable;
//(In .NET Framework 4.x) using System.Device.Location;
using GeographicLib;
using GeographicLib.Tiles;

using Vehicles.Domain.SeedWork;

namespace Vehicles.Domain.Model
{
    public class Vehicle : Entity
    {
        #region Properties (Domain Data)
        [JsonProperty("id")]   //Overrride so we add the JSON attribute to the Id property
        //[JsonConverter(typeof(Guid))]
        public override Guid Id { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; } = string.Empty;

        [JsonProperty("tenantid")]
        public string TenantId { get; set; } = string.Empty;

        //private GeoCoordinate _geoLocation = null;
        ////[IgnoreDataMember]
        //public GeoCoordinate GeoLocation
        //{
        //    get
        //    {
        //        return _geoLocation;
        //    }
        //    set
        //    {
        //        _geoLocation = value;
        //        _latitude = value.Latitude;
        //        _longitude = value.Longitude;
        //        EncodeGeohash();
        //        EncodeGeoQuadKey();
        //    }
        //}

        //private double _latitude;
        [JsonProperty("latitude")]
        public double Latitude { get; set; }      

        //private double _longitude;
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        private string _geoLocationHash = string.Empty;
        [JsonProperty("geohash")]
        public string Geohash
        {
            get
            {
                if (_geoLocationHash == string.Empty)
                {
                    EncodeGeohash();
                }

                return _geoLocationHash;
            }
        }

        private void EncodeGeohash()
        {
            // Calculate hash with full precision
            const int precision = 13;
            _geoLocationHash = GeohashTool.Encode(Latitude,
                                                  Longitude,
                                                  precision);
        }

        private long _geoLocationQuadKey;
        [JsonProperty("geoquadkey")]
        public long GeoQuadKey
        {
            get
            {
                if (_geoLocationQuadKey == 0)
                {
                    EncodeGeoQuadKey();
                }

                return _geoLocationQuadKey;
            }
        }

        public long GetPartitionKey()
        {
            return GeoQuadKey;
        }

        private void EncodeGeoQuadKey()
        {
            _geoLocationQuadKey = GeoTileTool.GeoCoordinateToInt64QuadKey(Latitude, Longitude);
        }

        [JsonProperty("make")]
        public string Make { get; set; } = string.Empty;

        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        [JsonProperty("fulltitle")]
        public string FullTitle
        {
            get
            {
                return Make + " " + Model;
            }

        }

        [JsonProperty("year")]
        public string Year { get; set; } = string.Empty;

        [JsonProperty("licenseplate")]
        public string LicensePlate { get; set; } = string.Empty;

        [JsonProperty("vin")]  //Vehicle Identification Number
        public string VIN { get; set; } = string.Empty;  


        [JsonProperty("frontviewphoto")]
        public string FrontViewPhoto { get; set; } = string.Empty;

        [JsonProperty("backviewphoto")]
        public string BackViewPhoto { get; set; } = string.Empty;

        [JsonProperty("sideviewphoto")]
        public string SideViewPhoto { get; set; } = string.Empty;

        #endregion

        #region Methods (Domain Logic)
        #endregion

    }
}
