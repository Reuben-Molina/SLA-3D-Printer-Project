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
        //Open Method
        //Close Method
        //Filestream saved to property
        //ParseNextCommand Method

            public List<PrinterCommand> Parse(string path)
        {
            //get the file from user with openfiledialog 
            //
            //return ParseContents(file.contents());
            return ParseContents(File.ReadAllText(path));
        }

        private List<PrinterCommand> ParseContents(string fileContents)
        {
            var printerCommands = new List<PrinterCommand>();
            var lines = fileContents.Split('\n');
            foreach (var line in lines)
            {
                if (line[0] != 'G')
                {
                    continue;
                }
                string[] splitLine = line.Split(null);
                PrinterCommand individualCommand = new PrinterCommand();
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
