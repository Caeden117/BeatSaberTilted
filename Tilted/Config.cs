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
            scalar = 10;
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
            bool isParsed = true;
            bool enabled;
            float scalar;
            bool avoidFilters;
            Console.WriteLine("[Tilted] Loading config file...");
            if (!File.Exists(fullPath))
            {
                Console.WriteLine("[Tilted] No config found! Creating default config...");
                File.WriteAllText(fullPath, DEFAULT);
                Console.WriteLine("[Tilted] Config file created.");
                return new ConfigInfo("");
            }
            string[] config = File.ReadAllLines(fullPath);
            if (!bool.TryParse(config[0].Split('|').Last(), out enabled)) isParsed = false; //Check if config values are valid.
            if (!float.TryParse(config[1].Split('|').Last(), out scalar)) isParsed = false;
            if (!bool.TryParse(config[2].Split('|').Last(), out avoidFilters)) isParsed = false;
            if (!isParsed)
            {
                Console.WriteLine("[Tilted] Invalid config! Overwriting with default settings...");
                File.WriteAllText(fullPath, DEFAULT);
                Console.WriteLine("[Tilted] Config file created.");
                return new ConfigInfo("");
            }
            else
                return new ConfigInfo(enabled, scalar, avoidFilters);
        }
    }
}
