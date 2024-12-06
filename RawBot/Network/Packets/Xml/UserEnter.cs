using RawBot.State;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RawBot.Network.Packets.Xml
{
    [Action("uER")]
    public class UserEnter : XmlPacket<UserEnterPayload>
    {
        public override Task HandleAsync(Game game)
        {
            return game.World.RetrieveUserData(Payload.User.Id);
        }
    }

    [XmlRoot("body")]
    public class UserEnterPayload
    {
        [XmlAttribute("r")]
        public int RoomId { get; set; }

        [XmlElement("u")]
        public SfcUser User { get; set; }
    }
}