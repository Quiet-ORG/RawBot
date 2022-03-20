using RawBot.Network.Packets.Json;
using RawBot.Network.Packets.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RawBot.Network.Packets
{
    public class PacketTypeMap
    {
        private static Dictionary<string, Type> _packetTypes = new();

        static PacketTypeMap()
        {
            foreach (var packetType in typeof(IPacket).Assembly.GetTypes()
                .Where(t => t.IsClass
                            && !t.IsAbstract
                            && t.IsAssignableTo(typeof(IPacket))))
            {
                var action = packetType.GetCustomAttribute<ActionAttribute>();
                if (action is null)
                {
                    continue;
                }

                var prefix = Protocols.Xml;
                if (packetType.IsSubclassOf(typeof(StringPacket)))
                {
                    prefix = Protocols.String;
                }
                else if (packetType.IsSubclassOf(typeof(JsonPacket)))
                {
                    prefix = Protocols.Json;
                }

                foreach (var name in action.Names)
                {
                    _packetTypes[$"{prefix}:{name}"] = packetType;
                }
            }
        }

        public static Type GetPacketType(string protocol, string action)
        {
            return _packetTypes.GetValueOrDefault($"{protocol}:{action}");
        }
    }
}
