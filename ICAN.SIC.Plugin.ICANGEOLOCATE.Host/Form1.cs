using ICAN.SIC.Abstractions.IMessageVariants;
using ICAN.SIC.Plugin.ICANGEOLOCATE.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICAN.SIC.Plugin.ICANGEOLOCATE.Host
{
    public partial class Form1 : Form
    {
        ICANGEOLOCATE ICANGEOLOCATE;

        public Form1()
        {
            this.ICANGEOLOCATE = new ICANGEOLOCATE();
            this.ICANGEOLOCATE.Hub.Subscribe<IMachineMessage>(this.ShowMachineMessage);

            InitializeComponent();
        }

        private void ShowMachineMessage(IMachineMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("MachineMessage: " + message.Message);
            Console.ResetColor();
        }

        private void btnCallGeoRequest_Click(object sender, EventArgs e)
        {
            string inText = txtInput.Text;

            GeoLocationRequest req = new GeoLocationRequest { CommandText = inText };
            this.ICANGEOLOCATE.Hub.PublishDownwards<IGeoLocationRequest>(req);
        }
    }
}
