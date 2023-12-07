using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UDP_Socket_stone_scissors_paper
{
    public class Connections
    {
        Socket _socket;
        EndPoint _remotePoint;

        //public Connections()
        //{
            
        //}

        public Socket GetSocket {  get { return _socket; } }
        public EndPoint GetEndPoint { get { return _remotePoint; } }
        public void SocketClose()
        {
            if (_socket != null && _socket.Connected)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
        }
        public void ConnectTo(string ipAddress, int port) 
        {
            SocketClose();

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _remotePoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        }
        public async Task<string> Send(string str)
        {
            try
            {
                string message = str;
                byte[] data = new byte[256];
                data = Encoding.UTF8.GetBytes(message);
                await _socket.SendToAsync(data, SocketFlags.None, _remotePoint);

                data = new byte[256];
                var result = await _socket.ReceiveFromAsync(data, SocketFlags.None, _remotePoint);
                message = Encoding.UTF8.GetString(data, 0, result.ReceivedBytes);

                return message;
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }

            return string.Empty;
        }
    }
}
