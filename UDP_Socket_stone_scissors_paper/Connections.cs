using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace UDP_Socket_stone_scissors_paper
{
    public delegate void ResponceDelegat(string str);
    public class Connections
    {
        Socket _socketSend;
        Socket _socketReceive;
        EndPoint _remotePoint;
        EndPoint _localPoint;
        Socket _socketSend_2;
        Socket _socketReceive_2;
        EndPoint _remotePoint_2;
        EndPoint _localPoint_2;
        public event ResponceDelegat ResponceEvent = null;

        public void SocketClose()
        {
            if (_socketSend != null && _socketSend.Connected)
            {
                _socketSend.Shutdown(SocketShutdown.Both);
                _socketSend.Close();
            }

            if (_socketReceive != null && _socketReceive.Connected)
            {
                _socketReceive.Shutdown(SocketShutdown.Both);
                _socketReceive.Close();
            }
        }
        public void ConnectTo(string ipAddress, int port) 
        {
            try
            {
                SocketClose();

                _socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _remotePoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ConnectTo(string ipAddress, int localPort, int remotePort, int localPort_2, int remotePort_2)
        {
            try
            {
                SocketClose();

                _socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _socketReceive = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                
                _remotePoint = new IPEndPoint(IPAddress.Parse(ipAddress), remotePort);
                _localPoint = new IPEndPoint(IPAddress.Parse(ipAddress), localPort);

                _socketReceive.Bind(_localPoint);

                _socketSend_2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _socketReceive_2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                _remotePoint_2 = new IPEndPoint(IPAddress.Parse(ipAddress), remotePort_2);
                _localPoint_2 = new IPEndPoint(IPAddress.Parse(ipAddress), localPort_2);

                _socketReceive_2.Bind(_localPoint_2);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task<string> Send(string str)
        {
            try
            {
                string message = str;
                byte[] data = new byte[256];
                data = Encoding.UTF8.GetBytes(message);
                await _socketSend.SendToAsync(data, SocketFlags.None, _remotePoint);

                data = new byte[256];
                var result = await _socketSend.ReceiveFromAsync(data, SocketFlags.None, _remotePoint);
                message = Encoding.UTF8.GetString(data, 0, result.ReceivedBytes);

                return message;
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }

            return string.Empty;
        }
        public async Task<string> ReceiveMessageAsync()
        {
            byte[] data = new byte[256];
            var result = await _socketReceive.ReceiveFromAsync(data, SocketFlags.None, _localPoint);
            string message = Encoding.UTF8.GetString(data, 0, result.ReceivedBytes);

            return message;
        }
        public async Task SendMessageAsync(string str)
        {
            string message = str;
            byte[] data = new byte[256];
            data = Encoding.UTF8.GetBytes(message);
            await _socketSend.SendToAsync(data, SocketFlags.None, _remotePoint);
        }
        public async Task ReceiveMessageAsync_2()
        {
            while (true)
            {
                byte[] data = new byte[256];
                var result = await _socketReceive_2.ReceiveFromAsync(data, SocketFlags.None, _localPoint_2);
                string message = Encoding.UTF8.GetString(data, 0, result.ReceivedBytes);

                Responce(message);
            }
        }
        public async Task SendMessageAsync_2(string str)
        {
            string message = str;
            byte[] data = new byte[256];
            data = Encoding.UTF8.GetBytes(message);
            await _socketSend_2.SendToAsync(data, SocketFlags.None, _remotePoint_2);
        }
        void Responce(string str)
        {
            if (ResponceEvent != null)
            {
                ResponceEvent(str);
            }
        }
    }
}
