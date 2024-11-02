using UnityEngine;
using XRL.Messages;

namespace Pathway
{
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