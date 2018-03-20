using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DPrinterSim
{
    //Bob The Builder
    class Command
    {
        public double z_layer { get; set; }
        public double x_coordinate { get; set; }
        public double y_coordinate { get; set; }
        public bool laser_state { get; set; }
    }
}
