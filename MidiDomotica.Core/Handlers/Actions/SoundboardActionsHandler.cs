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
    public class SoundboardActionsHandler
    {
        private MidiManager _manager;

        public SoundboardActionsHandler(MidiManager manager)
        {
            _manager = manager;
        }

        public IEnumerable<string> GetSoundboardSoundFolders()
        {
            return SoundboardTranslator.GetSoundFolders();
        }

        public IEnumerable<Sound> GetSoundboardSoundsFromFolder(string folder)
        {
            return SoundboardTranslator.GetSoundsFromFolder(folder);
        }

        public IEnumerable<Sound> GetSoundboardSounds()
        {
            return SoundboardTranslator.GetSounds();
        }

        public IEnumerable<string> GetInputDevices()
        {
            return SoundboardTranslator.GetInputDevices();
        }

        public IEnumerable<string> GetOutputDevices()
        {
            return SoundboardTranslator.GetOutputDevices();
        }

        public bool SetInputDevice(string inputDeviceInfo)
        {
            return SoundboardTranslator.SetInputDevice(inputDeviceInfo);
        }

        public bool SetThroughputDevice(string outputDeviceInfo)
        {
            return SoundboardTranslator.SetThroughputDevice(outputDeviceInfo);
        }

        public bool SetOutputDevice(string outputDeviceInfo)
        {
            return SoundboardTranslator.SetThroughputDevice(outputDeviceInfo);
        }
    }
}
