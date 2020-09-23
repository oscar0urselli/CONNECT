using Core;
using System;
using System.Collections.Generic;
using System.Text;

public class ClientSend
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.tcp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.myId);
            _packet.Write(Client.myUsername);

            SendTCPData(_packet);
        }
    }

    public static void UserMessage(int _toClient, string _message)
    {
        using (Packet _packet = new Packet((int)ClientPackets.userMessage))
        {
            _packet.Write(_toClient);
            _packet.Write(_message);

            SendTCPData(_packet);
        }
    }
    #endregion
}