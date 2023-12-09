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
    public class Connections
    {
        Socket _socketSend;
        Socket _socketReceive;
        EndPoint _remotePoint;
        EndPoint _localPoint;

        //public Socket GetSocket {  get { return _socket; } }
        public EndPoint GetRemotePoint { get { return _remotePoint; } }
        public EndPoint GetLocalPoint { get { return _localPoint; } }
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
        public void ConnectTo(string ipAddress, int localPort, int remotePort)
        {
            try
            {
                SocketClose();

                _socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _socketReceive = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                
                _remotePoint = new IPEndPoint(IPAddress.Parse(ipAddress), remotePort);
                _localPoint = new IPEndPoint(IPAddress.Parse(ipAddress), localPort);

                _socketReceive.Bind(_localPoint);
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
        //public async Task<string> SendToPlayer(string str)
        //{
        //    try
        //    {
        //        string message = str;
        //        byte[] data = new byte[256];
        //        data = Encoding.UTF8.GetBytes(message);
        //        await _socket.SendToAsync(data, SocketFlags.None, _remotePoint);

        //        data = new byte[256];
        //        var result = await _socket.ReceiveFromAsync(data, SocketFlags.None, _localPoint);
        //        message = Encoding.UTF8.GetString(data, 0, result.ReceivedBytes);

        //        return message;
        //    }
        //    catch (SocketException ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //    return string.Empty;
        //}
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
    }
}
