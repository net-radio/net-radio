using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NetRadio.Devices;

namespace NetRadio.G31Ddc.Audio.Models
{
    class SettingsModel
    {
        private const string RECORDING_FOLDER = "Recordings";

        public string RecordingPath { get; set; }
        public bool RecordMp3 { get; set; }
        public bool RecordWav { get; set; }
        public SoftwareAgc FastSagc { get; set; }
        public SoftwareAgc MediumSagc { get; set; }
        public SoftwareAgc SlowSagc { get; set; }
        public uint StartFrequency { get; set; }
        public float AudioBuffer { get; set; }
        public int AfcThreshold { get; set; }
        public int MeterUpdateSpeed { get; set; }
        public bool DetailedAnalyze { get; set; }
        public bool AsyncAnalyze { get; set; }

        public static string DataPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NetRadio");
        }

        public SettingsModel()
        {
            RecordingPath = Path.Combine(DataPath(), RECORDING_FOLDER);
            RecordMp3 = true;
            RecordWav = true;

            SlowSagc = new SoftwareAgc { AttackTime = 25, DecayTime = 4000, ReferenceLevel = -8 };
            MediumSagc = new SoftwareAgc { AttackTime = 15, DecayTime = 2000, ReferenceLevel = -8 };
            FastSagc = new SoftwareAgc { AttackTime = 5, DecayTime = 200, ReferenceLevel = -8 };

            StartFrequency = 88600000;

            AudioBuffer = 0.3F;
            AfcThreshold = 500;
            MeterUpdateSpeed = 5;

            DetailedAnalyze = true;
            AsyncAnalyze = false;
        }

        public void Initialize()
        {
            Directory.CreateDirectory(DataPath());
            Directory.CreateDirectory(RecordingPath);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(GetPath(), json);
        }

        public static SettingsModel LoadOrCreate()
        {
            var path = GetPath();
            if (File.Exists(path))
                return Load(path);

            var settings = new SettingsModel();
            settings.Initialize();
            settings.Save();

            return settings;
        }

        public static string GetPath()
        {
            var path = Path.Combine(DataPath(), "settings.json");
            return path;
        }

        private static SettingsModel Load(string path)
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<SettingsModel>(json);
        }
    }
}
