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
        public IEnumerable<object> GetMixers()
        {
            return WindowsAudioMixerTranslator.GetMixers();
        }

        public string GetMixerName(string deviceId)
        {
            return WindowsAudioMixerTranslator.GetName(deviceId);
        }

        public string GetMixerId(string deviceName)
        {
            return WindowsAudioMixerTranslator.GetId(deviceName);
        }
    }
}
