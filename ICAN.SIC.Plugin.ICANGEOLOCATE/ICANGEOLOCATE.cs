using GeographicLocationProviders;
using GeoSharp;
using ICAN.SIC.Abstractions;
using ICAN.SIC.Abstractions.ConcreteClasses;
using ICAN.SIC.Abstractions.IMessageVariants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ICANGEOLOCATE
{
    public class ICANGEOLOCATE : AbstractPlugin
    {
        ICANGEOLOCATEUtility utility;
        ICANGEOLOCATEHelper helper;

        public ICANGEOLOCATE() : base("ICANGEOLOCATEv1")
        {
            utility = new ICANGEOLOCATEUtility();
            helper = new ICANGEOLOCATEHelper(utility);

            hub.Subscribe<IGeoLocationRequest>(Service);
        }

        private void Service(IGeoLocationRequest request)
        {
            Console.WriteLine("Request: " + request.CommandText);
            string[] split = request.CommandText.Split(',');

            string functionName = split[0];
            List<string> paramList = new List<string>();
            for (int i = 1; i < split.Length; i++)
            {
                paramList.Add(split[i]);
            }

            var modules = GetType().GetMethods();

            foreach (var methodInfo in modules)
            {
                if (methodInfo.Name.ToLower() == functionName.ToLower() + "Processor".ToLower())
                {
                    var res = methodInfo.Invoke(this, paramList.ToArray());
                    break;
                }
            }
        }

        //public void ReverseGeoCodeProcessor(string lat, string lon)
        //{
        //    ReverseGeoCode geocode = helper.GetReverseGeoCodeInstance();
        //    bool errorFound = false;

        //    if (!double.TryParse(lat, out double dLat))
        //    {
        //        utility.ShowError($"Provided latitude value '{lat}' cannot be converted to double");
        //        errorFound = true;
        //    }

        //    if (!double.TryParse(lon, out double dLon))
        //    {
        //        utility.ShowError($"Provided longitude value '{lon}' cannot be converted to double");
        //        errorFound = true;
        //    }

        //    if (errorFound)
        //    {
        //        utility.ShowError($"ReverseGeoCodeProcessor(lat={lat}, lon={lon}) failed");
        //        return;
        //    }

        //    string placeName = geocode.NearestPlaceName(dLat, dLon);
        //    MachineMessage message = new MachineMessage("[ReverseGeoCodeProcessor] " + placeName);
        //    hub.Publish<IMachineMessage>(message);
        //}

        public void GetCurrentCoordinatesProcessor()
        {
            GeoCoordinates coords = helper.GetCurrentCoordinates();

            utility.PushMessage(hub, "[Coordinates] " + coords.Latitude + "," + coords.Longitude);
        }

        public void GetCurrentCoordinatesControlledProcessor(string timeSpanSec, string timeoutSec)
        {
            GeoCoordinates coords = helper.GetCurrentCoordinates(TimeSpan.FromSeconds(double.Parse(timeSpanSec)), TimeSpan.FromSeconds(double.Parse(timeoutSec)));

            utility.PushMessage(hub, "[Coordinates] " + coords.Latitude + "," + coords.Longitude);
        }

        public void AddCurrentLocalPlaceProcessor(string name)
        {
            this.AddCurrentPlaceProcessor(name, DateTime.Now.ToShortDateString(), "IN", "", "10",
                "true", "false", "false", "false", "false", "false", "false", "false", "false");
        }

        public void AddCurrentBeachPlaceProcessor(string name)
        {
            this.AddCurrentPlaceProcessor(name, DateTime.Now.ToShortDateString(), "IN", "", "10", 
                "false", "false", "false", "false", "false", "false", "false", "false", "true");
        }

        public void AddCurrentWaterBodyProcessor(string name)
        {
            AddCurrentReservoirPlaceProcessor(name);
        }

        public void AddCurrentReservoirPlaceProcessor(string name)
        {
            this.AddCurrentPlaceProcessor(name, DateTime.Now.ToShortDateString(), "IN", "", "10",
                "false", "false", "false", "false", "false", "false", "false", "true", "false");
        }

        public void AddCurrentAtmProcessor(string name)
        {
            this.AddCurrentPlaceProcessor(name, DateTime.Now.ToShortDateString(), "IN", "", "10",
                "false", "false", "false", "true", "false", "false", "false", "false", "false");
        }

        public void AddCurrentHillProcessor(string name)
        {
            this.AddCurrentPlaceProcessor(name, DateTime.Now.ToShortDateString(), "IN", "", "10",
                "false", "false", "false", "false", "false", "true", "false", "false", "false");
        }

        public void AddCurrentPlaceProcessor(string name,
            string dateTime, string countryCode, string timeZone, string population,
            string anyLocality, string busStop, string bank,
            string atm, string ground, string hill,
            string headland, string reservoir, string beach)
        {
            // timeZone = null;

            helper.AddCurrentPlace(name,
                DateTime.Parse(dateTime), countryCode, null, int.Parse(population),
                bool.Parse(anyLocality), bool.Parse(busStop), bool.Parse(bank),
                bool.Parse(atm), bool.Parse(ground), bool.Parse(hill),
                bool.Parse(headland), bool.Parse(reservoir), bool.Parse(beach));

            utility.PushMessage(hub, "[AddCurrentPlace] " + name + "," + countryCode);
        }

        public void AddCurrentPlaceControlledProcessor(string timeSpanSec, string timeoutSec, string name,
            string dateTime, string countryCode, string timeZone, string population,
            string anyLocality, string busStop, string bank,
            string atm, string ground, string hill,
            string headland, string reservoir, string beach)
        {
            // timeZone = null;

            helper.AddCurrentPlace(TimeSpan.FromSeconds(double.Parse(timeSpanSec)), TimeSpan.FromSeconds(double.Parse(timeoutSec)), name,
                DateTime.Parse(dateTime), countryCode, null, int.Parse(population),
                bool.Parse(anyLocality), bool.Parse(busStop), bool.Parse(bank),
                bool.Parse(atm), bool.Parse(ground), bool.Parse(hill),
                bool.Parse(headland), bool.Parse(reservoir), bool.Parse(beach));

            utility.PushMessage(hub, "[AddCurrentPlace] " + name);
        }

        public void GetPlaceProcessor()
        {
            GeoName place = helper.GetPlace();

            utility.PushMessage(hub, "[Place] " + place.Name + "," + place.CountryCode + "," + place.FeatureClass 
                + ",lat_" + place.Latitude + ",lon_" + place.Longitude);
        }

        public void GetPlaceNameProcessor()
        {
            string place = helper.GetPlaceName();

            utility.PushMessage(hub, "[Place] " + place);
        }

        public void GetPlaceControlledProcessor(string timeSpanSec, string timeoutSec)
        {
            GeoName place = helper.GetPlace(TimeSpan.FromSeconds(double.Parse(timeSpanSec)), TimeSpan.FromSeconds(double.Parse(timeoutSec)));

            utility.PushMessage(hub, "[Place] " + place.Name + "," + place.CountryCode + "," + place.FeatureClass
                + ",lat_" + place.Latitude + ",lon_" + place.Longitude);
        }

        public void GetPlaceNameControlledProcessor(string timeSpanSec, string timeoutSec)
        {
            string place = helper.GetPlaceName(TimeSpan.FromSeconds(double.Parse(timeSpanSec)), TimeSpan.FromSeconds(double.Parse(timeoutSec)));

            utility.PushMessage(hub, "[Place] " + place);
        }

        public void RemovePlaceByGeoCodeProcessor(string latitude, string longitude)
        {
            helper.RemovePlaceByGeoCode(double.Parse(latitude), double.Parse(longitude));

            utility.PushMessage(hub, "[RemoveCurrentPlace] " + "lat_" + latitude + ",lon_" + longitude);
        }

        public void AddPlaceProcessor(string lat, string lon, string name,
            string dateTime, string countryCode, string timeZone, string population,
            string anyLocality, string busStop, string bank,
            string atm, string ground, string hill,
            string headland, string reservoir, string beach)
        {
            // timeZone = null;

            helper.AddPlace(double.Parse(lat), double.Parse(lon), name,
                DateTime.Parse(dateTime), countryCode, null, int.Parse(population),
                bool.Parse(anyLocality), bool.Parse(busStop), bool.Parse(bank),
                bool.Parse(atm), bool.Parse(ground), bool.Parse(hill),
                bool.Parse(headland), bool.Parse(reservoir), bool.Parse(beach));

            utility.PushMessage(hub, "[AddPlace] " + name + "," + countryCode
                + ",lat_" + lat + ",lon_" + lon);
        }

        public void NearestPlaceNameProcessor(string latitude, string longitude)
        {
            string place = helper.NearestPlaceName(double.Parse(latitude), double.Parse(longitude));

            utility.PushMessage(hub, "[Place] " + place);
        }

        public void NearestPlaceProcessor(string latitude, string longitude)
        {
            GeoName place = helper.NearestPlace(double.Parse(latitude), double.Parse(longitude));

            utility.PushMessage(hub, "[Place] " + place.Name + "," + place.CountryCode + "," + place.FeatureClass
                + ",lat_" + place.Latitude + ",lon_" + place.Longitude);
        }

        public override void Dispose()
        {
            hub.Unsubscribe<IGeoLocationRequest>(Service);

            utility = null;
            helper = null;
        }
    }
}
