using System.Collections.Generic;
using System.IO;
using NetRadio.G313.Model;
using Newtonsoft.Json;

namespace NetRadio.G313.Utilties.G313
{
    public class Memorizer:List<MemorySlot>
    {
        private const string MEMORY_FILE = "memory.json";

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this);
            var path = GetPath();
            File.WriteAllText(path,json);
        }

        public static Memorizer LoadOrCreate()
        {
            var path = GetPath();
            if (File.Exists(path))
                return LoadFromFile();

            var memorizer = new Memorizer();
            memorizer.Save();

            return memorizer;
        }

        private static Memorizer Load(string json)
        {
            return JsonConvert.DeserializeObject<Memorizer>(json);
        }

        public static Memorizer LoadFromFile()
        {
            var path = GetPath();
            var text = File.ReadAllText(path);
            return Load(text);
        }

        private static string GetPath()
        {
            return Path.Combine(SettingsModel.DataPath(), MEMORY_FILE);
        }
    }
}
