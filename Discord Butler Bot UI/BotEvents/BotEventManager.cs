using System;
using System.Windows.Media;

namespace Discord_Butler_Bot_UI.BotEvents
{
    public enum BotEvent
    {
        Online,
        JoinedChannel,
        LeftChannel,
        AddedSong,
        PlayingSong,
        SkippedSong,
        None
    }
    internal static class BotEventManager
    {
        // If a line starts with this prefix, it is a bot event
        // Example Format: BotEvent.Online
        private const string BOT_EVENT_PREFIX = "BotEvent.";

        /// <summary>
        /// Gets a bot event from the given line. The line is expected to be the output of the bot process.
        /// </summary>
        /// <param name="line">The line to get the bot event from</param>
        /// <returns>The bot event from the given line. If the line doesn't contain a valid event, returns BotEvent.None</returns>
        public static BotEvent GetBotEvent(string? line)
        {
            if (line == null) return BotEvent.None;

            if (line.StartsWith(BOT_EVENT_PREFIX))
            {
                // Remove the prefix
                line = line.Substring(BOT_EVENT_PREFIX.Length);

                // Loop through all bot events and check if the line starts with one of them
                foreach (var botEvent in Enum.GetValues<BotEvent>())
                {
                    if (line.StartsWith(botEvent.ToString()))
                    {
                        return botEvent;
                    }
                }
            }

            return BotEvent.None;
        }

        public static Brush BotEventToBrush(BotEvent botEvent)
        {
            return botEvent switch
            {
                BotEvent.Online => Brushes.Green,
                BotEvent.JoinedChannel => Brushes.LightGreen,
                BotEvent.LeftChannel => Brushes.Pink,
                BotEvent.AddedSong => Brushes.LightBlue,
                BotEvent.PlayingSong => Brushes.Blue,
                BotEvent.SkippedSong => Brushes.LightYellow,
                BotEvent.None => Brushes.LightGray,
                _ => Brushes.LightGray,
            };
        }
    }
}
