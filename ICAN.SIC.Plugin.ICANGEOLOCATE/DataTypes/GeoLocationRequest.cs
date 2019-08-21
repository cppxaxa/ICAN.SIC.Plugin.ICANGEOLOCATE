using ICAN.SIC.Abstractions.IMessageVariants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ICANGEOLOCATE.DataTypes
{
    public class GeoLocationRequest : IGeoLocationRequest
    {
        string commandText;

        public string CommandText { get => commandText; set => commandText = value; }
    }
}
