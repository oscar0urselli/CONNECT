using System;
using System.Collections.Generic;

namespace CONNECTServerSide
{
    class ServerSend
    {
        private static void SendTCPData(int _toCLient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toCLient].tcp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxUsers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxUsers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        #region Packets
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void SpawnUser(int _fromClient, string _username)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnUser))
            {
                _packet.Write(_fromClient);
                _packet.Write(_username);

                SendTCPDataToAll(_packet);
            }
        }

        public static void UserDisconnected(int _playerId)
        {
            using (Packet _packet = new Packet((int)ServerPackets.userDisconnected))
            {
                _packet.Write(_playerId);

                SendTCPDataToAll(_packet);
            }
        }

        public static void UserSentMessage(int _toClient, int _fromClient, string _message)
        {
            using (Packet _packet = new Packet((int)ServerPackets.userSentMessage))
            {
                _packet.Write(_fromClient); // Id of who is sending the message
                _packet.Write(_message); // The message

                try
                {
                    SendTCPData(_toClient, _packet);
                    Console.WriteLine("Message sented!");
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error while sendig message: {_ex}");
                }
            }
        }

        #endregion
    }
}
