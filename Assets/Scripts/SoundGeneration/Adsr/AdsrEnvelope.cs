using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkArtsStudios.SoundGenerator.Module;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Assets.Scripts.SoundGeneration.Adsr
{
    class AdsrEnvelope : BaseModule
    {
        public Vector2 attack;

        public Vector2 decay;

        public Vector2 sustain;

        public Vector2 release;

        private BaseModule Input => this.GetInput();

        public override void InitializeAttributes()
        {
            attributes.Add(new Attribute("Input", _hiddenValue: true));
        }

        public override float OnAmplitude(float frequency, float time, float duration, int depth)
        {
            var result = Input.amplitude(frequency, time, duration, depth);

            result *= TestEnvelope(time);

            return result;
        }

        private float TestEnvelope(float time)
        {
            Debug.Log(time);
            return 0.0f;
        }

        private float Envelope(float time)
        {
            if (time > attack.X && time <= decay.X)
                return time * attack.Y;

            if (time > decay.X && time <= sustain.X)
                return time * decay.Y;

            if (time > sustain.X && time <= release.X)
                return time * sustain.Y;

            
            return time * release.Y;


        }
    }
}
