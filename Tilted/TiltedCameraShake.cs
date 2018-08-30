using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tilted
{
    class TiltedCameraShake : MonoBehaviour
    {
        private float scalar = Config.load().scalar;

        private ScoreMultiplierUIController multi;
        private ScoreController score;
        private PlayerHeadAndObstacleInteraction ob;

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
                    ob = Resources.FindObjectsOfTypeAll<PlayerHeadAndObstacleInteraction>().FirstOrDefault();
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
            score.noteWasMissedEvent += OnNoteMissed;
        }

        private async void OnNoteCut(NoteData data, NoteCutInfo info, int c)
        {
            if (data.noteType == NoteType.Bomb || !info.allIsOK) await TiltScene();
        }

        private async void OnNoteMissed(NoteData data, int c)
        {
            if (data.noteType != NoteType.Bomb) await TiltScene();
        }

        private Task TiltScene()
        {
            return Task.Run(() =>
            {
                foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    if (obj.GetComponent<Camera>() != null)
                    {
                        if (obj.GetComponent<ShakeTransform>() == null)
                            obj.AddComponent<ShakeTransform>();
                        obj.GetComponent<ShakeTransform>().AddShakeEvent(new ShakeDetail(2, scalar));
                    }
                    if (obj.transform.childCount > 0)
                    {
                        RecursiveTilt(obj); //LETS GET SOME RECURSION BOIIIS
                    }
                }
            });
        }

        private void RecursiveTilt(GameObject parent)
        {
            for (var i = 0; i < parent.transform.childCount; i++)
            {
                GameObject obj = parent.transform.GetChild(i).gameObject;
                if (obj.GetComponent<Camera>() != null)
                {
                    if (obj.GetComponent<ShakeTransform>() == null)
                        obj.AddComponent<ShakeTransform>();
                    obj.GetComponent<ShakeTransform>().AddShakeEvent(new ShakeDetail(2, scalar));
                }
                if (obj.transform.childCount > 0)
                {
                    RecursiveTilt(obj); //LETS GET SOME RECURSION BOIIIS
                }
            }
        }
    }
}
