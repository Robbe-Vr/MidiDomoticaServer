using MidiDomotica.Exchange.Models;
using MidiDomotica.MidiReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiDomotica.Core.Handlers
{
    public class ControlsHandler
    {
        private MidiManager _manager;

        public ControlsHandler(MidiManager manager)
        {
            _manager = manager;
        }
        public IEnumerable<ControlModel> GetControls(int page, int rpp)
        {
            return MidiDomoticaSetup.EventsManager.GetEventNames()
                .Skip((page - 1) * rpp)
                .Take(rpp)
                .Select(x => new ControlModel() { Name = x });
        }

        public IEnumerable<ActionDataModel> GetSubsciptionsForControl(string controlName, int page, int rpp)
        {
            return _manager.GetSubscriptionsForEvent(controlName)?
                .Skip((page - 1) * rpp)?
                .Take(rpp) ?? Array.Empty<ActionDataModel>();
        }
    }
}
