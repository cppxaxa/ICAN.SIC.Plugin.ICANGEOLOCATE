using GeographicLocationProviders;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GeoSharp
{
    public class GeoNameManager
    {
        private string fname = null;
        private string customDb = null;
        public ReverseGeoCode reverseGeoCode;
        public ReverseGeoCode customReverseGeoCode;
        private GeoNameManagerUtility utility;

        private IGeographicLocationProvider m_locationProvider = null;
        private IGeographicLocationProvider locationProvider
        {
            get
            {
                if (m_locationProvider == null)
                    throw new Exception("Kindly set geographic location provider by calling SetGeographicLocationProvider(locationProvider)");

                return m_locationProvider;
            }
            set
            {
                m_locationProvider = value;
            }
        }

        event Action RefreshCompleted = null;

        public GeoNameManager()
        {
            utility = new GeoNameManagerUtility();
        }

        public void Initialize(string fname, string customDb)
        {
            this.fname = fname;
            this.customDb = customDb;

            Parallel.For(0, 2, (i) =>
            {
                if (i % 2 == 0)
                    utility.Initialize(ref customReverseGeoCode, customDb);
                else
                    utility.Initialize(ref reverseGeoCode, fname);
            });
        }

        public GeoCoordinates GetCurrentCoordinates()
        {
            return GetCurrentCoordinates(new TimeSpan(0, 0, 10), new TimeSpan(0, 0, 10));
        }

        public GeoCoordinates GetCurrentCoordinates(TimeSpan timeSpan, TimeSpan timeout)
        {
            GeoCoordinates loc = this.locationProvider.GetCurrentLocation(timeSpan, timeout);
            return loc;
        }

        public void AddCurrentPlace(string name,
            DateTime dateTime, string countryCode = "IN", TimeZone timeZone = null, int population = 100,
            bool anyLocality = false, bool busStop = false, bool bank = false,
            bool atm = false, bool ground = false, bool hill = false,
            bool headland = false, bool reservoir = false, bool beach = false)
        {
            AddCurrentPlace(new TimeSpan(0, 0, 10), new TimeSpan(0, 0, 10), name, dateTime, countryCode, timeZone, population,
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
            GeoCoordinates loc = this.locationProvider.GetCurrentLocation(timeSpan, timeout);

            AddPlace(loc.Latitude, loc.Longitude, name, dateTime, countryCode, timeZone, population,
            anyLocality, busStop, bank,
            atm, ground, hill,
            headland, reservoir, beach);
        }

        public GeoName GetPlace()
        {
            return GetPlace(new TimeSpan(0, 0, 10), new TimeSpan(0, 0, 10));
        }

        public string GetPlaceName()
        {
            return GetPlace().Name;
        }

        public GeoName GetPlace(TimeSpan timeSpan, TimeSpan timeout)
        {
            GeoCoordinates loc = this.locationProvider.GetCurrentLocation(timeSpan, timeout);

            return this.NearestPlace(loc.Latitude, loc.Longitude);
        }

        public string GetPlaceName(TimeSpan timeSpan, TimeSpan timeout)
        {
            return GetPlace(timeSpan, timeout).Name;
        }

        public void SetGeographicLocationProvider(IGeographicLocationProvider locationProvider)
        {
            this.locationProvider = locationProvider;
        }

        public void RemovePlaceByGeoCode(double lat, double lon)
        {
            if (customDb == null)
                throw new Exception("Kindly initialize GeoNameManager by calling Initialize(dbFilePath, customDb)");

            utility.RemovePlaceByGeoCode(customDb, customReverseGeoCode, lat, lon);
            utility.Initialize(ref customReverseGeoCode, customDb);
        }

        public void AddPlace(double lat, double lon, string name,
            DateTime dateTime, string countryCode = "IN", TimeZone timeZone = null, int population = 100,
            bool anyLocality = false, bool busStop = false, bool bank = false,
            bool atm = false, bool ground = false, bool hill = false,
            bool headland = false, bool reservoir = false, bool beach = false)
        {
            if (fname == null)
                throw new Exception("Kindly initialize GeoNameManager by calling Initialize(dbFilePath)");

            utility.AddPlace(customDb, lat, lon, name,
            dateTime, countryCode, timeZone, population,
            anyLocality, busStop, bank,
            atm, ground, hill,
            headland, reservoir, beach);

            utility.Initialize(ref customReverseGeoCode, customDb);
        }

        public void Refresh()
        {
            if (fname == null || customDb == null)
                throw new Exception("Kindly initialize GeoNameManager by calling Initialize(dbFilePath, customDb)");

            utility.Initialize(ref reverseGeoCode, fname);
            utility.Initialize(ref customReverseGeoCode, customDb);

            if (RefreshCompleted != null)
                RefreshCompleted.Invoke();
        }

        public string NearestPlaceName(double Latitude, double Longitude)
        {
            return NearestPlace(Latitude, Longitude).Name;
        }

        public GeoName NearestPlace(double Latitude, double Longitude)
        {
            GeoName a = reverseGeoCode.NearestPlace(Latitude, Longitude);
            GeoName b = customReverseGeoCode.NearestPlace(Latitude, Longitude);

            if (a.Latitude == 0 && a.Longitude == 0)
                return b;
            else if (b.Latitude == 0 && b.Longitude == 0)
                return a;

            GeoName req = new GeoName(Latitude, Longitude);

            double distA = a.SquaredDistance(req);
            double distB = b.SquaredDistance(req);

            if (distA < distB)
                return a;
            else
                return b;
        }
    }
}