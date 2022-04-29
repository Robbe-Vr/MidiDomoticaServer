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
    public partial class WindowsAudioActionsHandler
    {
        private MidiManager _manager;

        public WindowsAudioActionsHandler(MidiManager manager)
        {
            _manager = manager;
        }
    }
}
