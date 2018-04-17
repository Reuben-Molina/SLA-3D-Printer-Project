using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardware;
using Firmware;

namespace Hardware
{

    class HostCP

    {
        PrinterThread host;

        public HostCP(PrinterThread hostControl)
        {
            host = hostControl;

        }

        private string WaitForEndResponse() // Reads the firmwares response to the parameter data.
        {
            while (true)
            {

                byte[] byteResponse = new byte[1];
                string stringResponse = "";
                int response;

                do // Reads the bytes one at a time and then converts them into a string
                {
                    response = host.GetPrinterSim().ReadSerialFromFirmware(byteResponse, 1);
                    if (byteResponse != null)
                        if (response != 0)
                            string.Concat(stringResponse, System.Text.Encoding.ASCII.GetString(byteResponse));
                    else
                        break;
                } while (true);


                return stringResponse;


            }
        }

        private bool ConfirmCorrectHeader(byte[] bytesToWrite, byte[] bytesToRead)
        {
            int serialReceived = 0;
            byte[] headerBytes = new byte[4];
            Array.Copy(bytesToWrite, headerBytes, 4);

            if (headerBytes.SequenceEqual(bytesToRead)) // Confirms the Firmware has recieved the correct header
            {
                byte[] ack = { 0xA5 };
                host.GetPrinterSim().WriteSerialToFirmware(ack, 1);
                return true;
            }
            else //If not then we will resend the header bytes again
            {
                byte[] nak = { 0xFF };
                host.GetPrinterSim().WriteSerialToFirmware(nak, 1);
                host.GetPrinterSim().WriteSerialToFirmware(bytesToWrite, 4);
                serialReceived = 0;
                do
                {
                    serialReceived = host.GetPrinterSim().ReadSerialFromFirmware(bytesToRead, 4);
                } while (serialReceived == 0);

                return false;
            }
        }

        public string SendPacket(byte[] bytesToWrite)
        {
            //Sends initial header bytes to Firmware
            host.GetPrinterSim().WriteSerialToFirmware(bytesToWrite, 4);
            int serialReceived = 0;
            byte[] bytesToRead = new byte[4];
            do // Waits for a response from Firmware. Make this a method Wait()
            {
                serialReceived = host.GetPrinterSim().ReadSerialFromFirmware(bytesToRead, 4);
            } while (serialReceived == 0);

            bool flag;

            do
            {
                flag = ConfirmCorrectHeader(bytesToWrite, bytesToRead);

            } while (!flag);

            byte[] commandBytes = new byte[bytesToWrite.Length - 4];
            Array.Copy(bytesToWrite, 4, commandBytes, 0, bytesToWrite.Length - 4); // New byte Array containing the parameter data

            host.GetPrinterSim().WriteSerialToFirmware(commandBytes, commandBytes.Length); //Sends the rest of the packet

            return (WaitForEndResponse());

        }
    }
}
