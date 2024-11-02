using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NativeWebSocket;
using XRL;
using XRL.CharacterBuilds;
using XRL.World;

// https://sta.github.io/websocket-sharp/
// https://wiki.cavesofqud.com/wiki/Modding:Adding_Code_at_Startup

namespace Pathway
{
    [HasCallAfterGameLoaded]
    [HasModSensitiveStaticCache]
    [HasGameBasedStaticCache]
    public static class RadioManager
    {
        public static WebSocket Socket;

        private static readonly List<WebSocketOpenEventHandler> onOpenDelegates = new();

        private static readonly List<WebSocketMessageEventHandler> onMessageDelegates = new();

        private static readonly List<WebSocketErrorEventHandler> onErrorDelegates = new();

        private static readonly List<WebSocketCloseEventHandler> onCloseDelegates = new();

        [ModSensitiveCacheInit]
        public static void InitModCache()
        {
            Socket = new WebSocket("ws://localhost:8080");
            
            _ = Connect();
        }

        [CallAfterGameLoaded]
        public static void InitGameLoaded()
        {
            The.Game.RequireSystem<Pathway_System>();
            // The.Player.RequirePart<Pathway_Line>();
        }

        // Create a function that takes a callback function
        public static async Task Connect()
        {
            await Disconnect();
            
            Scream.Inside("Initializing websocket");

            Scream.Inside("Connecting websocket");
            
            await Socket.Connect();
        }

        private static void unsubscribeAllEventHandler()
        {
            foreach (var handler in onOpenDelegates)
            {
                Scream.Inside("Unsubscribing onOpen");
                onOpen -= handler;
            }

            foreach (var handler in onMessageDelegates)
            {
                Scream.Inside("Unsubscribing onMessage");
                onMessage -= handler;
            }

            foreach (var handler in onErrorDelegates)
            {
                Scream.Inside("Unsubscribing onError");
                onError -= handler;
            }

            foreach (var handler in onCloseDelegates)
            {
                Scream.Inside("Unsubscribing onClose");
                onClose -= handler;
            }
        }

        public static event WebSocketOpenEventHandler onOpen
        {
            add
            {
                Socket.OnOpen += value;
                onOpenDelegates.Add(value);
            }
            remove
            {
                Socket.OnOpen -= value;
                onOpenDelegates.Remove(value);
            }
        }

        public static event WebSocketMessageEventHandler onMessage
        {
            add
            {
                Socket.OnMessage += value;
                onMessageDelegates.Add(value);
            }
            remove
            {
                Socket.OnMessage -= value;
                onMessageDelegates.Remove(value);
            }
        }

        public static event WebSocketErrorEventHandler onError
        {
            add
            {
                Socket.OnError += value;
                onErrorDelegates.Add(value);
            }
            remove
            {
                Socket.OnError -= value;
                onErrorDelegates.Remove(value);
            }
        }

        public static event WebSocketCloseEventHandler onClose
        {
            add
            {
                Socket.OnClose += value;
                onCloseDelegates.Add(value);
            }
            remove
            {
                Socket.OnClose -= value;
                onCloseDelegates.Remove(value);
            }
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
            
            Socket.DispatchMessageQueue();
            
            unsubscribeAllEventHandler();
            
            Socket = new WebSocket("ws://localhost:8080");
        }

        public static void HandleMessages()
        {
            Socket?.DispatchMessageQueue();
        }
    }

    public class Pathway_System : IGameSystem
    {
        public static void OnBootGame(XRLGame game, EmbarkInfo info)
        {
        }

        public override void Register(XRLGame Game, IEventRegistrar Registrar)
        {
            Scream.Inside("Registering Pathway_System");

            Registrar.Register(ZoneActivatedEvent.ID);

            base.Register(Game, Registrar);
        }

        public override bool HandleEvent(ZoneActivatedEvent E)
        {
            string[] parts =
            {
                "wX: " + The.ActiveZone.wX,
                "wY: " + The.ActiveZone.wY,
                "Z: " + The.ActiveZone.Z,
                "Level: " + The.ActiveZone.Level,
                "Tier: " + The.ActiveZone.Tier
            };

            Scream.Inside("ZoneActivatedEvent fired: " + string.Join(", ", parts));

            return true;
        }

        public override void AfterLoad(XRLGame game)
        {
            Scream.Inside("AfterLoad fired");
        }

        public override void OnRemoved()
        {
            Scream.Inside("OnRemoved fired");
        }
    }
}