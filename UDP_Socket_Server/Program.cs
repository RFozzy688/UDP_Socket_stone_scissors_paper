using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Socket_Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Program program = new Program();

            await program.StartServer();
        }
        async Task StartServer()
        {
            using Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            EndPoint localIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            server.Bind(localIP);

            Console.WriteLine("UDP-server started...");

            byte[] data = new byte[256];

            EndPoint remoteIP = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                try
                {
                    var result = await server.ReceiveFromAsync(data, SocketFlags.None, remoteIP);
                    var message = Encoding.UTF8.GetString(data, 0, result.ReceivedBytes);

                    Console.WriteLine($"Получено {result.ReceivedBytes} байт");
                    Console.WriteLine($"Удаленный адрес: {result.RemoteEndPoint}");
                    Console.WriteLine(message);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }
    }
}