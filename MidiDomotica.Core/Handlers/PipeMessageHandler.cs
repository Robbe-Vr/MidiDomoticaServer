using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiDomotica.Core.Handlers
{
    internal static class PipeMessageHandler
    {
        internal static string Request(string target, string command)
        {
            IEnumerable<string> responses = SYWPipeNetworkManager.PipeMessageControl.SendToApp(target, command);
            return responses.FirstOrDefault(x => x.StartsWith(command))?.Split(" -> ")[1];
        }
    }
}
