using System.IO;

namespace PortFDummy.Models
{
    public static class AppPaths
    {
        public static readonly string SaveDir =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GameSteemSh");


        public static string ShowcaseJsonPath => Path.Combine(SaveDir, "showcase.json");


        public static void EnsureSaveDir()
        {
            Directory.CreateDirectory(SaveDir);
        }
    }
}
