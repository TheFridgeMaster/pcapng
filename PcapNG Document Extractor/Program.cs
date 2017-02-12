using System;
using System.Linq;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;

namespace PcapNG_Document_Extractor
{
    class Program
    {
        private static void DispatcherHandler(Packet packet)
        {
            // print packet timestamp and packet length
            Console.WriteLine(packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff") + " length:" + packet.Length);

            // Print the packet
            const int LineLength = 64;
            for (int i = 0; i != packet.Length; ++i)
            {
                Console.Write((packet[i]).ToString("X2"));
                if ((i + 1) % LineLength == 0)
                    Console.WriteLine();
            }

            Console.WriteLine();

        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private static void PacketHandler(Packet packet)
        {
            // print timestamp and length of the packet
            //Console.WriteLine(packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff") + " length:" + packet.Length);

            IpV4Datagram ip = packet.Ethernet.IpV4;

            // header and footer hexadecimal values for pdf filetype
            string docHeader = "25504446";
            string docFooter = "0D2525454F460D";

            bool? capturePacket = null;
            //bool stopCapture = false;

            if (ip.Payload.ToHexadecimalString().Contains(docHeader))
            {
                capturePacket = true;
            }

            var datastream = "";

            if (capturePacket == true)
            {       
                    //datastream += ip.Payload.ToHexadecimalString();
                    Console.WriteLine(ip.Payload.ToHexadecimalString());
            }
            else if (ip.Payload.ToHexadecimalString().Contains(docFooter))
            {
                capturePacket = false;
                //System.IO.File.WriteAllBytes("test.pdf", StringToByteArray(datastream));
            }

            //Console.WriteLine(ip.Payload.ToHexadecimalString());
            // print raw data content and write to pdf file
            //Console.WriteLine(datastream);
            //Console.WriteLine(result);
            //Console.WriteLine(ip.Payload.ToHexadecimalString());
        }
       
        static void Main(string[] args)
        {
            //var hex = "";
            if (args.Length != 1)
            {
                Console.WriteLine("usage: " + Environment.GetCommandLineArgs()[0] + " <filename>");
                return;
            }
            // Create the offline device
            OfflinePacketDevice selectedDevice = new OfflinePacketDevice(args[0]);

            // Open the capture file
            using (PacketCommunicator communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                communicator.SetFilter("tcp");
                // start the capture
                var i = 0;
                communicator.ReceiveSomePackets(out i, 1000, PacketHandler);
                Console.WriteLine(i);
                //communicator.ReceivePackets(0, PacketHandler);
            }
            /*string docHeader = "25504446";
            string docFooter = "0D0A2525454F460D0A";
            int pFrom = hex.IndexOf(docHeader) + docHeader.Length;
            int pTo = hex.LastIndexOf(docFooter);
            String result2 = hex.Substring(pFrom, pTo - pFrom);*/
        }

    }
}