using RawBot.State;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RawBot.Network.Packets.Xml
{
    [Action("apiOk")]
    public class ApiOk : XmlPacket<ApiOkPayload>
    {
        public override Task HandleAsync(Game game)
        {
            return game.Client.SendAsync("< msg t = 'sys' >< body action = 'apiOK' r = '0' ></ body ></ msg > ");
        }


    }

    [XmlRoot("body")]
    public class ApiOkPayload
    {

    }
}