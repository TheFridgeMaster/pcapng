using System;
using PcapngUtils.PcapNG;
using System.Threading;
using PcapngUtils.Common;

namespace PcapNG_Document_Extractor
{
    class Program
    {
        static void OpenPcapNGFile(string filename, bool swapBytes)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            using (var reader = new PcapNGReader(filename, swapBytes))
            {
                reader.OnReadPacketEvent += Reader_OnReadPacketEvent;
                reader.ReadPackets(token);
                reader.OnReadPacketEvent -= Reader_OnReadPacketEvent;
            }
        }

        static void Reader_OnReadPacketEvent(object context, IPacket packet)
        {
            var pants = "";
            int count = 1474;
            //Console.WriteLine();
            foreach (byte item in packet.Data)
            {
                pants += item.ToString() + System.Environment.NewLine;
            } 
            //Console.WriteLine(string.Format("Src IP: {0}.{1}.{2}.{3} - Dst IP: {4}.{5}.{6}.{7} - {8} {9} {10} {11} {12} {13} {14} {15}", packet.Data.GetValue(5), packet.Data.GetValue(6), packet.Data.GetValue(7), packet.Data.GetValue(8), packet.Data.GetValue(9), packet.Data.GetValue(10), packet.Data.GetValue(11), packet.Data.GetValue(12), packet.Data.GetValue(13), packet.Data.GetValue(14), packet.Data.GetValue(15), packet.Data.GetValue(16), packet.Data.GetValue(17), packet.Data.GetValue(18), packet.Data.GetValue(19), packet.Data.GetValue(20)));
            Console.WriteLine(pants);
        }
        static void Main(string[] args)
        {
            OpenPcapNGFile(@"c:\users\Fridge\packetsbro.pcapng", false);    
        }
    }
}