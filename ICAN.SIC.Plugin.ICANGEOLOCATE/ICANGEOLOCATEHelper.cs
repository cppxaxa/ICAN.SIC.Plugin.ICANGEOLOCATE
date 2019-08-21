using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeographicLocationProviders;
using GeoSharp;

namespace ICAN.SIC.Plugin.ICANGEOLOCATE
{
    class ICANGEOLOCATEHelper
    {
        private GeoNameManager _geoNameManagerInstance = null;
        private ICANGEOLOCATEUtility utility;

        public ICANGEOLOCATEHelper(ICANGEOLOCATEUtility utility)
        {
            this.utility = utility;
        }

        private GeoNameManager GetGeoNameManagerInstance()
        {
            if (_geoNameManagerInstance == null)
            {
                try
                {
                    _geoNameManagerInstance = new GeoNameManager();
                    _geoNameManagerInstance.Initialize(@"GeoResources\GeoLocateGeoNamesDb.tsv", @"GeoResources\CustomGeoNamesDb.tsv");

                    _geoNameManagerInstance.SetGeographicLocationProvider(new GeographicLocationProviders.DeviceGeographicLocationProvider());
                }
                catch(Exception ex)
                {
                    string message = "";

                    message = "Exception: " + ex.Message;
                    
                    if (ex.InnerException != null)
                    {
                        message += "\n\nInnerException: " + ex.InnerException.Message;
                    }

                    message += "\n\nStackTrace: " + ex.StackTrace;

                    utility.PushError(message);

                    _geoNameManagerInstance = null;
                }
            }
            return _geoNameManagerInstance;
        }

        public GeoCoordinates GetCurrentCoordinates()
        {
            GeoCoordinates coords = GetGeoNameManagerInstance().GetCurrentCoordinates();
            return coords;
        }

        public GeoCoordinates GetCurrentCoordinates(TimeSpan timeSpan, TimeSpan timeout)
        {
            GeoCoordinates coords = GetGeoNameManagerInstance().GetCurrentCoordinates(timeSpan, timeout);
            return coords;
        }

        public void AddCurrentPlace(string name,
            DateTime dateTime, string countryCode = "IN", TimeZone timeZone = null, int population = 100,
            bool anyLocality = false, bool busStop = false, bool bank = false,
            bool atm = false, bool ground = false, bool hill = false,
            bool headland = false, bool reservoir = false, bool beach = false)
        {
            GetGeoNameManagerInstance().AddCurrentPlace(name,
            dateTime, countryCode, timeZone, population,
            anyLocality, busStop, bank,
            atm, ground, hill,
            headland, reservoir, beach);
        }

        public void AddCurrentPlace(TimeSpan timeSpan, TimeSpan timeout, string name,
            DateTime dateTime, string countryCode = "IN", TimeZone timeZone = null, int population = 100,
            bool anyLocality = false, bool busStop = false, bool bank = false,
            bool atm = false, bool ground = false, bool hill = false,
            bool headland = false, bool reservoir = false, bool beach = false)
        {
            GetGeoNameManagerInstance().AddCurrentPlace(timeSpan, timeout, name,
            dateTime, countryCode, timeZone, population,
            anyLocality, busStop, bank,
            atm, ground, hill,
            headland, reservoir, beach);
        }

        public GeoName GetPlace()
        {
            GeoName res = GetGeoNameManagerInstance().GetPlace();
            return res;
        }

        public string GetPlaceName()
        {
            string res = GetGeoNameManagerInstance().GetPlaceName();
            return res;
        }

        public GeoName GetPlace(TimeSpan timeSpan, TimeSpan timeout)
        {
            GeoName res = GetGeoNameManagerInstance().GetPlace(timeSpan, timeout);
            return res;
        }

        public string GetPlaceName(TimeSpan timeSpan, TimeSpan timeout)
        {
            string res = GetGeoNameManagerInstance().GetPlaceName(timeSpan, timeout);
            return res;
        }

        public void RemovePlaceByGeoCode(double latitude, double longitude)
        {
            GetGeoNameManagerInstance().RemovePlaceByGeoCode(latitude, longitude);
        }

        public GeoName NearestPlace(double latitude, double longitude)
        {
            GeoName res = GetGeoNameManagerInstance().NearestPlace(latitude, longitude);
            return res;
        }

        public string NearestPlaceName(double latitude, double longitude)
        {
            string res = GetGeoNameManagerInstance().NearestPlaceName(latitude, longitude);
            return res;
        }

        public void AddPlace(double latitude, double longitude, string name,
            DateTime dateTime, string countryCode = "IN", TimeZone timeZone = null, int population = 100,
            bool anyLocality = false, bool busStop = false, bool bank = false,
            bool atm = false, bool ground = false, bool hill = false,
            bool headland = false, bool reservoir = false, bool beach = false)
        {
            GetGeoNameManagerInstance().AddPlace(latitude, longitude, name,
            dateTime, countryCode, timeZone, population,
            anyLocality, busStop, bank,
            atm, ground, hill,
            headland, reservoir, beach);
        }
    }
}
