using MidiDomotica.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomotica.Core.WrapperTranslators
{
    public static class KeyboardTranslator
    {
        private const string pipeTargetname = "Keyboard";

        /// <summary>
        /// gets available keys
        /// </summary>
        /// <returns>returns array of strings containing key names</returns>
        public static IEnumerable<string> GetKeys()
        {
            string command = "Get";
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response.Split(',');
            }
            else return Array.Empty<string>();
        }

        public static string ConvertToCommand(string mode, params string[] keys)
        {

            return 
                mode.ToUpper() == "GET" ?
                    "Get"
                :
                    $"Simulate::[{String.Join(',', keys)}]";
        }

        private static bool ValidateResponse(string response, string command)
        {
            if (
                response == "INVALID DATA" ||
                response == "INVALID COMMAND" ||
                response == "INVALID PROPERTY" ||
                response == "UNKNOWN GET" ||
                response == "UNKNOWN MODE" ||
                response == "NO TARGETS" ||
                response == "FAIL" ||
                response == "NO" ||
                response == string.Empty
                )
            {
                Logger.Log("Failed to execute command for HueWrapper!\nCommand: " + command + "\nResponse: " + response);

                return false;
            }

            return true;
        }
    }
}
