using IllusionPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Text;

namespace Tilted
{
    public class Plugin : IPlugin
    {
        //Credit where credit is due.
        //Most of the code is based off of FullComboDisplay by bigfoott.
        //Config code based off of HitScoreVisualizer config code by artemiswkearney (Except simplified by a shit ton).

        public string Name => "Tilted";
        public string Version => "1.1.6";

        private readonly string[] env = { "DefaultEnvironment", "BigMirrorEnvironment", "TriangleEnvironment", "NiceEnvironment" };

        private bool enabled = true;

        public void OnApplicationStart()
        {
            enabled = Config.load().enabled; //This ensures that if there is no config that it gets created when Beat Saber starts.
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            if (!enabled) return;
            if (arg1.buildIndex == 1)
            {
                TMP_Text newText = null;
                TMP_Text[] array = Resources.FindObjectsOfTypeAll<TMP_Text>();
                foreach (TMP_Text bob in array)
                    if (bob.gameObject.activeInHierarchy)
                        newText = bob;
                if (newText != null)
                {
                    string[] modeText = new string[] { "Screw up the scene.", "Shake every external camera." };
                    newText.text += String.Format("\n\n<color=#FF0000><size=200%>Tilted Plugin (Version {0}) by Caeden117</size></color>\n\n", Version);
                    if (Config.load().avoidFilters)
                    {
                        newText.text += "<color=#FF0000><size=200%>I hope you know what you're getting into.</size></color>\n\n" +
                            "If you are seeing this text, then you have disabled the built-in filters for the plugin.\n" + 
                            "When losing a combo, GameObjects that are key for the game to work properly (mainly Note/Obstacle spawning) will also be effected.\n" +
                            "This will, unless by some act of god you're able to live through the carnage, make the level unbeatable and may crash the game.\n" +
                            "<color=#FF0000><size=150%>This is a warning of what's to come.</size></color>";
                    }
                    else
                    {
                        newText.text += "<size=150%>Current Mode: " + modeText[(int)Config.load().tiltedMode] + "</size>\n\n";
                        newText.text += "By continuing into Beat Saber, you understand that this plugin is enabled and can cause the game to act abnormaly when losing a combo.\n" +
                            "Actions described above are a direct result of this plugin.\n\n" +
                            "If you wish to disable this plugin, please open the plugin's Configuration file located in:\n" +
                            "<color=#7289DA>" + Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/Tilted.txt</color>\n" +
                            "and change the <color=#7289DA>enabled</color> setting to <color=#7289DA>false</color>.";
                        
                    }
                    if (new System.Random().Next(0, 50) == 50)
                    {
                        newText.text += "\n\n" + Encoding.UTF8.GetString(Convert.FromBase64String("SWYgeW91ciBuYW1lIGlzIFJRLCB5b3UgZGVzZXJ2ZWQgdG8gYmUgYmFubmVkLg==")); //OwO what's this?
                    }
                }
            }
            if (!env.Contains(arg1.name)) return;
            switch (Config.load().tiltedMode)
            {
                case ConfigInfo.tiltedModes.standard:
                    new GameObject("Tilted").AddComponent<TiltedComponent>();
                    break;
                case ConfigInfo.tiltedModes.shakecamera:
                    new GameObject("Tilted").AddComponent<TiltedCameraShake>();
                    break;
            }
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
        }

        public void OnLevelWasLoaded(int level) { }
        public void OnLevelWasInitialized(int level) { }
        public void OnUpdate() { }
        public void OnFixedUpdate() { }
    }
}
