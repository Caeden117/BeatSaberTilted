using IllusionPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Tilted
{
    public class Plugin : IPlugin
    {
        //Credit where credit is due.
        //Most of the code is based off of FullComboDisplay by bigfoott.
        //Config code based off of HitScoreVisualizer config code by artemiswkearney (Except simplified by a shit ton).

        public string Name => "Tilted";
        public string Version => "1.1.1";

        private readonly string[] env = { "DefaultEnvironment", "BigMirrorEnvironment", "TriangleEnvironment", "NiceEnvironment" };

        private bool enabled = true;

        public void OnApplicationStart()
        {
            enabled = Config.load().enabled; //This ensures that if there is no config that it gets created when Beat Saber starts.
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            if (!env.Contains(arg1.name) || !enabled) return;
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
