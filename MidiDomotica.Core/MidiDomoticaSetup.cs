using MidiDomotica.MidiReader;
using System;

namespace MidiDomotica.Core
{
    public static class MidiDomoticaSetup
    {
        public static bool Initialized { get; private set; }

        public static EventsManager EventsManager { get; private set; }

        public static bool Init()
        {
            EventsManager = new EventsManager();

            Initialized = true;
            return true;
        }
    }
}
