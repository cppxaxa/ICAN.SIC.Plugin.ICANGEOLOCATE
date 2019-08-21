using System;
using System.IO;

namespace GeoSharp
{
    internal class GeoNameManagerUtility
    {
        public void RemovePlaceByGeoCode(string fname, ReverseGeoCode reverseGeoCode, double lat, double lon)
        {
            var geoCode = reverseGeoCode.NearestPlace(lat, lon);

            int lineNumInDb = geoCode.DbLineNumber;

            using (var outFile = new FileInfo(fname + ".tmp").Open(FileMode.Create))
            using (var writer = new StreamWriter(outFile))
            using (var f = new FileInfo(fname).Open(FileMode.Open))
            using (var reader = new StreamReader(f))
            {
                string line;
                int lineNumber = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    if (lineNumInDb == lineNumber)
                        Console.WriteLine("[INFO] Deleted Entry: " + line);
                    else
                        writer.WriteLine(line);

                    lineNumber++;
                }
            }

            File.Replace(fname + ".tmp", fname, fname + ".bak");
            File.Delete(fname + ".bak");
        }

        public void Initialize(ref ReverseGeoCode reverseGeoCode, string fname)
        {
            using (var db = new FileStream(fname, FileMode.Open))
            {
                reverseGeoCode = new ReverseGeoCode(db, false);
            }
        }

        public void AddPlace(string fname, double lat, double lon, string name,
            DateTime dateTime, string countryCode = "IN", TimeZone timeZone = null, int population = 100,
            bool anyLocality = false, bool busStop = false, bool bank = false, 
            bool atm = false, bool ground = false, bool hill = false, 
            bool headland = false, bool reservoir = false, bool beach = false)
        {
            string content = string.Empty;

            string locationType = "PPLX";
            if (anyLocality)
                locationType = "LCTY";
            else if (busStop)
                locationType = "BUSTP";
            else if (bank)
                locationType = "BANK";
            else if (atm)
                locationType = "ATM";
            else if (ground)
                locationType = "HMCK";
            else if (hill)
                locationType = "HLL";
            else if (headland)
                locationType = "HDLD";
            else if (reservoir)
                locationType = "RSV";
            else if (beach)
                locationType = "PRMN";

            string dateOfAddition = dateTime.Year.ToString("0000") + "-" + dateTime.Month.ToString("00") + "-" + dateTime.Day.ToString("00");

            if (timeZone == null)
                timeZone = TimeZone.CurrentTimeZone;
            string timeZoneStr = "";
            timeZoneStr = timeZone.StandardName;

            if (timeZoneStr == "India Standard Time")
                timeZoneStr = "Asia/Kolkata";

            content = $"8000000\t{name}\t{name}\t\t{lat}\t{lon}\tP\t{locationType}\t{countryCode}\t\t0\t0\t\t\t{population}\t\t100\t{timeZoneStr}\t{dateOfAddition}";

            File.AppendAllText(fname, content + "\r\n");
        }
    }
}