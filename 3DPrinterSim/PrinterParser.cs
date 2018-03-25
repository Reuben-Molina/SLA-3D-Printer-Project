using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DPrinterSim
{
    public class PrinterParser
    {
        //datamemeber fileStream
        //Open Method
        //Close Method
        //ParseContents Method

        //get the file from user with openfiledialog 
        //pass filepath to Openfile (opens file stream)
        //Call ParseContents on file contents
        //ParseContents returns list of printerCommand objects ready for translation to Firmware Packets
        //if file is done or canceled call close method

        public StreamReader fileStream;
        public void OpenFile(string filepath)
        {
            fileStream = new StreamReader(filepath);
            ParseContents();
        }
        public void CloseFile()
        {
            fileStream.Close();
        }

        private List<PrinterCommand> ParseContents()
        {
            var printerCommands = new List<PrinterCommand>();

            string line;

            while ((line = fileStream.ReadLine()) != null)
            {
                if (line[0] != 'G') continue;

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
                                individualCommand.isLaserOn = false;
                            }
                            break;
                    }
                    printerCommands.Add(individualCommand);
                }
            }
            return printerCommands;
        }
    }
}
