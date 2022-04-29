using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomotica.TimedEvents
{
    internal static class TimedEventsStorage
    {
        private static string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MidiDomotica", "TimedEvents", "stored_events.json");

        internal static IEnumerable<TimedEventData> GetStoredTimedEvents()
        {
            try
            {
                string json = File.Exists(_filePath) ? File.ReadAllText(_filePath) : string.Empty;

                return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<TimedEventData>>(json) ?? new List<TimedEventData>();
            }
            catch
            {
                return new List<TimedEventData>();
            }
        }

        internal static bool StoreNew(TimedEventData data)
        {
            try
            {
                List<TimedEventData> events = GetStoredTimedEvents().ToList();

                events.Add(data);

                return StoreEvents(events);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to store new timed event for {data?.MethodStr} at UTC {data?.Offset.UtcDateTime}!\nError: {e.Message}");

                return false;
            }
        }

        internal static bool RemoveFromStored(TimedEventData data)
        {
            try
            {
                List<TimedEventData> events = GetStoredTimedEvents().ToList();

                if (events.Remove(data))
                {
                    return StoreEvents(events);
                }
                return false;
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to remove stored timed event for {data?.MethodStr} at UTC {data?.Offset.UtcDateTime}!\nError: {e.Message}");

                return false;
            }
}

        private static bool StoreEvents(IEnumerable<TimedEventData> events)
        {
            try
            {
                string jsonContent = System.Text.Json.JsonSerializer.Serialize(events);

                File.WriteAllText(_filePath, jsonContent);

                return true;
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to store updated timed events!\nError: {e.Message}");

                return false;
            }
        }
    }
}
