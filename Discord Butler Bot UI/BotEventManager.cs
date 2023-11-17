using System;

namespace Discord_Butler_Bot_UI
{
    enum BotEvent
    {
        Started
    }
    internal class BotEventManager
    {
        public static bool IsBotEvent(String? line, BotEvent botEvent)
        {
            return line != null && line.StartsWith("BotEvent." + botEvent.ToString());
        }
    }
}
