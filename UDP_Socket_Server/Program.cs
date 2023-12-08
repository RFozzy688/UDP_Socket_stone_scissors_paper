using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Socket_Server
{
    internal class Program
    {
        Random _random = new Random();
        List<string> _response = new List<string>();
        Dictionary<EndPoint, string> _players;
        int _counter = 0;

        public Program()
        {
            _response.Add("stone");
            _response.Add("scissors");
            _response.Add("paper");

            _players = new Dictionary<EndPoint, string>();
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

            EndPoint remoteIP = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                try
                {
                    string message;
                    byte[] data = new byte[256];
                    var result = await server.ReceiveFromAsync(data, SocketFlags.None, remoteIP);
                    message = Encoding.UTF8.GetString(data, 0, result.ReceivedBytes);

                    string[] temp = message.Split(' ');

                    if (temp[0].CompareTo("human") != 0)
                    {
                        message = _response[_random.Next(0, 3)];
                        data = Encoding.UTF8.GetBytes(message);
                        await server.SendToAsync(data, SocketFlags.None, result.RemoteEndPoint);

                        Console.WriteLine(result.RemoteEndPoint + ": " + message);
                    }
                    else
                    {
                        if (_players.Count != 2)
                        {
                            _players.Add(result.RemoteEndPoint, string.Empty);

                            if (_players.Count != 2) continue;

                            string str = "player connected";
                            data = new byte[256];
                            data = Encoding.UTF8.GetBytes(str);

                            foreach (var player in _players)
                            {
                                await server.SendToAsync(data, SocketFlags.None, player.Key);
                            }
                        }
                        else
                        {
                            _counter++;

                            _players[result.RemoteEndPoint] = temp[1];

                            if (_counter == 2)
                            {
                                data = new byte[256];
                                data = Encoding.UTF8.GetBytes(_players.ElementAt(0).Value);
                                await server.SendToAsync(data, SocketFlags.None, _players.ElementAt(1).Key);
                                Console.WriteLine(_players.ElementAt(1).Key + " " + _players.ElementAt(0).Value);

                                data = new byte[256];
                                data = Encoding.UTF8.GetBytes(_players.ElementAt(1).Value);
                                await server.SendToAsync(data, SocketFlags.None, _players.ElementAt(0).Key);
                                Console.WriteLine(_players.ElementAt(0).Key + " " + _players.ElementAt(1).Value);

                                _counter = 0;
                            }
                        }
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }
    }
}