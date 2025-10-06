using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortFDummy.Services
{
    internal class JsonShowcaseStore
    {
        private readonly string filePath;
        public JsonShowcaseStore(string filePath)
        {
            this.filePath = filePath;
        }

        public List<int> Load()
        {
            try
            {
                if (!File.Exists(filePath)) return new();
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<int>>(json) ?? new();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("読込失敗: " + ex.Message);
                return new();
            }
        }

        public void Save(List<int> showcaseIds)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                string json = JsonSerializer.Serialize(showcaseIds, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("保存失敗: " + ex.Message);
            }
        }
    }
}