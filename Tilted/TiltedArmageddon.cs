using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tilted
{
    class TiltedArmageddon : MonoBehaviour
    {
        private float tiltScalar = Config.load().scalar;
        private bool avoidFilters = Config.load().avoidFilters;
        private bool includeCameras = Config.load().includeCameras;

        private ScoreMultiplierUIController multi;
        private ScoreController score;
        private BeatmapObjectSpawnController ob;

        private async void Awake()
        {
            await WaitForLoad();
        }

        private Task WaitForLoad()
        {
            return Task.Run(() =>
            {
                var loaded = false;
                while (!loaded)
                {
                    multi = Resources.FindObjectsOfTypeAll<ScoreMultiplierUIController>().FirstOrDefault();
                    score = Resources.FindObjectsOfTypeAll<ScoreController>().FirstOrDefault();
                    ob = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().FirstOrDefault();
                    if (multi == null || score == null || ob == null)
                        Thread.Sleep(10);
                    else
                        loaded = true;
                }
                Init();

            });
        }

        private void Init()
        {
            score.noteWasCutEvent += OnNoteCut;
            ob.noteWasCutEvent += OnNoteCut2;
            score.noteWasMissedEvent += OnNoteMissed;
        }

        private async void OnNoteCut(NoteData data, NoteCutInfo info, int c)
        {
            if (data.noteType == NoteType.Bomb || !info.allIsOK) await TiltScene();
        }

        private void OnNoteCut2(BeatmapObjectSpawnController cont, NoteController note, NoteCutInfo info)
        {
            Console.WriteLine(note.gameObject.name);
        }

        private async void OnNoteMissed(NoteData data, int c)
        {
            if (data.noteType != NoteType.Bomb) await TiltScene();
        }

        public static string fullPath => Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/TiltedDebug.txt";

        static string[] filters = new string[]{
            "MenuControllers",
            "GameNote(Clone)",
            "BeatmapObjectSpawnController",
            "ControllerLeft",
            "ControllerRight",
            "PauseMenuVRLaserPointer",
            "VRCursor",
            "LevelFailedTextEffect",
            "RightHandController",
            "LeftHandController",
            "SaberClashChecker",
            "GamePauseManager",
            "GameEnergyCounter",
            "LevelTransitions",
            "LevelFinishedController",
            "RestartLevel",
            "PauseMenu",
            "Resume",
            "ReturnToMenu",
            "Buttons",
            "Button",
            "MenuButton",
            "RestartButton",
            "ContinueButton",
            "_CustomSaber(Clone)",
            "Saber Loader",
        };

        private Task TiltScene()
        {
            return Task.Run(() =>
            {
                //File.WriteAllText(fullPath, "");
                foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
                {
                    if (!obj.activeInHierarchy) continue;
                    if (!avoidFilters) //If it is some important objects (Notes, obstacles, sabers), dont fuck them up.
                    {
                        if (filters.Contains(obj.name)) continue;
                        if (obj.GetComponent<NoteData>() != null) continue;
                        if (obj.GetComponent<NoteFloorMovement>() != null) continue;
                        if (obj.GetComponent<NoteController>() != null) continue;
                        if (obj.GetComponent<Saber>() != null) continue;
                        if (obj.GetComponent<PlayerController>() != null) continue;
                        if (obj.GetComponent<PlayerDynamicData>() != null) continue;
                        if (obj.GetComponent<ObstacleData>() != null) continue;
                        if (obj.GetComponent<StretchableObstacle>() != null) continue;
                        if (obj.GetComponent<ObstacleController>() != null) continue;
                        if (!includeCameras)
                            if (obj.GetComponent<Camera>() != null) continue;
                    }
                    //File.AppendAllText(fullPath, Environment.NewLine + obj.name);
                    Vector3 rotationEuler = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
                    rotationEuler *= tiltScalar;
                    obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.eulerAngles + rotationEuler);
                }
            });
        }

        private void RecursiveTilt(GameObject parent)
        {
            for (var i = 0; i < parent.transform.childCount; i++)
            {
                GameObject obj = parent.transform.GetChild(i).gameObject;
                if (!obj.activeInHierarchy) continue;
                if (!avoidFilters) //If it is some important objects (Notes, obstacles, sabers), dont fuck them up.
                {
                    if (obj.GetComponent<NoteData>() != null) continue;
                    if (obj.GetComponent<NoteController>() != null) continue;
                    if (obj.GetComponent<Saber>() != null) continue;
                    if (obj.GetComponent<ObstacleData>() != null) continue;
                    if (obj.GetComponent<ObstacleController>() != null) continue;
                    if (!includeCameras)
                        if (obj.GetComponent<Camera>() != null) continue;
                }
                //File.AppendAllText(fullPath, Environment.NewLine + obj.name);
                Vector3 rotationEuler = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
                rotationEuler *= tiltScalar;
                obj.transform.rotation = Quaternion.Euler(obj.transform.rotation.eulerAngles + rotationEuler);
                if (obj.transform.childCount > 0)
                {
                    RecursiveTilt(obj); //LETS GET SOME RECURSION BOIIIS
                }
            }
        }
    }
}
