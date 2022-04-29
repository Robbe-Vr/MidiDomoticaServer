using MidiDomotica.TimedEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidiDomoticaApi.Models
{
    public class TimedEventDataModel
    {
        public string ActionGroup { get; set; }
        public string MethodStr { get; set; }

        public int Second { get; set; }
        public int Minute { get; set; }
        public int Hour { get; set; }
        public int DayOfMonth { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public int X { get; set; }
        public int amount { get; set; }
        public DaysOfWeek DaysOfWeek { get; set; }
    }
}
