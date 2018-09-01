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
        public bool avoidFilters;
        public bool includeCameras;
        public bool shakeNotes;
        public enum tiltedModes { standard, shakecamera, armageddon };
        public tiltedModes tiltedMode;

        public ConfigInfo(string randomtextbecausecsharpdoesntlikeconstructorswithnoparams)
        {
            enabled = true;
            scalar = 10;
            avoidFilters = false;
            includeCameras = true;
            tiltedMode = tiltedModes.standard;
            shakeNotes = true;
        }

        public ConfigInfo(bool isEnabled, string mode, float tiltedScalar, bool skipCameras, bool shakeNotes, bool avoidFilters)
        {
            enabled = isEnabled;
            scalar = tiltedScalar;
            this.avoidFilters = avoidFilters;
            includeCameras = skipCameras;
            tiltedMode = (tiltedModes)Enum.Parse(typeof(tiltedModes), mode);
            this.shakeNotes = shakeNotes;
        }
    }

    class Config
    {
        public static float defaultScalar = 10;

        private const string FILE_PATH = "/UserData/Tilted.txt";
        private const string DEFAULT = @"enabled|true
mode|standard
scalar|10
includeCameras|false
shakeCamera_AlsoShakeNotes|true
avoidFiltersBecauseYouAreAFuckingMadManAndItWillMakeBeatSaberUnplayable|false




Using this plugin will sometimes crash Beat Saber when exiting a level. I have no idea how to find a solution for this issue.
Avoiding filters can and WILL make Beat Saber unplayable.";  //Lots of spaces for future config options

        public static string fullPath => Environment.CurrentDirectory.Replace('\\', '/') + FILE_PATH;

        public static ConfigInfo load()
        {
            ConfigInfo info = new ConfigInfo();
            bool isParsed = true;
            Console.WriteLine("[Tilted] Loading config file...");
            if (File.Exists(fullPath))
            {
                string[] config = File.ReadAllLines(fullPath);
                try
                {
                    if (!bool.TryParse(config[0].Split('|').Last(), out info.enabled)) isParsed = false; //Check if config values are valid.
                    if (!Enum.TryParse(config[1].Split('|').Last().ToLower(), out info.tiltedMode)) isParsed = false;
                    if (!float.TryParse(config[2].Split('|').Last(), out info.scalar)) isParsed = false;
                    if (!bool.TryParse(config[3].Split('|').Last(), out info.includeCameras)) isParsed = false;
                    if (!bool.TryParse(config[4].Split('|').Last(), out info.shakeNotes)) isParsed = false;
                    if (!bool.TryParse(config[5].Split('|').Last(), out info.avoidFilters)) isParsed = false;
                    if (isParsed) return info;
                }
                catch (Exception) { } //If it messes up so bad it caused an exception, lets just overwrite it.
            }
            Console.WriteLine("[Tilted] Invalid or non-existing config! Overwriting with default settings...");
            File.WriteAllText(fullPath, DEFAULT);
            Console.WriteLine("[Tilted] Config file created.");
            return new ConfigInfo("");
        }
    }
}
