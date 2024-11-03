using UnityEngine;
using XRL.Messages;

namespace Pathway
{
    // Logger, either to the Player.log or player history log 
    public static class Scream
    {
        public static void Outside(string message)
        {
            MessageQueue.AddPlayerMessage(message);
        }

        public static void Inside(string message)
        {
            Debug.LogError("Pathway: " + message);
        }

        public static void Everywhere(string message)
        {
            Outside(message);
            Inside(message);
        }
    }
}