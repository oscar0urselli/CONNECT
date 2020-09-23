using Core;
using System;
using System.Net;

class ClientHandle
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Console.WriteLine($"Message from server: {_msg}");
        Client.myId = _myId;
        ClientSend.WelcomeReceived();
    }

    public static void SpawnClient(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();

        Client.clients.Add(_id, _username);
    }

    public static void ClientDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Client.clients.Remove(_id);
    }

    public static void UserSentMessage(Packet _packet)
    {
        int _fromClientId = _packet.ReadInt();
        string _msg = _packet.ReadString();

        Console.WriteLine($"Message from {Client.clients[_fromClientId]}:\n {_msg}");
    }
}