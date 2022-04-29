using MidiDomotica.Exchange.FileLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MidiDomotica.MidiReader
{
    public class EventsManager
    {
        private MidiReciever midiReciever;

        public event EventHandler onDisconnect;
        public event EventHandler onConnect;

        public EventsManager()
        {
            MidiReaderData data = new MidiReaderData();
            data.Load();

            midiReciever = new MidiReciever(data);

            midiReciever.onDisconnect += (sender, e) =>
            {
                onDisconnect?.Invoke(sender, e);
            };
            midiReciever.onConnect += (sender, e) =>
            {
                onConnect?.Invoke(sender, e);
            };
        }

        public string CurrentMidiDevice { get { return midiReciever.CurrentMidiDevice; } }
        public bool Connected { get { return midiReciever.Connected; } }

        public void SetCallback(Action<string, string, int> callback)
        {
            midiReciever.SetCallback(callback);
        }

        public bool AddSubscription(string eventName, string funcData)
        {
            return midiReciever.AddSubsciption(eventName, funcData);
        }

        public bool RemoveSubscription(string eventName, string funcData)
        {
            return midiReciever.RemoveSubscription(eventName, funcData);
        }

        public List<string> GetMidiDevices()
        {
            return midiReciever.GetMidiDevices();
        }

        public IEnumerable<string> GetEventNames()
        {
            return DefaultEventNames.GetNames();
        }

        public IReadOnlyList<string> GetSubscriptionsForEvent(string eventName)
        {
            return midiReciever.GetSubsciptions(eventName);
        }

        public bool TrySetMidiDevice(string deviceName)
        {
            if (midiReciever.SetMidiDevice(deviceName))
            {
                midiReciever.MidiMessageRecieved += (sender, e) =>
                {
                    
                };

                return true;
            }

            midiReciever.ManualDisconnect = false;

            return false;
        }

        public void StopRecieving(bool manual = false, bool dispose = false)
        {
            if (dispose)
            {
                midiReciever.onDisconnect -= (sender, e) =>
                {
                    onDisconnect?.Invoke(sender, e);
                };
                midiReciever.onConnect -= (sender, e) =>
                {
                    onConnect?.Invoke(sender, e);
                };

                midiReciever.StopRecieving(log: false);
                midiReciever = null;
            }
            else
            {
                midiReciever.ManualDisconnect = manual;

                if (midiReciever != null)
                {
                    midiReciever.StopRecieving(log: !manual);
                }
            }
        }
    }
}
