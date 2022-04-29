using MidiDomotica.Core.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomotica.Core.WrapperTranslators
{
    public static class SoundboardTranslator
    {
        private const string pipeTargetname = "Soundboard";

        public static bool SetInputDevice(string deviceInfo)
        {
            string command = "Set::Input::" + deviceInfo;
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            return ValidateResponse(response, command);
        }

        public static bool SetThroughputDevice(string deviceInfo)
        {
            string command = "Set::Throughput::" + deviceInfo;
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            return ValidateResponse(response, command);
        }

        public static bool SetOutputDevice(string deviceInfo)
        {
            string command = "Set::Output::" + deviceInfo;
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            return ValidateResponse(response, command);
        }

        public static IEnumerable<Sound> GetSounds()
        {
            string command = "Get::Sounds";
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
               string[] soundPaths = response.Split(',');

                return soundPaths.Select(path => new Sound(path));
            }
            else return Array.Empty<Sound>();
        }

        public static IEnumerable<Sound> GetSoundsFromFolder(string folderName)
        {
            return GetSounds().Where(x => x.Directory == folderName);
        }

        public static IEnumerable<string> GetSoundFolders()
        {
            return GetSounds().Select(x => x.Directory).Distinct();
        }

        /// <summary>
        /// gets available output devices for the soundboard
        /// </summary>
        /// <returns>returns array of strings containing output devices as "{id}:{name}"</returns>
        public static string[] GetOutputDevices()
        {
            string command = "Get::Output";
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response.Split(',');
            }
            else return Array.Empty<string>();
        }

        /// <summary>
        /// gets available input devices for the soundboard
        /// </summary>
        /// <returns>returns array of strings containing input devices as "{id}:{name}"</returns>
        public static string[] GetInputDevices()
        {
            string command = "Get::Input";
            string response = PipeMessageHandler.Request(pipeTargetname, command);

            if (response != null && ValidateResponse(response, command))
            {
                return response.Split(',');
            }
            else return Array.Empty<string>();
        }


        public static string ConvertToCommand(string mode)
        {
            return $"{mode}";
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

    public class Sound
    {
        public Sound() { }
        public Sound(string filePath)
        {
            FilePath = filePath;
        }

        public string Name { get { return Path.GetFileNameWithoutExtension(FilePath); } }
        public string Directory { get { return Path.GetFileName(Path.GetDirectoryName(FilePath)); } }
        public string FilePath { get; set; }
    }
}
