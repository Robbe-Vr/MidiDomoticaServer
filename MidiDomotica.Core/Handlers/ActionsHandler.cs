using MidiDomotica.Exchange.Models;
using MidiDomotica.MidiReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiDomotica.Core.Handlers
{
    public class ActionsHandler
    {
        private MidiManager _manager;

        public ActionsHandler(MidiManager manager)
        {
            _manager = manager;
        }

        public bool InstantExecuteAction(string actionGroup, string actionDataString)
        {
            return _manager.InstantExecute(actionGroup, actionDataString);
        }

        public bool LinkActionToEvent(string eventName, string actionGroup, string actionDataString)
        {
            return _manager.SubscribeToEvent(eventName, actionGroup, actionDataString);
        }

        public bool UnlinkActionFromEvent(string eventName, string actionGroup, string actionDataString)
        {
            return _manager.UnsubscribeFromEvent(eventName, actionGroup, actionDataString);
        }
    }
}
