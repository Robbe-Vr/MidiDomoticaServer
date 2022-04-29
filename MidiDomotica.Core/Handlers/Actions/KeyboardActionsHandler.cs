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
    public class KeyboardActionsHandler
    {
        private MidiManager _manager;

        public KeyboardActionsHandler(MidiManager manager)
        {
            _manager = manager;
        }

        public IEnumerable<string> GetKeys()
        {
            return KeyboardTranslator.GetKeys();
        }
    }
}
