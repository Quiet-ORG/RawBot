using RawBot.State;
using System;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RawBot.Network.Packets.Xml
{
    [Action("apiOK")]
    public class ApiOk : XmlPacket<ApiOkPayload>
    {
        public override Task HandleAsync(Game game)
        {
            return game.Client.SendAsync(
                $"<msg t='sys'><body action='login' r='0'><login z='zone_master'><nick><![CDATA[SPIDER#0001~{game.Username}~3.009]]></nick><pword><![CDATA[{game.Token}]]></pword></login></body></msg>");
        }
    }

    [XmlRoot("body")]
    public class ApiOkPayload
    {

    }
}