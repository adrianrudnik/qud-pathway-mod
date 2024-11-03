using UnityEngine.UI;
using XRL;
using XRL.World;
using XRL.World.Parts;

// https://github.com/endel/NativeWebSocket
// https://sta.github.io/websocket-sharp/
// https://wiki.cavesofqud.com/wiki/Modding:Adding_Code_at_Startup
// https://github.com/egocarib/CavesOfQud-QudUX-v2
// https://github.com/plaidman/qud-mods

namespace Pathway
{
    // This will wire the actual player to the websocket instance
    // and later on, process stuff like player character name to "id" the client.
    public class PlayerWire : IPlayerPart
    {
        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register(BeforeRenderEvent.ID);

            Scream.Inside("PlayerWire.Register");
            
            RadioManager.Socket.OnOpen += () => Scream.Inside("OnOpen");
            RadioManager.Socket.OnError += e => Scream.Inside("OnError: " + e);
            RadioManager.Socket.OnClose += e => Scream.Inside("OnClose: " + e);

            // Pipe "normal" message through to the player history within the game
            RadioManager.Socket.OnMessage += bytes => Scream.Outside("OnMessage: " + System.Text.Encoding.UTF8.GetString(bytes));
            
            base.Register(Object, Registrar);
        }
        
        public override bool HandleEvent(BeforeRenderEvent E)
        {
            // This needs to be called in regular intervals to consume the incoming messages that
            // pile up in a websocket buffer.
            RadioManager.HandleMessages();
            
            return base.HandleEvent(E);
        }
    }
}