using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawBot.Logging;
using RawBot.Network.Packets;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RawBot.Network
{
    public delegate void RawPacketHandler(string packet);

    public delegate void PacketHandler(IPacket packet);

    public class Client
    {
        public const int MessageBufferSize = 4096;

        private readonly TcpClient _socket;
        private readonly NetworkStream _stream;

        private readonly AutoResetEvent _packetEvent = new(false);
        private Predicate<string> _packetWait;

        private volatile bool _exit;

        public ILogger Log { get; set; }

        public event RawPacketHandler RawPacketReceived;
        public event PacketHandler PacketReceived;

        public Client(string host, int port)
        {
            _socket = new TcpClient(host, port);
            _stream = _socket.GetStream();
        }

        public void Start()
        {
            new Thread(Listen).Start();
        }

        public void Stop()
        {
            _exit = true;
            _socket.Close();
        }

        private void Listen()
        {
            var messageBuffer = new byte[MessageBufferSize];
            using var currentPacket = new MemoryStream();
            while (!_exit)
            {
                var read = _stream.Read(messageBuffer, 0, MessageBufferSize);
                if (read == 0)
                {
                    Thread.Sleep(10);
                    continue;
                }

                for (var i = 0; i < read; i++)
                {
                    var b = messageBuffer[i];
                    if (b == 0)
                    {
                        if (currentPacket.Length > 0)
                        {
                            var packet = currentPacket.ToArray();
                            var packetString = Encoding.UTF8.GetString(packet);
                            currentPacket.SetLength(0);
                            OnRawPacketReceived(packetString);
                        }
                    }
                    else
                    {
                        currentPacket.Write(messageBuffer, i, 1);
                    }
                }
            }
        }

        protected void OnRawPacketReceived(string packet)
        {
            if (_packetWait is not null)
            {
                _packetEvent.Set();
            }

            RawPacketReceived?.Invoke(packet);

            if (PacketReceived is null)
            {
                return;
            }

            var action = string.Empty;
            var protocol = string.Empty;
            if (packet.StartsWith("%"))
            {
                packet = packet.Trim('%');
                action = packet.Split('%')[1];
                protocol = Protocols.String;
            }
            else if (packet.StartsWith("{"))
            {
                var json = JsonConvert.DeserializeObject<dynamic>(packet);
                action = (string)json?.b.o.cmd;
                packet = (json?.b.o as JObject)?.ToString();
                protocol = Protocols.Json;
            }
            else if (packet.StartsWith("<msg"))
            {
                var root = XElement.Parse(packet);
                var firstBody = root.Descendants("body").FirstOrDefault();
                action = firstBody?.Attribute("action")?.Value;
                packet = firstBody?.ToString();
                protocol = Protocols.Xml;
            }

            var packetType = PacketTypeMap.GetPacketType(protocol, action);
            if (packetType is not null && Activator.CreateInstance(packetType) is IPacket packetInstance)
            {
                packetInstance.Deserialize(packet);
                PacketReceived.Invoke(packetInstance);
            }
            else if (protocol != string.Empty)
            {
                Log?.Debug($"Unhandled {protocol} packet: {action}");
            }
        }

        public Task SendAsync<T>(T packet) where T : IPacket
        {
            return SendAsync(packet.Serialize());
        }

        public Task SendAsync(string packet)
        {
            var data = new byte[packet.Length + 1];
            Encoding.UTF8.GetBytes(packet, data);
            return _stream.WriteAsync(data).AsTask();
        }

        public void WaitFor(Predicate<string> packetPredicate)
        {
            _packetWait = packetPredicate;
            _packetEvent.WaitOne();
            _packetWait = null;
        }
    }
}
