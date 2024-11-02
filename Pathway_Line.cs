using System.Text;
using XRL;
using XRL.World;
using XRL.World.Parts;

namespace Pathway
{
    public class Pathway_Line : IPlayerPart
    {
        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register(AfterGameLoadedEvent.ID);
            Registrar.Register(BeforeRenderEvent.ID);

            base.Register(Object, Registrar);
        }

        public override bool HandleEvent(AfterGameLoadedEvent E)
        {
            subscribe();

            return base.HandleEvent(E);
        }

        public override bool HandleEvent(BeforeRenderEvent E)
        {
            RadioManager.HandleMessages();

            return base.HandleEvent(E);
        }

        private void subscribe()
        {
            Scream.Inside("Dialing...");

            RadioManager.onOpen += () => { Scream.Inside("OnOpen"); };
            RadioManager.onMessage += message => { Scream.Outside("OnMessage " + Encoding.UTF8.GetString(message)); };
            RadioManager.onError += message => { Scream.Inside("OnError " + message); };
            RadioManager.onClose += message => { Scream.Inside("OnClose " + message); };
        }
    }
}