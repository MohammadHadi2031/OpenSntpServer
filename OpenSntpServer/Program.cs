using OpenSntpServer.NtpBase;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace OpenSntpServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await RunSntpServer();

            Console.ReadKey();
        }

        static async Task RunSntpServer()
        {
            UdpClient udpServer = new UdpClient(123);
            while (true)
            {
                var received = await udpServer.ReceiveAsync();
                var ntpServerPacket = new NtpServerPacket(received.Buffer); //Create the server packet
                await udpServer.SendAsync(ntpServerPacket.Bytes, NtpServerPacket.RESPONSE_SIZE, received.RemoteEndPoint); //Respond to the client request
                Debug.WriteLine($"Responded to {received.RemoteEndPoint.Address} with {ntpServerPacket.UTCTime.ToLocalTime()}"); //Log the transmitted time 
            }
        }
    }
}
