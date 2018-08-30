using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace Tilted
{
    struct ConfigInfo
    {
        public bool enabled;
        public float scalar;
        public bool extremeMode;

        public ConfigInfo(string randomtextbecausecsharpdoesntlikeconstructorswithnoparams)
        {
            enabled = true;
            scalar = 1;
            extremeMode = false;
        }

        public ConfigInfo(bool isEnabled, float tiltedScalar, bool avoidFilters)
        {
            enabled = isEnabled;
            scalar = tiltedScalar;
            extremeMode = avoidFilters;
        }
    }

    class Config
    {
        public static float defaultScalar = 10;

        private const string FILE_PATH = "/UserData/Tilted.txt";
        private const string DEFAULT = @"enabled|true
scalar|10
avoidFiltersBecauseYouAreAFuckingMadManAndItWillMakeBeatSaberUnplayable|false

Using this plugin will sometimes crash Beat Saber when exiting a level. I have no idea how to find a solution for this issue.
Avoiding filters can and WILL make Beat Saber unplayable.

";

        public static string fullPath => Environment.CurrentDirectory.Replace('\\', '/') + FILE_PATH;

        public static ConfigInfo load()
        {
            Console.WriteLine("[Tilted] Loading config file...");
            if (!File.Exists(fullPath))
            {
                Console.WriteLine("[Tilted] No config found! Creating default config...");
                File.WriteAllText(fullPath, DEFAULT);
                Console.WriteLine("[Tilted] Config file created.");
                return new ConfigInfo("");
            }
            string[] config = File.ReadAllLines(fullPath);
            return new ConfigInfo(bool.Parse(config[0].Split('|').Last()), float.Parse(config[1].Split('|').Last()), bool.Parse(config[2].Split('|').Last()));
        }
    }
}
