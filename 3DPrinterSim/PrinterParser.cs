using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3DPrinterSim;

namespace _3DPrinterSim
{
    public class PrinterParser
    {
        //datamemeber fileStream
        //Open Method
        //Close Method
        //ParseContents Method
        //Test file from main menu

        //get the file from user with openfiledialog 
        //pass filepath to Openfile (opens file stream)
        //Call ParseContents on file contents
        //ParseContents returns list of printerCommand objects ready for translation to Firmware Packets
        //if file is done or canceled call close method

        public static StreamReader FileStream;

        public static void OpenFile(string filepath)
        {


            FileStream = new StreamReader(filepath);
        }
        public static void CloseFile()
        {
            FileStream.Close();
        }

        public static List<PrinterCommand> ParseContents()
        {
            var printerCommands = new List<PrinterCommand>();

            string line;

            while ((line = FileStream.ReadLine()) != null)
            {
                //Console.WriteLine(line);
                if (string.IsNullOrWhiteSpace(line) || line.First() != 'G') continue;

                string[] splitLine = line.Split(null);
                PrinterCommand individualCommand = new PrinterCommand();
                individualCommand.g_command = double.MaxValue;
                individualCommand.z_layer = double.MaxValue;
                individualCommand.x_coordinate = double.MaxValue;
                individualCommand.y_coordinate = double.MaxValue;
                individualCommand.x_coordinate = double.MaxValue;
                individualCommand.isLaserOn = false;
                foreach (var part in splitLine)
                {
                    if (part.Contains(';')) break;
                    if (string.IsNullOrEmpty(part)) continue;
                   
                    char firstChar = part[0];
                   
                    switch (firstChar)
                    { 
                        case 'G':
                            individualCommand.g_command = Convert.ToDouble(part.Substring(1));
                            break;
                        case 'Z':
                            individualCommand.z_layer = Convert.ToDouble(part.Substring(1));
                            break;
                        case 'X':
                            individualCommand.x_coordinate = Convert.ToDouble(part.Substring(1));
                            break;
                        case 'Y':
                            individualCommand.y_coordinate = Convert.ToDouble(part.Substring(1));
                            break;
                        case 'E':
                            if (Convert.ToDouble(part.Substring(1)) != 0)
                            {
                                individualCommand.isLaserOn = true;
                            }
                            else
                            {
                                //individualCommand.isLaserOn = false;
                            }
                            break;
                    }
                    
                }
                printerCommands.Add(individualCommand);
            }
            return printerCommands;
        }
    }
}
