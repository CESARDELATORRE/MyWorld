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
    //(CDLTLL) Vehicle is an Entity class used by the VehiclesStatefulService and the VehicleActor service.
    [DataContract]
    public class Vehicle : Entity
    {
        #region Properties (Domain Data)

        [DataMember]
        [JsonProperty("id")]   //Overrride so we add the JSON attribute to the Id property
        //[JsonConverter(typeof(Guid))]
        public override Guid Id { get; set; }

        [DataMember]
        [JsonProperty("alias")]
        public string Alias { get; set; } = string.Empty;

        [DataMember]
        [JsonProperty("tenantid")]
        public string TenantId { get; set; } = string.Empty;

        private GeoCoordinate _geoLocation = null;
        //Don't serialize GeoCoordinate, as it is a heavy object with many properties
        //Just use it as internal Domain Value-Object with useful logic, within Vehicle
        [IgnoreDataMember]
        public GeoCoordinate GeoLocation
        {
            get
            {
                return _geoLocation;
            }
            set
            {
                _geoLocation = value;
                Latitude = value.Latitude;
                Longitude = value.Longitude;
                EncodeGeohash();
                EncodeGeoQuadKey();
            }
        }

        [DataMember]
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [DataMember]
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        private string _geoLocationHash = string.Empty;
        [DataMember]
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
            set
            {
                _geoLocationHash = value;
            }
        }

        private long _geoLocationQuadKey;
        [DataMember]
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
            set
            {
                _geoLocationQuadKey = value;
            }
        }

        [DataMember]
        [JsonProperty("make")]
        public string Make { get; set; } = string.Empty;

        [DataMember]
        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        string _fullTitle = string.Empty;
        [DataMember]
        [JsonProperty("fulltitle")]
        public string FullTitle
        {
            get
            {
                if(_fullTitle == string.Empty)
                    _fullTitle = Make + " " + Model;

                return _fullTitle;
            }
            set
            {
                _fullTitle = value;
            }
        }

        [DataMember]
        [JsonProperty("year")]
        public string Year { get; set; } = string.Empty;

        [DataMember]
        [JsonProperty("licenseplate")]
        public string LicensePlate { get; set; } = string.Empty;

        [DataMember]
        [JsonProperty("vin")]  //Vehicle Identification Number
        public string VIN { get; set; } = string.Empty;

        [DataMember]
        [JsonProperty("frontviewphoto")]
        public string FrontViewPhoto { get; set; } = string.Empty;

        [DataMember]
        [JsonProperty("backviewphoto")]
        public string BackViewPhoto { get; set; } = string.Empty;

        [DataMember]
        [JsonProperty("sideviewphoto")]
        public string SideViewPhoto { get; set; } = string.Empty;

        #endregion

        #region Methods (Domain Logic)

        private void EncodeGeohash()
        {
            // Calculate hash with full precision
            const int precision = 13;
            _geoLocationHash = GeohashTool.Encode(Latitude,
                                                  Longitude,
                                                  precision);
        }

        private void EncodeGeoQuadKey()
        {
            _geoLocationQuadKey = GeoTileTool.GeoCoordinateToInt64QuadKey(Latitude, Longitude);
        }
        public long GetPartitionKey()
        {
            return GeoQuadKey;
        }
        #endregion

    }
}
