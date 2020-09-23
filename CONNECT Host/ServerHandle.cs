using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CONNECTServerSide
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now user {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }

            SerialCommandsSend.InitializeSerialCommunication();
            Server.clients[_fromClient].SentIntoChat(_fromClient, _username);
        }

        public static void UserMessage(int _fromClient, Packet _packet)
        {
            int _toClientId = _packet.ReadInt();
            string _command = _packet.ReadString();

            Console.WriteLine($"[{_fromClient}]{Server.clients[_fromClient].username} sent a message to the server.");
            Console.WriteLine($"Command: {_command}");
            SerialCommandsSend.LedCommand(_command);
        }
    }
}
