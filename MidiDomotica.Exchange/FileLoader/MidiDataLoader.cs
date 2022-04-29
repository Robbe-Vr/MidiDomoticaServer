using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomotica.Exchange.FileLoader
{
    public class MidiReaderData
    {
        private string _dirPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MidiDomotica"); } }
        private string _fileName = "mididata.json";
        private string _path { get { return Path.Combine(_dirPath, _fileName); } }

        public Dictionary<string, List<string>> Subscriptions { get; set; } = new Dictionary<string, List<string>>();
        public string MidiDeviceName { get; set; }

        public void Load()
        {
            if (File.Exists(_path))
            {
                string jsonString = File.ReadAllText(_path);

                MidiReaderData data = System.Text.Json.JsonSerializer.Deserialize<MidiReaderData>(jsonString);

                this.MidiDeviceName = data.MidiDeviceName;
                this.Subscriptions = data.Subscriptions;
            }
            else
            {
                this.MidiDeviceName = "Novation SL MkIII";
                Store();
            }
        }

        public bool Store()
        {
            try
            {
                if (!Directory.Exists(_dirPath)) Directory.CreateDirectory(_dirPath);

                File.WriteAllText(_path, System.Text.Json.JsonSerializer.Serialize(this));

                return true;
            }
            catch (Exception e)
            {
                Logger.Log("Failed to store midi reader data! Error: " + e.Message, SeverityLevel.Mediocre);
                return false;
            }
        }
    }
}
