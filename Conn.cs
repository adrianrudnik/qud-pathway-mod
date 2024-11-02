using System.Text;
using NativeWebSocket;
using UnityEngine;

namespace Pathway
{
    public class Connection : MonoBehaviour
    {
        private WebSocket websocket;

        // Start is called before the first frame update
        private async void Start()
        {
            websocket = new WebSocket("ws://localhost:2567");

            websocket.OnOpen += () => { Scream.Inside("Connection open!"); };

            websocket.OnError += e => { Scream.Inside("Error! " + e); };

            websocket.OnClose += e => { Scream.Inside("Connection closed!"); };

            websocket.OnMessage += bytes =>
            {
                Scream.Inside("OnMessage: " + Encoding.UTF8.GetString(bytes));

                // getting the message as a string
                // var message = System.Text.Encoding.UTF8.GetString(bytes);
                // Debug.Log("OnMessage! " + message);
            };

            // Keep sending messages at every 0.3s
            // InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

            // waiting for messages
            await websocket.Connect();
        }

        private void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            websocket.DispatchMessageQueue();
#endif
        }

        private async void OnApplicationQuit()
        {
            await websocket.Close();
        }

        private async void SendWebSocketMessage()
        {
            if (websocket.State == WebSocketState.Open)
            {
                // Sending bytes
                await websocket.Send(new byte[] { 10, 20, 30 });

                // Sending plain text
                await websocket.SendText("plain text message");
            }
        }
    }
}