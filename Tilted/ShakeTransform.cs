using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tilted
{
    public class ShakeTransform : MonoBehaviour
    {
        public class ShakeEvent
        {
            float duration;
            float timeRemaining;
            ShakeDetail data;
            Vector3 noiseOffset;
            public Vector3 noise;

            public ShakeEvent(ShakeDetail data)
            {
                this.data = data;
                duration = data.amplitude;
                timeRemaining = duration;

                float rand = 32.0f;
                noiseOffset.x = UnityEngine.Random.Range(0, rand);
                noiseOffset.y = UnityEngine.Random.Range(0, rand);
                noiseOffset.z = UnityEngine.Random.Range(0, rand);
            }

            public void Update()
            {
                timeRemaining -= Time.deltaTime;
                float noiseOffsetDelta = Time.deltaTime * data.frequency;
                noiseOffset.x += noiseOffsetDelta;
                noiseOffset.y += noiseOffsetDelta;
                noiseOffset.z += noiseOffsetDelta;

                noise.x = Mathf.PerlinNoise(noiseOffset.x, 0);
                noise.y = Mathf.PerlinNoise(noiseOffset.y, 0);
                noise.z = Mathf.PerlinNoise(noiseOffset.z, 0);
                noise -= Vector3.one * 0.5f;

                noise *= data.amplitude;

                float agePercent = 1.0f - (timeRemaining / duration);
                noise *= data.blendOverLifetime.Evaluate(agePercent);
            }

            public bool isAlive()
            {
                return timeRemaining > 0.0f;
            }
        }

        List<ShakeEvent> events = new List<ShakeEvent>();
        public void AddShakeEvent(ShakeDetail data)
        {
            events.Add(new ShakeEvent(data));
        }

        public void AddShakeEvent(float scalar)
        {
            ShakeDetail data = new ShakeDetail(scalar);
            events.Add(new ShakeEvent(data));
        }

        void LateUpdate()
        {
            Vector3 posOffset = Vector3.zero;
            Vector3 rotOffset = Vector3.zero;
            for (var i = events.Count - 1; i != -1; i--)
            {
                ShakeEvent sc = events[i];
                sc.Update();

                posOffset += sc.noise;
                rotOffset += sc.noise;

                if (!sc.isAlive()) events.RemoveAt(i);
            }

            //transform.localPosition = posOffset;
            transform.localEulerAngles = rotOffset;
        }
    }
}
