using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tilted
{
    public class ShakeDetail
    {
        public float amplitude = 1.0f;
        public float frequency = 1.0f;
        public AnimationCurve blendOverLifetime = new AnimationCurve(
            new Keyframe(0.0f, 0.0f, Mathf.Deg2Rad * 0.0f, Mathf.Deg2Rad * 720.0f),
            new Keyframe(0.2f, 1.0f),
            new Keyframe(1.0f, 0.0f)
        );

        public ShakeDetail(float amp, float scalar)
        {
            amplitude = amp;
            frequency = scalar;
        }
    }
}
