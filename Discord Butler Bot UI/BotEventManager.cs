using System;

namespace Discord_Butler_Bot_UI
{
    enum BotEvent
    {
        Online,
        JoinedChannel,
        LeftChannel,
        AddedSong,
        PlayedSong,
        SkippedSong,
    }
    internal class BotEventManager
    {
        /// <summary>
        /// Returns true if the given line represents a bot event of the given type.
        /// </summary>
        /// <param name="line">The string produced by the bot process</param>
        /// <param name="botEvent">The type of bot event to check for</param>
        public static bool IsBotEvent(String? line, BotEvent botEvent)
        {
            return line != null && line.StartsWith("BotEvent." + botEvent.ToString());
        }
    }
}
