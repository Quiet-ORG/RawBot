/*using RawBot.State;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RawBot.Network.Packets.Xml
{
    [Action("userGone")]
    public class UserGone : XmlPacket<ApiOkPayload>
    {
        public override Task HandleAsync(Game game)
        {
            return game.Client.SendAsync("<msg t='sys'><body action='apiOK' r='0'></body></msg>");
        }


    }

    [XmlRoot("body")]
    public class UserGonePayload
    {
        [XmlAttribute("r")]
        public int RoomId { get; set; }

        [XmlElement("user")]
        public SfcUser UserId { get; set; }
    }
}
// <msg t='sys'><body action='userGone' r='{RoomId}'><user id='{UserId}' /></body></msg>
*/