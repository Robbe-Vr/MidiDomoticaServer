using MidiDomotica.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomotica.Core.WrapperTranslators
{
    public static class PowerModesTranslator
    {
        private const string pipeTargetname = "PowerModes";

        /// <summary>
        /// gets available power modes
        /// </summary>
        /// <returns>returns array of strings containing power modes as "{guid}:{name}"</returns>
        public static IEnumerable<object> GetPowerModes()
        {
            string command = "Get::All";
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response.Split(',').Select(x => { string[] data = x.Split(':'); return new { Guid = data[0], Name = data[1] } as object; });
            }
            else return Array.Empty<object>();
        }

        /// <summary>
        /// gets the name for the given power mode guid
        /// </summary>
        /// <returns>returns string containing the name for the powermode or empty string if not found</returns>
        public static string GetName(string modeId)
        {
            string command = "Get::Name::" + modeId;
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response;
            }
            else return string.Empty;
        }

        /// <summary>
        /// gets the guid for the given power mode name
        /// </summary>
        /// <returns>returns string containing the guid for the powermode or empty string if not found</returns>
        public static string GetId(string modeName)
        {
            string command = "Get::Id::" + modeName;
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response;
            }
            else return string.Empty;
        }

        public static string ConvertToCommand(string mode, string type, string modeInfo)
        {

            return 
                mode.ToUpper() == "GET" ?
                    $"Get::{type}::{modeInfo}"
                :
                mode.ToUpper() == "SET" ?
                    $"Set::{modeInfo}"
                :
                null;
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
