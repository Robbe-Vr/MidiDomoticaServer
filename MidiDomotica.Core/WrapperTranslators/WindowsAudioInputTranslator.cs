using MidiDomotica.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomotica.Core.WrapperTranslators
{
    public static class WindowsAudioInputTranslator
    {
        private const string pipeTargetname = "WindowsAudio";

        /// <summary>
        /// gets the available input devices in windows
        /// </summary>
        /// <returns>returns array of strings containing input devices as "{guid}:{name}"</returns>
        public static IEnumerable<object> GetDevices()
        {
            string command = "Get::Input::All";
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response.Split(',').Select(x => { string[] data = x.Split(':'); return new { Guid = data[0], Name = data[1] } as object; });
            }
            else return Array.Empty<object>();
        }

        /// <summary>
        /// gets the name for the given input device guid
        /// </summary>
        /// <returns>returns string containing the name for the input device or empty string if not found</returns>
        public static string GetName(string deviceId)
        {
            string command = "Get::Input::Name::" + deviceId;
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response;
            }
            else return string.Empty;
        }

        /// <summary>
        /// gets the guid for the given input device name
        /// </summary>
        /// <returns>returns string containing the guid for the input device or empty string if not found</returns>
        public static string GetId(string deviceName)
        {
            string command = "Get::Input::Id::" + deviceName;
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response;
            }
            else return string.Empty;
        }

        public static string ConvertToCommand(string mode, string target, string type, string value)
        {
            return 
                mode.ToUpper() == "SET" ?
                    $"Set::{target}::{type}::{value}"
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
                response == string.Empty ||
                response == null
                )
            {
                Logger.Log("Failed to execute command for HueWrapper!\nCommand: " + command + "\nResponse: " + (response ?? "NULL"));

                return false;
            }

            return true;
        }
    }
}
