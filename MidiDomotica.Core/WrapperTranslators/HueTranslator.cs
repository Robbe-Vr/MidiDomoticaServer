using MidiDomotica.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomotica.Core.WrapperTranslators
{
    public static class HueTranslator
    {
        private const string pipeTargetname = "Hue";

        public enum Modes
        {
            Lights,
            Room,
            Entertainment,
        }

        public static bool ConnectToBridge(string bridgeInfo)
        {
            string command = "Connect::Bridge::" + bridgeInfo;
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            return ValidateResponse(response, command);
        }

        /// <summary>
        /// gets available lights on the currently connected hue bridge
        /// </summary>
        /// <returns>returns array of strings containing light data as "{id}:{name}"</returns>
        public static string[] GetAvailableLights()
        {
            string command = "Get::Available::Lights";
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response.Split(',');
            }
            else return Array.Empty<string>();
        }

        /// <summary>
        /// gets available rooms on the currently connected hue bridge
        /// </summary>
        /// <returns>returns array of strings containing room names</returns>
        public static string[] GetAvailableRooms()
        {
            string command = "Get::Available::Rooms";
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response.Split(',');
            }
            else return Array.Empty<string>();
        }

        /// <summary>
        /// gets available entertainment rooms on the currently connected hue bridge
        /// </summary>
        /// <returns>returns array of strings containing entertainment room names</returns>
        public static string[] GetAvailableEntertainmentRooms()
        {
            string command = "Get::Available::Entertainment::Rooms";
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response.Split(',');
            }
            else return Array.Empty<string>();
        }

        /// <summary>
        /// gets the available entertainment modes
        /// </summary>
        /// <returns>returns array of strings containing entertainment mode names</returns>
        public static IEnumerable<string> GetEntertainmentModes()
        {
            string command = "Get::Available::Entertainment::Modes";
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response.Split(',');
            }
            else return Array.Empty<string>();
        }

        public enum State
        {
            ON,
            OFF,
            ALERT,
            ALERT_ONCE,
        }

        public static string ConvertToCommand(Modes mode, string[] target_s, State state, int? brightness = null, string colorHex = null, int[] colorRGB = null)
        {
            string target = target_s.Length == 1 ? target_s[0] : $"[{String.Join(",", target_s)}]";
            string color = colorHex ?? $"[{String.Join(",", colorRGB)}]";
            return $"{mode}::{target}::{state.ToString().Replace('_', '-')}" +
                brightness != null ? $"::{brightness}" : string.Empty +
                color != null ? $"::{color}" : string.Empty;
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
