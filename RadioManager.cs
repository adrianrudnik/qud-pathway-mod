using System.Threading.Tasks;
using NativeWebSocket;
using XRL;

namespace Pathway
{
    [HasModSensitiveStaticCache]
    [HasCallAfterGameLoaded]
    public static class RadioManager
    {
        public static WebSocket Socket;

        [ModSensitiveCacheInit]
        public static void Init()
        {
            // We use it as a global singleton for during the wholes game runtime
            // until I may find a better way to find something like a
            // deconstruct player, session or game somewhere.
            Socket = new WebSocket("ws://localhost:8080");

            // Ignore the returned task, it will terminate itself and
            // this method does not like to be awaited
            _ = Connect();
        }

        [CallAfterGameLoaded]
        public static void WirePlayer()
        {
            The.Player.RequirePart<PlayerWire>();
        }

        // Create a function that takes a callback function
        public static async Task Connect()
        {
            await Disconnect();

            Scream.Inside("Connecting websocket");

            await Socket.Connect();
        }

        public static async Task Disconnect()
        {
            Scream.Inside("Disconnecting");

            if (Socket.State is WebSocketState.Open or WebSocketState.Connecting)
            {
                Scream.Inside("Closing websocket");
                await Socket.Close();
            }
            else
            {
                Scream.Inside("Canceling websocket");
                Socket.CancelConnection();
            }

            // At least consume the first batch of messages, not yet sure if required
            // to handle possible clean disconnects, but I do it anyway
            Socket.DispatchMessageQueue();

            // Recreate a fresh instance
            Socket = new WebSocket("ws://localhost:8080");
        }

        // Helper function that normally goes into Unity Update method that needs to be
        // called in specific intervals (like every sync frame or so)
        public static void HandleMessages()
        {
            Socket?.DispatchMessageQueue();
        }
    }
}