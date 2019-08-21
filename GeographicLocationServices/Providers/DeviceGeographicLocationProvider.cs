using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeographicLocationProviders
{
    public class DeviceGeographicLocationProvider : AbstractGeographicLocationProvider, IGeographicLocationProvider
    {
        private string latitude;
        private string longitute;
        private GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

        public DeviceGeographicLocationProvider()
        {
            watcher = new GeoCoordinateWatcher();
            watcher.StatusChanged += Watcher_StatusChanged;
            watcher.Start();

            NeedsRefresh = false;
        }

        private void Watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            try
            {
                if (e.Status == GeoPositionStatus.Ready)
                {
                    // Display the latitude and longitude.  
                    if (watcher.Position.Location.IsUnknown)
                    {
                        latitude = "0";
                        longitute = "0";
                    }
                    else
                    {
                        latitude = watcher.Position.Location.Latitude.ToString();
                        longitute = watcher.Position.Location.Longitude.ToString();

                        SetLocation(double.Parse(latitude), double.Parse(longitute));
                    }
                }
                else
                {
                    latitude = "0";
                    longitute = "0";
                }
            }
            catch (Exception)
            {
                latitude = "0";
                longitute = "0";
            }
        }
    }
}
