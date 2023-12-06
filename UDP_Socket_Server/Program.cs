using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Socket_Server
{
    internal class Program
    {
        Random _random = new Random();
        List<string> _response = new List<string>();

        Program()
        {
            _response.Add("stone");
            _response.Add("scissors");
            _response.Add("paper");
        }
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

                    string message = _response[_random.Next(0, 3)];
                    data = Encoding.UTF8.GetBytes(message);
                    await server.SendToAsync(data, SocketFlags.None, result.RemoteEndPoint);

                    Console.WriteLine(result.RemoteEndPoint + ": " + message);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }
    }
}