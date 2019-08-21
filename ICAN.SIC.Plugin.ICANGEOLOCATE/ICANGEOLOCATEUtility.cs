using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAN.SIC.Abstractions;
using ICAN.SIC.Abstractions.ConcreteClasses;
using ICAN.SIC.Abstractions.IMessageVariants;

namespace ICAN.SIC.Plugin.ICANGEOLOCATE
{
    class ICANGEOLOCATEUtility
    {
        internal void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] " + message);
            Console.ResetColor();
        }

        internal void PushMessage(IHub hub, string message)
        {
            hub.Publish<IMachineMessage>(new MachineMessage("[ICANGEOLOCATE] " + message));
        }

        internal void PushError(string message)
        {
            ShowError(message);
        }
    }
}
