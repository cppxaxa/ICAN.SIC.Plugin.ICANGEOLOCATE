using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeographicLocationProviders
{
    public abstract class AbstractGeographicLocationProvider : IGeographicLocationProvider
    {
        double lat, lon;
        DateTime lastUpdated;
        bool dataAvailable = false;
        bool dataUnread = false;

        protected bool NeedsRefresh = true;

        public AbstractGeographicLocationProvider()
        {
            lastUpdated = DateTime.Now;
            lat = 0;
            lon = 0;
        }

        protected void SetLocation(GeoCoordinates coordinates)
        {
            SetLocation(coordinates.Latitude, coordinates.Longitude);
        }

        protected void SetLocation(double lat, double lon)
        {
            this.lat = lat;
            this.lon = lon;

            lastUpdated = DateTime.Now;
            dataUnread = true;
            dataAvailable = true;
        }

        public bool IsDataAvailable { get => dataAvailable; }

        public bool IsUpdatedDataAvailable { get => dataUnread; }

        public GeoCoordinates GetCurrentLocation(TimeSpan timeSpan, TimeSpan timeout)
        {
            int waitingMillis = 500;
            TimeSpan timeInterval = new TimeSpan(0, 0, 0);

            while (!dataAvailable)
            {
                Thread.Sleep(500);
                timeInterval = timeInterval.Add(new TimeSpan(0, 0, 0, 0, waitingMillis));

                if (timeInterval >= timeout)
                    return new GeoCoordinates(lat, lon);
            }

            if (NeedsRefresh)
            {
                dataUnread = false;

                DateTime currentLastUpdated = lastUpdated;

                if (lastUpdated.Add(timeout) >= DateTime.Now)
                {
                    return new GeoCoordinates(lat, lon);
                }

                while (currentLastUpdated == lastUpdated)
                {
                    Thread.Sleep(waitingMillis);
                    timeInterval.Add(new TimeSpan(0, 0, 0, 0, waitingMillis));

                    if (timeInterval >= timeout)
                        break;
                }
            }

            return new GeoCoordinates(lat, lon);
        }

        public GeoCoordinates GetLastKnownLocation()
        {
            return new GeoCoordinates(lat, lon);
        }
    }
}
