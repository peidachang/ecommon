﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ECommon.IoC;
using ECommon.Logging;

namespace ECommon.Socketing
{
    public class ClientSocket
    {
        private Socket _socket;
        private SocketService _socketService;
        private ILogger _logger;

        public ClientSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socketService = new SocketService(null);
            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(GetType().Name);
        }

        public ClientSocket Connect(string address, int port)
        {
            _socket.Connect(new IPEndPoint(IPAddress.Parse(address), port));
            return this;
        }
        public ClientSocket Start(Action<byte[]> replyMessageReceivedCallback)
        {
            Task.Factory.StartNew(() =>
            {
                _socketService.ReceiveMessage(new SocketInfo(_socket), reply =>
                {
                    try
                    {
                        replyMessageReceivedCallback(reply);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                    }
                });
            });
            return this;
        }
        public ClientSocket Shutdown()
        {
            _socket.Shutdown(SocketShutdown.Send);
            _socket.Close();
            return this;
        }
        public ClientSocket SendMessage(byte[] messageContent, Action<SendResult> messageSendCallback)
        {
            _socketService.SendMessage(_socket, messageContent, messageSendCallback);
            return this;
        }
    }
}
