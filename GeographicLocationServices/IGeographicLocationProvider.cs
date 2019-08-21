using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeographicLocationProviders
{
    public class GeoCoordinates
    {
        public double Longitude;
        public double Latitude;

        public GeoCoordinates()
        {

        }

        public GeoCoordinates(double lat, double lon)
        {
            this.Latitude = lat;
            this.Longitude = lon;
        }
    }

    public interface IGeographicLocationProvider
    {
        GeoCoordinates GetCurrentLocation(TimeSpan timeSpan, TimeSpan timeout);
        GeoCoordinates GetLastKnownLocation();

        bool IsDataAvailable { get; }
        bool IsUpdatedDataAvailable { get; }
    }
}
