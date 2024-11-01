
using XRL;

namespace Pathway.Handlers
{
    [HasCallAfterGameLoaded]
    public class LoadGameHandler
    {
        [CallAfterGameLoaded]
        public static void AfterLoaded()
        {
            The.Player.RequirePart<Pathway_Radio>();
        }      
    }
}
