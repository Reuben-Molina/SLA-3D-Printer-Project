using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DPrinterSim
{
    //Bob The Builder
    public class PrinterCommand
    {
        public double g_command { get; set; }
        public double z_layer { get; set; }

        public double x_coordinate { get; set; }

        public double y_coordinate { get; set; }

        public bool isLaserOn { get; set; }
    }
}
