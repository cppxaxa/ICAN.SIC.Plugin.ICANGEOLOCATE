using GeographicLocationProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GeoSharp
{
    class Program
    {
        public static void Main(string[] args)
        {
            IGeographicLocationProvider locationSevice = new DeviceGeographicLocationProvider();
            //GeoCoordinates location = locationSevice.GetCurrentLocation(TimeSpan.MaxValue, new TimeSpan(0, 0, 10));

            //Console.WriteLine("Latitude, Longitude " + location.Latitude + "," + location.Longitude);
            //Console.ReadLine();

            GeoNameManager manager = new GeoNameManager();
            manager.Initialize(@"GeoResources\GeoLocateGeoNamesDb.tsv", @"GeoResources\CustomGeoNamesDb.tsv");
            manager.SetGeographicLocationProvider(locationSevice);

            //manager.AddPlace(10, 11, "SNR Flat", DateTime.Now);
            manager.AddCurrentPlace("SNR Flat", DateTime.Now);

            var place = manager.GetPlace();
            Console.WriteLine(manager.GetPlace().Name);

            manager.RemovePlaceByGeoCode(place.Latitude, place.Longitude);

            Console.ReadLine();
            return;

            //ReverseGeoCode geocode = new ReverseGeoCode();
            //Console.WriteLine(geocode.NearestPlaceName(22.093285, 84.0315853));
            //Console.WriteLine(geocode.NearestPlaceName(22.093613, 84.0318423));

            //Console.WriteLine(manager.ReverseGeoCode.NearestPlaceName(22.093285, 84.0315853));

            Console.WriteLine("Start");

            manager.AddPlace(10, 11, "SNR Flat", DateTime.Now);
            Console.WriteLine(manager.NearestPlaceName(10.12, 11.5));

            manager.RemovePlaceByGeoCode(10.12, 11.5);
            Console.WriteLine(manager.NearestPlaceName(10.12, 11.5));

            manager.AddPlace(10, 11, "SNR Flat", DateTime.Now);
            Console.WriteLine(manager.NearestPlaceName(10.12, 11.5));

            manager.RemovePlaceByGeoCode(10.12, 11.5);
            Console.WriteLine(manager.NearestPlaceName(10.12, 11.5));

            Console.ReadLine();
        }
    }
}
