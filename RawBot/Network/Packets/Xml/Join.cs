using RawBot.State;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RawBot.Network.Packets.Xml
{
    [Action("joinOK")]
    public class Join : XmlPacket<JoinPayload>
    {
        public override Task HandleAsync(Game game)
        {
            game.World.RoomId = Payload.RoomId;
            
            game.World.UserIds.Clear();
            foreach (var user in Payload.UserData.Users)
            {
                game.World.UserIds[user.Name.ToLower()] = user.Id;
            }

            return Task.CompletedTask;
        }
    }

    [XmlRoot("body")]
    public class JoinPayload
    {
        [XmlAttribute("r")]
        public int RoomId { get; set; }

        [XmlElement("uLs")]
        public UserData UserData { get; set; }
    }

    public class UserData
    {
        [XmlElement("u")]
        public List<SfcUser> Users { get; set; } = new();
    }

    public class SfcUser
    {
        [XmlElement("n")]
        public string Name { get; set; }

        [XmlAttribute("i")]
        public int Id { get; set; }

        [XmlAttribute("p")]
        public int Index { get; set; }
    }
}