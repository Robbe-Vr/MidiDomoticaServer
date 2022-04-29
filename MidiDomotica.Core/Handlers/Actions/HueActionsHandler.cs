using MidiDomotica.Core.WrapperTranslators;
using MidiDomotica.Exchange.Models;
using MidiDomotica.MidiReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;
using static MidiDomotica.Core.WrapperTranslators.HueTranslator;

namespace MidiDomotica.Core.Handlers.Actions
{
    public class HueActionsHandler
    {
        private MidiManager _manager;

        public HueActionsHandler(MidiManager manager)
        {
            _manager = manager;
        }

        public IEnumerable<string> GetTargetsForMode(Modes mode)
        {
            switch (mode)
            {
                case Modes.Lights:
                    return GetAvailableLights();

                case Modes.Room:
                    return GetAvailableRooms();

                case Modes.Entertainment:
                    return GetAvailableEntertainmentRooms();
            }

            return Array.Empty<string>();
        }

        public IEnumerable<string> GetEntertainmentModes()
        {
            return HueTranslator.GetEntertainmentModes();
        }

        public bool ConnectToBridge(string bridgeInfo)
        {
            return HueTranslator.ConnectToBridge(bridgeInfo);
        }
    }
}
