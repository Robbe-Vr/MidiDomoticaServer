using MidiDomotica.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomotica.Core.WrapperTranslators
{
    public static class WindowsAudioMixerTranslator
    {
        private const string pipeTargetname = "WindowsAudio";

        /// <summary>
        /// gets the available application mixers in windows
        /// </summary>
        /// <returns>returns array of strings containing mixers as "{guid}:{name}"</returns>
        public static IEnumerable<object> GetMixers()
        {
            string command = "Get::Mixer::All";
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response.Split(',').Select(x => { string[] data = x.Split(':'); return new { Guid = data[0], Name = data[1] } as object; });
            }
            else return Array.Empty<object>();
        }

        /// <summary>
        /// gets the name for the given mixer guid
        /// </summary>
        /// <returns>returns string containing the name for the mixer or empty string if not found</returns>
        public static string GetName(string deviceId)
        {
            string command = "Get::Mixer::Name::" + deviceId;
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response;
            }
            else return string.Empty;
        }

        /// <summary>
        /// gets the guid for the given mixer name
        /// </summary>
        /// <returns>returns string containing the guid for the mixer or empty string if not found</returns>
        public static string GetId(string deviceName)
        {
            string command = "Get::Mixer::Id::" + deviceName;
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
                    $"Set::Mixer::{target}::{type}::{value}"
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
