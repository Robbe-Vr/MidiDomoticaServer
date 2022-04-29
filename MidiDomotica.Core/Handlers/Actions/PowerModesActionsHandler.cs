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
    public class PowerModesActionsHandler
    {
        private MidiManager _manager;

        public PowerModesActionsHandler(MidiManager manager)
        {
            _manager = manager;
        }

        public IEnumerable<object> GetPowerModes()
        {
            return PowerModesTranslator.GetPowerModes();
        }

        public string GetName(string id)
        {
            return PowerModesTranslator.GetName(id);
        }

        public string GetId(string name)
        {
            return PowerModesTranslator.GetId(name);
        }
    }
}
