using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Hardware;
using Firmware;
using System.IO;
using _3DPrinterSim;

namespace PrinterSimulator
{
    class PrintSim
    {
        public System.IO.StreamReader file = new System.IO.StreamReader("..\\..\\..\\SampleSTLs\\F-35_Corrected.gcode");

        
        static void PrintFile(PrinterControl simCtl, PrinterThread printer)
        {
         

            Stopwatch swTimer = new Stopwatch();
            swTimer.Start();
            

            swTimer.Stop();
            long elapsedMS = swTimer.ElapsedMilliseconds;

            Console.WriteLine("Total Print Time: {0}", elapsedMS / 1000.0);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        } 

        [STAThread]

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        static void Main()
        {

            IntPtr ptr = GetConsoleWindow();
            MoveWindow(ptr, 0, 0, 1000, 400, true);

            // Start the printer - DO NOT CHANGE THESE LINES
            PrinterThread printer = new PrinterThread();
            Thread oThread = new Thread(new ThreadStart(printer.Run));
            oThread.Start();
            printer.WaitForInit();

            // Start the firmware thread - DO NOT CHANGE THESE LINES
            FirmwareController firmware = new FirmwareController(printer.GetPrinterSim());
            oThread = new Thread(new ThreadStart(firmware.Start));
            oThread.Start();
            firmware.WaitForInit();

            SetForegroundWindow(ptr);

            bool fDone = false;
            while (!fDone)
            {
                Console.Clear();
                Console.WriteLine("3D Printer Simulation - Control Menu\n");
                Console.WriteLine("P - Print");
                Console.WriteLine("T - Test GCODE");
                Console.WriteLine("Q - Quit");

                char ch = Char.ToUpper(Console.ReadKey().KeyChar);
                switch (ch)
                {
                    case 'P': // Print
                        PrintFile(printer.GetPrinterSim(), printer);
                        break;

                    case 'T': // Test GCODE
                        PrinterParser.OpenFile("C:\\Users\\User\\Documents\\GitKraken\\SLAPrintingProject\\SLA-3D-Printer-Project\\SampleSTLs\\F-35_Corrected.gcode");
                        List<PrinterCommand> FileContents = PrinterParser.ParseContents();
                        double max_x_coordinate = 0.0;
                        double max_y_coordinate = 0.0;
                        double max_z_layer = 0.0;
                        for (int i = 0; i < FileContents.Count(); i++)
                        {
                            //Console.WriteLine(FileContents[i].x_coordinate);
                            //Console.WriteLine(FileContents[i].y_coordinate);
                            //Console.WriteLine("Line:" + i);
                            
                            

                            if (FileContents[i].g_command != double.MaxValue)
                            { Console.WriteLine("Command: " + FileContents[i].g_command); }
                            if (FileContents[i].x_coordinate != double.MaxValue)
                            {   
                                if (FileContents[i].x_coordinate > max_x_coordinate)
                                {
                                    max_x_coordinate = FileContents[i].x_coordinate;
                                }
                                Console.WriteLine("X: " + FileContents[i].x_coordinate); }
                            if (FileContents[i].y_coordinate != double.MaxValue)
                            {
                                if (FileContents[i].y_coordinate > max_y_coordinate)
                                {
                                    max_y_coordinate = FileContents[i].y_coordinate;
                                }
                                Console.WriteLine("Y: " + FileContents[i].y_coordinate); }
                            if (FileContents[i].z_layer != double.MaxValue)
                            {
                                if (FileContents[i].z_layer > max_z_layer)
                                {
                                    max_z_layer = FileContents[i].z_layer;
                                }
                                Console.WriteLine("Z: " + FileContents[i].z_layer); }
                            //if (FileContents[i].isLaserOn == true)
                            //{ Console.WriteLine("Is Laser On? " + FileContents[i].isLaserOn); }
                            Console.WriteLine("Is Laser On? " + FileContents[i].isLaserOn);
                            Console.WriteLine("");
                             
                        }
                        Console.WriteLine("Max x coordinate: " + max_x_coordinate);
                        Console.WriteLine("");
                        Console.WriteLine("Max y coordinate: " + max_y_coordinate);
                        Console.WriteLine("");
                        Console.WriteLine("Max z layer: " + max_z_layer);
                        Console.WriteLine("");
                        Console.ReadLine();
                        break;

                    case 'Q' :  // Quit
                        printer.Stop();
                        firmware.Stop();
                        fDone = true;
                        break;

                }

            }

        }
    }
}