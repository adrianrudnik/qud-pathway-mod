using XRL;
using XRL.Messages;
using XRL.World;
using XRL.World.Parts;

namespace Pathway
{
    public class Pathway_Radio : IPlayerPart
    {
        public override bool HandleEvent(CommandEvent E)
        {
            MessageQueue.AddPlayerMessage("Hello world!");

            return base.HandleEvent(E);
        }

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register(CommandEvent.ID);

            base.Register(Object, Registrar);
        }
    }
}