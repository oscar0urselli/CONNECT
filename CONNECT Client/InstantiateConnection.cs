using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Core
{
    public class Client
    {
        public static Dictionary<int, string> clients = new Dictionary<int, string>();
        public static int dataBufferSize = 4096;

        public static string ip = "127.0.0.1";
        public static int port = 26950;
        public static int myId = 0;
        public static string myUsername = "";
        public static TCP tcp;

        private static bool isConnected = false;
        private delegate void PacketHandler(Packet _packet);
        private static Dictionary<int, PacketHandler> packetHandlers;

        public static void Connect(string _username, string _ip, int _port)
        {
            myUsername = _username;
            ip = _ip;
            port = _port;

            try
            {
                TcpClient tcpClient = new TcpClient();

                Console.WriteLine("Connecting...");

                tcpClient.Connect(_ip, _port);

                tcp = new TCP();

                InitializeClientData();

                isConnected = true;
                tcp.Connect();

                Console.WriteLine("Connected.");
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"An error occured: {_ex}");
            }
        }

        public static void SendMessage(int _idReceiver, string _msg)
        {
            ClientSend.UserMessage(_idReceiver, _msg);
        }

        public class TCP
        {
            public TcpClient socket;

            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                receiveBuffer = new byte[dataBufferSize];
                socket.BeginConnect(ip, port, ConnectCallback, socket);
            }

            private void ConnectCallback(IAsyncResult _result)
            {
                socket.EndConnect(_result);

                if (!socket.Connected)
                {
                    return;
                }

                stream = socket.GetStream();

                receivedData = new Packet();

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    }
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error sending data to server via TCP: {_ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        Disconnect();
                        return;
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer, _data, _byteLength);

                    receivedData.Reset(HandleData(_data));
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch
                {
                    Disconnect();
                }
            }

            private bool HandleData(byte[] _data)
            {
                int _packetLength = 0;

                receivedData.SetBytes(_data);

                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }

                while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
                {
                    byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int _packetId = _packet.ReadInt();
                            packetHandlers[_packetId](_packet);
                        }
                    });

                    _packetLength = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {
                        _packetLength = receivedData.ReadInt();
                        if (_packetLength <= 0)
                        {
                            return true;
                        }
                    }
                }

                if (_packetLength <= 1)
                {
                    return true;
                }

                return false;
            }

            private void Disconnect()
            {
                Disconnect();

                stream = null;
                receivedData = null;
                receiveBuffer = null;
                socket = null;
            }
        }

        private static void InitializeClientData()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ServerPackets.welcome, ClientHandle.Welcome },
                { (int)ServerPackets.spawnClient, ClientHandle.SpawnClient},
                { (int)ServerPackets.clientDisconnected, ClientHandle.ClientDisconnected},
                { (int)ServerPackets.userSentMessage, ClientHandle.UserSentMessage }
            };
            Console.WriteLine("Initialized packets.");
        }

        public static void Disconnect()
        {
            if (isConnected)
            {
                isConnected = false;
                tcp.socket.Close();

                Console.WriteLine("Disconnected from server.");
            }
        }
    }
}