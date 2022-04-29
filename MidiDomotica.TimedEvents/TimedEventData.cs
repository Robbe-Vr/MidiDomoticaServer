using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiDomotica.TimedEvents
{
    public class TimedEventData
    {
        public string MethodStr { get; set; }
        public TimedEventType Type { get; set; }
        public Type ClassType { get; set; }
        public DateTimeOffset Offset { get; set; }
        public int? ForAmount { get; set; }
        public DaysOfWeek? DaysOfWeek { get; set; }
    }

    [Flags]
    public enum DaysOfWeek
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64,
    }
}
